using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

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

        private string userAgent;
        private string userName;
        private string passWord;

        HttpWebRequest request = null;
        private void Post(ServiceResource service, Dictionary<string, string> parameters)
        {
            string url = null;

            switch (service)
            {
                case ServiceResource.SearchNearby:
                    url = "http://api.foursquare.com/v1/venues";
                    break;
                default:
                    break;
            }

            StringBuilder queryString = new StringBuilder();

            foreach (string key in parameters.Keys)
            {
                string value = parameters[key];
                if (!string.IsNullOrEmpty(value))
                {
                    queryString.Append("&");
                    queryString.Append(key);
                    queryString.Append(value);
                }
            }

            if (queryString.Length > 0)
                url += "?" + queryString.Remove(0, 1).ToString();

            request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.UserAgent = userAgent;
            request.Method = "GET";
            request.Headers.Add(string.Format("{0}:{1}", userName, passWord));

            request.BeginGetResponse(new AsyncCallback(ParseResponse), null);
        }

        private void ParseResponse(IAsyncResult r)
        {
            if (request != null)
            {
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(r);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream stream = response.GetResponseStream();
                        
                    }
                    else
                    {
                        OnError(new EventArgs());
                    }

                }
                finally
                {
                    request = null;
                }
            }
        }


        #region Events

        internal event EventHandler Error;
        private void OnError(EventArgs e)
        {
            if (Error != null)
                Error(this, e);
        }

        #endregion

    }
}
