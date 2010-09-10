using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Reflection;

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

        Dictionary<int, HttpWebRequest> requests = new Dictionary<int, HttpWebRequest>();
        public void Abort()
        {
            foreach (int key in requests.Keys.ToArray())
            {
                Abort(key);
            }
        }

        private void Abort(int service)
        {
            lock (this)
            {
                if (requests.ContainsKey(service))
                {
                    var request = requests[service];
                    Debug.WriteLine("Aborting " + request.Address.ToString());
                    //RequestAbortException ex = new RequestAbortException(request.Address.ToString(), null);

                    requests.Remove(service);
                    request.Abort();
                    //Tenor.Mobile.Network.WebRequest.Abort(request);
                    //OnError(new ErrorEventArgs(ex));
                }
            }
        }

        protected void Post(int service, string url, bool isPost, string Login, string Password, Dictionary<string, string> parameters)
        {
            bool auth = !string.IsNullOrEmpty(Login);

            StringBuilder queryString = new StringBuilder();

            if (parameters != null)
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


            HttpWebRequest request;
            if (isPost)
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


            lock (this)
            {
                Abort(service);
                requests.Add(service, request);
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
            string resourceFile = url.Substring(url.LastIndexOf("/") + 1).Replace(".json", "Test.txt").Replace("me?", "leaderboardTest.txt?");
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
            if (isPost)
            {
                MemoryStream memData = new MemoryStream();
                StreamWriter writer = new StreamWriter(memData);
                if (queryString.Length > 0)
                    writer.Write(queryString.Remove(0, 1).ToString());
                writer.Flush();
                request.ContentLength = memData.Length;

                memData.Seek(0, SeekOrigin.Begin);

                request.BeginGetRequestStream(
                    new AsyncCallback(WriteRequest),
                    new object[] { request, service, memData });
            }
            else
            {
                request.BeginGetResponse(new AsyncCallback(ParseResponse), new object[] { request, service });
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
                    StreamReader networkReader = new StreamReader(stream);
                    string responseTxt = networkReader.ReadToEnd();

                    Type type = GetJsonType(service);
                    if (responseTxt.StartsWith("{ \"error\""))
                    {
                        type = typeof(ErrorEventArgs);
                    }
                    if (type == null)
                    {
                        result = responseTxt;
                    }
                    else
                    {

                        Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                        //Newtonsoft.Json.JsonReader reader = new Newtonsoft.Json.JsonTextReader(new StreamReader(stream));
                        Newtonsoft.Json.JsonReader reader = new Newtonsoft.Json.JsonTextReader(new StringReader(responseTxt));
                        result = serializer.Deserialize(reader, type);
                    }

                    if (responseTxt.StartsWith("{ \"error\""))
                    {
                        ((ErrorEventArgs)result).Exception = new ServerException(((ErrorEventArgs)result).Message);
                    }

                }

            }
            catch (Exception ex)
            {

                OnError(new ErrorEventArgs(new Exception(
                    string.Format("Request on {0} failed.", file), ex)));
                return;
            }

            if (result != null)
                OnResult(result, service);
            else
                OnError(new ErrorEventArgs(new Exception("Invalid response.")));

        }
#endif



        private void WriteRequest(IAsyncResult r)
        {
            object[] data = (object[])r.AsyncState;
            HttpWebRequest request = (HttpWebRequest)data[0]; 
            int service = (int)data[1];
            MemoryStream memData = (MemoryStream)data[2];

            try
            {
                Stream postData = null;
                postData = request.EndGetRequestStream(r);


                memData.Seek(0, SeekOrigin.Begin);
                memData.WriteTo(postData);
                postData.Close();

                request.BeginGetResponse(new AsyncCallback(ParseResponse), new object[] { request, service });
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
        protected abstract void OnResult(object result, int key);

        private void ParseResponse(IAsyncResult r)
        {
            object[] data = (object[])r.AsyncState;

            HttpWebRequest request = (HttpWebRequest)data[0];
            int service = (int)data[1];
            if (request != null)
            {
                HttpWebResponse response = null;
                string responseTxt = null;
                object result = null;
                try
                {
                    try
                    {
                        response = (HttpWebResponse)request.EndGetResponse(r);
                    }
                    catch (ArgumentException ex)
                    {
                        Log.RegisterLog(ex);
                        return;
                    }
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            StreamReader networkReader = new StreamReader(stream);
                            responseTxt = networkReader.ReadToEnd();

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
                                if (responseTxt.StartsWith("{ \"error\""))
                                {
                                    type = typeof(ErrorEventArgs);
                                }

                                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                                //Newtonsoft.Json.JsonReader reader = new Newtonsoft.Json.JsonTextReader(new StreamReader(stream));
                                Newtonsoft.Json.JsonReader reader = new Newtonsoft.Json.JsonTextReader(new StringReader(responseTxt));
                                result = serializer.Deserialize(reader, type);

                                if (responseTxt.StartsWith("{ \"error\""))
                                {
                                    ((ErrorEventArgs)result).Exception = new ServerException(((ErrorEventArgs)result).Message);
                                }
                            }

                            if (this.GetType() != typeof(RisingMobilityService))
                            {
                                Log.RegisterLog("data", new Exception(
                                    "Request address: " + request.Address.ToString() + "\r\n" +
                                    "Request output: " + responseTxt));
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
                    {
                        response.Close();
                        (response as IDisposable).Dispose();
                    }
                    response = null;

                    lock (this)
                    {
                        if (requests.ContainsKey(service))
                        {
                            if (requests[service] == request)
                                requests.Remove(service);
                        }
                    }
                    request = null;
                }

                if (result != null)
                    OnResult(result, service);
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
            byte[] result = null;
            if (cache)
                result = GetFromCache(url);
            if (result == null)
            {
                System.Diagnostics.Debug.WriteLine(url, "ImageDownload");
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
                catch (Exception ex)
                {
                    ex = new Exception("Cannot download " + url, ex);
                    Log.RegisterLog("image-service", ex);
                }
            }
            return result;
        }

        #endregion



        #region Cache
        internal static void CheckCacheFiles()
        {
            if (Configuration.IsFirstTime)
            {
                string appPath = Configuration.GetAppPath();
                string path = System.IO.Path.Combine(appPath, "cache");
                if (System.IO.Directory.Exists(path))
                {
                    foreach (string ext in new string[] { "*.jpg", "*.png", "*.gif" })
                    {
                        string[] files = System.IO.Directory.GetFiles(path, ext);
                        if (files != null && files.Length > 0)
                        {
                            foreach (string file in files)
                            {
                                System.IO.File.Move(file, file + ".cache");
                            }
                        }
                    }
                }
            }

        }

        internal static string GetCachePath(string url)
        {
            string appPath = Configuration.GetAppPath();
            string path = System.IO.Path.Combine(appPath, "cache");
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            string filePath = url.Substring(url.IndexOf(".com/") + 5).Replace("/", "_");
            filePath = System.IO.Path.Combine(path, filePath);
            filePath += ".cache";
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

        protected static string UrlEncode(string instring)
        {
            StringReader strRdr = new StringReader(instring);
            StringWriter strWtr = new StringWriter();
            int charValue = strRdr.Read();
            while (charValue != -1)
            {
                if (((charValue >= 48) && (charValue
        <= 57)) // 0-9
                || ((charValue >= 65) && (charValue
        <= 90)) // A-Z
                || ((charValue >= 97) && (charValue
        <= 122))) // a-z
                {
                    strWtr.Write((char)charValue);
                }
                else if (charValue == 32)  // Space
                {
                    strWtr.Write("+");
                }
                else
                {
                    strWtr.Write("%{0:x2}", charValue);
                }
                charValue = strRdr.Read();
            }
            return strWtr.ToString();
        }


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
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs()
        {
        }

        public ErrorEventArgs(Exception ex)
        {
            this.Exception = ex;
        }

        Exception exception;
        internal Exception Exception
        {
            get { return exception; }
            set
            {
                exception = value;
                Message = value.Message;
            }
        }

        [JsonProperty("error")]
        internal string Message
        { get; set; }

    }

    class ServerException : Exception
    {
        public ServerException() : base()
        {
        }
        public ServerException(string message)
            : base(message)
        {
        }
    }
}
