using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace MySquare.FourSquare
{
    class Service
    {
        enum ServiceResource
        {
            SearchNearby
        }

        internal Service()
        {
            var asName = typeof(Service).Assembly.GetName();
            userAgent = string.Format("{0}/{1}", asName.Name, asName.Version);
        }

        private System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo("en-us");
        private string userAgent;
        private string userName;
        private string passWord;
        int limit = 20;

        HttpWebRequest request = null;
        private void Post(ServiceResource service, Dictionary<string, string> parameters)
        {
            string url = null;

            switch (service)
            {
                case ServiceResource.SearchNearby:
                    url = "http://api.foursquare.com/v1/venues.json";
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

            if (queryString.Length > 0)
                url += "?" + queryString.Remove(0, 1).ToString();

            request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = userAgent;
            request.Method = "GET";
            if (!string.IsNullOrEmpty(userName))
                request.Headers.Add(string.Format("{0}:{1}", userName, passWord));

            request.BeginGetResponse(new AsyncCallback(ParseResponse), service);
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
                                        type = typeof(Venues);
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
                    OnError(new EventArgs());
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
                            Venues venues = (Venues)result;
                            Venue[] venueList = new Venue[] { };
                            if (venues.Groups.Length > 0)
                                venueList = venues.Groups[0].Venues;
                            OnSearchArrives(new SearchEventArgs(venueList));
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                }
                else
                    OnError(new EventArgs());

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

        #region Events

        internal event EventHandler Error;
        private void OnError(EventArgs e)
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

}
