using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.Win32;
using System.Threading;

namespace MySquare.FourSquare
{
    class Service
    {
        #region Settings
        private RegistryKey key;
        public string Login
        {
            get
            {
                return (string)key.GetValue("login", null);
            }
            set
            {
                key.SetValue("login", value);
            }
        }

        public string Password
        {
            get
            {
                return (string)key.GetValue("password", null);
            }
            set
            {
                key.SetValue("password", value);
            }
        }



        #endregion


        enum ServiceResource
        {
            SearchNearby,
            CheckIn,
            Venue,
            AddTip
        }

        internal Service()
        {
            string keyPath = "Software\\RisingMobility\\MySquare";
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
                key = Registry.LocalMachine.CreateSubKey(keyPath);
            else
                key = Registry.CurrentUser.CreateSubKey(keyPath);


            var asName = typeof(Service).Assembly.GetName();
            userAgent = string.Format("{0}/{1}", asName.Name, asName.Version);
        }

        private System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-us");
        private string userAgent;
        int limit = 20;



        HttpWebRequest request = null;
        internal void Abort()
        {
            if (request != null)
            {
                Tenor.Mobile.Network.WebRequest.Abort(request);
                request = null;
                OnError(new ErrorEventArgs(new Exception("The request was cancelled.")));
            }
        }

        private void Post(ServiceResource service, Dictionary<string, string> parameters)
        {
            if (request != null)
                return;

            string url = null;
            bool post = false;
            bool auth = false;

            switch (service)
            {
                case ServiceResource.SearchNearby:
                    url = "http://api.foursquare.com/v1/venues.json";
                    break;
                case ServiceResource.CheckIn:
                    url = "http://api.foursquare.com/v1/checkin.json";
                    post = true; auth = true;
                    break;
                case ServiceResource.Venue:
                    url = "http://api.foursquare.com/v1/venue.json";
                    auth = true;
                    break;
                case ServiceResource.AddTip:
                    url = "http://api.foursquare.com/v1/addtip.json";
                    auth = true; post = true;
                    break;
                default:
                    throw new NotImplementedException();
            }

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

        private void WriteRequest(IAsyncResult r)
        {
            object[] data = (object[])r.AsyncState;
            ServiceResource service = (ServiceResource)data[0];
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

        private void ParseResponse(IAsyncResult r)
        {
            if (request != null)
            {
                ServiceResource service = (ServiceResource)r.AsyncState;
                object result = null;
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(r))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (Stream stream = response.GetResponseStream())
                            {
                                Type type = null;
                                switch (service)
                                {
                                    case ServiceResource.SearchNearby:
                                        type = typeof(SearchResponse);
                                        break;
                                    case ServiceResource.CheckIn:
                                        type = typeof(CheckInResponse);
                                        break;
                                    case ServiceResource.Venue:
                                        type = typeof(VenueResponse);
                                        break;
                                    case ServiceResource.AddTip:
                                        type = typeof(TipResponse);
                                        break;  
                                    default:
                                        throw new NotImplementedException();
                                }

                                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                                Newtonsoft.Json.JsonReader reader = new Newtonsoft.Json.JsonTextReader(new StreamReader(stream));
                                result = serializer.Deserialize(reader, type);

                            }

                        }
                    }
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
                    request = null;
                }

                if (result != null)
                {

                    switch (service)
                    {
                        case ServiceResource.SearchNearby:
                            SearchResponse venues = (SearchResponse)result;
                            Venue[] venueList = new Venue[] { };
                            if (venues.Groups.Length > 0)
                                venueList = venues.Groups[0].Venues;
                            OnSearchArrives(new SearchEventArgs(venueList));
                            break;
                        case ServiceResource.CheckIn:
                            OnCheckInResult(new CheckInEventArgs(((CheckInResponse)result).CheckIn));
                            break;
                        case ServiceResource.Venue:
                            OnVenueResult(new VenueEventArgs(((VenueResponse)result).Venue));
                            break;
                        case ServiceResource.AddTip:
                            OnAddTipResult(new TipEventArgs(((TipResponse)result).Tip));
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                }
                else
                    OnError(new ErrorEventArgs(new Exception("Invalid response.")));

            }
        }

    

        internal void SearchNearby(string search, double lat, double lgn)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("geolat", lat.ToString(culture));
            parameters.Add("geolong", lgn.ToString(culture));
            parameters.Add("l", limit.ToString(culture));
            if (!string.IsNullOrEmpty(search))
                parameters.Add("q", search);

            Post(ServiceResource.SearchNearby, parameters);
        }

        internal void CheckIn(Venue venue, string shout, bool tellFriends, bool? facebook, bool? twitter)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("vid", venue.Id.ToString());
            if (!string.IsNullOrEmpty(shout))
                parameters.Add("shout", shout);
            parameters.Add("private", Convert.ToInt32((!tellFriends)).ToString());
            if (twitter.HasValue)
                parameters.Add("twitter", Convert.ToInt32(twitter.Value).ToString());
            if (facebook.HasValue)
                parameters.Add("facebook", Convert.ToInt32(facebook.Value).ToString());


            Post(ServiceResource.CheckIn, parameters);
        }

        internal void GetVenue(int vid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("vid", vid.ToString());

            Post(ServiceResource.Venue, parameters);
        }


        internal void AddTip(int vid, string text)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("vid", vid.ToString());
            parameters.Add("text", text.ToString());
            //parameters.Add("geolat", lat.ToString(culture));
            //parameters.Add("geolong", lng.ToString(culture));

            Post(ServiceResource.AddTip, parameters);
        }


        #region Image Service
        internal byte[] DownloadImageSync(string url)
        {
            System.Diagnostics.Debug.WriteLine(url, "ImageDownload");
            byte[] result = GetFromCache(url);
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
                                SaveToCache(url, result);
                            }
                        }
                    }
                }
                catch { }
            }
            return result;
        }


        internal void DownloadImage(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            System.Diagnostics.Debug.WriteLine(url, "ImageDownload");
            request.BeginGetResponse(
                new AsyncCallback(ParseImageResponse),
                new object[] { request, url });
        }

        private void ParseImageResponse(IAsyncResult r)
        {

            object[] items = (object[])r.AsyncState;

            HttpWebRequest request = (HttpWebRequest)items[0];
            string url = (string)items[1];

            byte[] result = null;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(r))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            result = Tenor.Mobile.IO.StreamToBytes(stream);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                request = null;
            }

            if (result != null)
                OnImageResult(new ImageEventArgs(url, result));
        }

        #endregion

        #region Cache

        static string appPath;
        private static string GetCachePath(string url)
        {
            if (string.IsNullOrEmpty(appPath))
            {
                appPath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                if (appPath.StartsWith("file://"))
                    appPath = appPath.Substring(8).Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
                appPath = System.IO.Path.GetDirectoryName(appPath);

            }
            string path = System.IO.Path.Combine(appPath, "cache");
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            string filePath = url.Substring(url.IndexOf(".com/") + 5).Replace("/", "_");
            filePath = System.IO.Path.Combine(path, filePath);
            return filePath;
        }

        private static byte[] GetFromCache(string url)
        {
            string path = Service.GetCachePath(url);
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
                string path = Service.GetCachePath(url);
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

        #region Events
      internal event AddTipEventHandler AddTipResult;
        private void OnAddTipResult(TipEventArgs e)
        {
            if (AddTipResult != null)
            {
                AddTipResult(this, e);
            }
        }

        internal event ImageResultEventHandler ImageResult;
        private void OnImageResult(ImageEventArgs e)
        {
            if (ImageResult != null)
            {
                ImageResult(this, e);
            }
        }

        internal event VenueEventHandler VenueResult;
        private void OnVenueResult(VenueEventArgs e)
        {
            if (VenueResult != null)
            {
                e.Venue.fullData = true;
                VenueResult(this, e);
            }
        }


        internal event CheckInEventHandler CheckInResult;
        private void OnCheckInResult(CheckInEventArgs e)
        {
            if (CheckInResult != null)
                CheckInResult(this, e);
        }


        internal event ErrorEventHandler Error;
        private void OnError(ErrorEventArgs e)
        {
            if (Error != null)
                Error(this, e);
        }

        internal event SearchEventHandler SearchArrives;
        private void OnSearchArrives(SearchEventArgs e)
        {
            if (SearchArrives != null)
                SearchArrives(this, e);
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
    delegate void AddTipEventHandler(object serder, TipEventArgs e);
    class TipEventArgs : EventArgs
    {
        internal TipEventArgs(Tip tip)
        {
            this.Tip = tip;
        }

        internal Tip Tip
        {
            get;
            private set;
        }
    }
    delegate void VenueEventHandler(object serder, VenueEventArgs e);
    class VenueEventArgs : EventArgs
    {
        internal VenueEventArgs(Venue venue)
        {
            this.Venue = venue;
        }

        internal Venue Venue
        {
            get;
            private set;
        }
    }


    delegate void CheckInEventHandler(object serder, CheckInEventArgs e);
    class CheckInEventArgs : EventArgs
    {
        internal CheckInEventArgs(CheckIn checkIn)
        {
            this.CheckIn = checkIn;
        }

        internal CheckIn CheckIn
        {
            get;
            private set;
        }
    }

    delegate void SearchEventHandler(object serder, SearchEventArgs e);
    class SearchEventArgs : EventArgs 
    {
        internal SearchEventArgs(Venue[] venues)
        {
            this.Venues = venues;
        }

        internal Venue[] Venues
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
