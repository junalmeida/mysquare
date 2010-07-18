using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace MySquare.Service
{
    class RequestAbortException : Exception
    {
        public RequestAbortException(string url, Exception inner)
            : base (string.Format("The request on {0} was cancelled.", url), inner)
        {
        }
    }

    abstract class Network : IDisposable
    {

        public Network()
        {
            var asName = typeof(FourSquare).Assembly.GetName();
            userAgent = string.Format("{0}/{1}", asName.Name, asName.Version);

        }

        #region Events
        internal event ErrorEventHandler Error;
        protected virtual void OnError(ErrorEventArgs e)
        {
            if (Error != null)
                Error(this, e);
            else
                Log.RegisterLog(e.Exception);
        }

        #endregion

        protected readonly System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-us");


        private string userAgent;
        protected string UserAgent { get { return userAgent; } }

        HttpWebRequest request = null;
        public void Abort()
        {
            if (request != null)
            {
                RequestAbortException ex = new RequestAbortException(request.Address.ToString(), null);
                
                Tenor.Mobile.Network.WebRequest.Abort(request);
                request = null;
                OnError(new ErrorEventArgs(ex));
            }

        }

        protected void Post(int service, string url, bool post, string Login, string Password, Dictionary<string, string> parameters)
        {
            bool auth = !string.IsNullOrEmpty(Login);

            StringBuilder queryString = new StringBuilder();

            foreach (string key in parameters.Keys)
            {
                string value = parameters[key];
                if (!string.IsNullOrEmpty(value))
                {
                    queryString.Append("&");
                    queryString.Append(key);
                    queryString.Append("=");
                    queryString.Append(value);
                }
            }

            if (request != null)
                Abort();
            if (post)
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
            }
            else
            {
                if (queryString.Length > 0)
                    url += "?" + queryString.Remove(0, 1).ToString();
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
            }

            request.Timeout = 15000;
            request.UserAgent = userAgent;

            if (auth && !string.IsNullOrEmpty(Login))
            {
                request.Headers.Add("Authorization", "Basic " +
                    Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}",
                    Login, Password))));
            }


#if TESTING
            string resourceFile = url.Substring(url.LastIndexOf("/") + 1).Replace(".json", "Test.txt");
            int qs = resourceFile.IndexOf("?");
            if (qs > -1)
                resourceFile = resourceFile.Substring(0, qs);
            resourceFile = "MySquare.Service." + resourceFile;
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(resourceFile);
            if (stream != null)
            {
                Thread t = new Thread(new ThreadStart(delegate()
                {
                    ParseResponse((int)service, resourceFile, stream);
                }));
                t.Start();
                return;
            }
#endif
            if (post)
            {
                MemoryStream memData = new MemoryStream();
                StreamWriter writer = new StreamWriter(memData);

                writer.Write(queryString.Remove(0, 1).ToString());
                writer.Flush();
                request.ContentLength = memData.Length;

                memData.Seek(0, SeekOrigin.Begin);

                request.BeginGetRequestStream(
                    new AsyncCallback(WriteRequest),
                    new object[] { service, memData });
            }
            else
            {
                request.BeginGetResponse(new AsyncCallback(ParseResponse), service);
            }

        }

#if TESTING
        private void ParseResponse(int service, string file, Stream stream)
        {

            object result;
            try
            {
                using (stream)
                {

                    Type type = GetJsonType(service);


                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    Newtonsoft.Json.JsonReader reader = new Newtonsoft.Json.JsonTextReader(new StreamReader(stream));
                    result = serializer.Deserialize(reader, type);

                }

            }
            catch (Exception ex)
            {

                OnError(new ErrorEventArgs(new Exception(
                    string.Format("Request on {0} failed.", file), ex)));
                return;
            }

            if (result != null)
                OnResult(result);
            else
                OnError(new ErrorEventArgs(new Exception("Invalid response.")));

        }
#endif



        private void WriteRequest(IAsyncResult r)
        {
            object[] data = (object[])r.AsyncState;
            int service = (int)data[0];
            MemoryStream memData = (MemoryStream)data[1];

            try
            {
                Stream postData = request.EndGetRequestStream(r);
                memData.Seek(0, SeekOrigin.Begin);
                memData.WriteTo(postData);
                postData.Close();

                request.BeginGetResponse(new AsyncCallback(ParseResponse), service);

            }
            catch (Exception ex)
            {
                if (ex is WebException && ((WebException)ex).Status == WebExceptionStatus.ProtocolError)
                    OnError(new ErrorEventArgs(new UnauthorizedAccessException()));
                else
                    OnError(new ErrorEventArgs(ex));
                return;
            }
            finally
            {
                memData.Close();
            }
        }

        protected abstract Type GetJsonType(int key);
        protected abstract void OnResult(object result);

        private void ParseResponse(IAsyncResult r)
        {
            if (request != null)
            {
                int service = (int)r.AsyncState;
                string responseTxt = null;
                object result = null;
                HttpWebResponse response = null;
                try
                {
                    response = (HttpWebResponse)request.EndGetResponse(r);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            StreamReader networkReader = new StreamReader(stream);
                            responseTxt = networkReader.ReadToEnd();
                            System.Diagnostics.Trace.WriteLine(request.Address.ToString());
                            System.Diagnostics.Trace.WriteLine(responseTxt);

                            Type type = GetJsonType(service);
                            if (type == null)
                            {
#if DEBUG
                                if (string.IsNullOrEmpty(responseTxt))
                                    throw new WebException("Empty response.");
#endif
                                result = responseTxt;
                            }
                            else
                            {
                                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                                //Newtonsoft.Json.JsonReader reader = new Newtonsoft.Json.JsonTextReader(new StreamReader(stream));
                                Newtonsoft.Json.JsonReader reader = new Newtonsoft.Json.JsonTextReader(new StringReader(responseTxt));
                                result = serializer.Deserialize(reader, type);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    if (ex is WebException &&
                        ((WebException)ex).Status == WebExceptionStatus.ProtocolError
                        && ex.Message.IndexOf("401") > -1
                        )
                    {
                        OnError(new ErrorEventArgs(new UnauthorizedAccessException()));

                    }
                    else
                        OnError(new ErrorEventArgs(new Exception(
                            string.Format("Request on {0} failed.", request.Address), ex)));
                    return;
                }
                finally
                {
                    if (response != null)
                        (response as IDisposable).Dispose();
                    response = null;
                    request = null;
                }

                if (result != null)
                    OnResult(result);
                else
                    OnError(new ErrorEventArgs(new Exception("Invalid response.")));

            }
        }



        #region Image Service
        internal byte[] DownloadImageSync(string url)
        {
            return DownloadImageSync(url, true);
        }
        internal byte[] DownloadImageSync(string url, bool cache)
        {
            System.Diagnostics.Debug.WriteLine(url, "ImageDownload");
            byte[] result = null;
            if (cache)
                result = GetFromCache(url);
            if (result == null)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream stream = response.GetResponseStream())
                            {
                                result = Tenor.Mobile.IO.StreamToBytes(stream);
                                if (cache)
                                    SaveToCache(url, result);
                            }
                        }
                    }
                }
                catch { }
            }
            return result;
        }

        #endregion

        static string _appPath;
        //TODO: Move this to somewhere else
        internal static string GetAppPath()
        {
            if (string.IsNullOrEmpty(_appPath))
            {
                _appPath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                if (_appPath.StartsWith("file://"))
                    _appPath = _appPath.Substring(8).Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
                _appPath = System.IO.Path.GetDirectoryName(_appPath);

            }
            return _appPath;
        }


        #region Cache

        protected static string GetCachePath(string url)
        {
            string appPath = GetAppPath();
            string path = System.IO.Path.Combine(appPath, "cache");
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            string filePath = url.Substring(url.IndexOf(".com/") + 5).Replace("/", "_");
            filePath = System.IO.Path.Combine(path, filePath);
            return filePath;
        }

        private static byte[] GetFromCache(string url)
        {
            string path = GetCachePath(url);
            if (System.IO.File.Exists(path))
            {
                using (System.IO.FileStream file = System.IO.File.OpenRead(path))
                {
                    return Tenor.Mobile.IO.StreamToBytes(file);
                }
            }
            else
                return null;
        }

        private static void SaveToCache(string url, byte[] image)
        {
            try
            {
                string path = GetCachePath(url);
                using (var file = System.IO.File.Open(path, System.IO.FileMode.Create))
                {
                    file.Write(image, 0, image.Length);
                    file.Close();
                }
            }
            catch
            {
            }
        }

        #endregion



        #region IDisposable Members

        public void Dispose()
        {
            Abort();
        }

        #endregion
    }




    delegate void ImageResultEventHandler(object serder, ImageEventArgs e);
    class ImageEventArgs : EventArgs
    {
        internal ImageEventArgs(string url, byte[] image)
        {
            this.Url = url;
            this.Image = image;
        }
        internal string Url
        { get; private set; }

        internal byte[] Image
        {
            get;
            private set;
        }
    }

    delegate void ErrorEventHandler(object serder, ErrorEventArgs e);
    class ErrorEventArgs : EventArgs
    {
        internal ErrorEventArgs(Exception ex)
        {
            this.Exception = ex;
        }

        internal Exception Exception
        {
            get;
            private set;
        }
    }
}
