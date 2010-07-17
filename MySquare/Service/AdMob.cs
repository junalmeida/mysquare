using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace MySquare.Service
{
    class AdMob : Service.Network
    {
        protected override Type GetJsonType(int key)
        {
            return null;
        }


        internal const string adMob = "http://r.admob.com/ad_source.php";
        internal const string appId = "a14c41b27254abd";
        internal void GetAd(double? latitude, double? longitude, string[] keywords)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("rt", "0"); //ad request
            param.Add("u", UserAgent); //user-agent
            param.Add("i", "127.0.0.1"); //ip??
             
#if DEBUG
            //param.Add("m", "test"); // test mode
#endif
            param.Add("ma", "wml");
            param.Add("f", "html_no_js");
            param.Add("y", "banner");
            if (keywords != null && keywords.Length > 0)
                param.Add("k", string.Join(" ", keywords));


            param.Add("s", appId);

            if (latitude.HasValue && longitude.HasValue)
                param.Add("d[coord]", latitude.Value.ToString(culture) + "," + longitude.Value.ToString(culture));


            base.Post(0, adMob, true, null, null, param);
        }

        protected override void OnResult(object result)
        {
            if (!string.IsNullOrEmpty(result as string))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml((string)result);

                AdEventArgs args = new AdEventArgs();
                if (doc.DocumentElement != null && doc.DocumentElement.Name == "a")
                {
                    args.Link = doc.DocumentElement.Attributes["href"].Value;
                    args.Text = doc.DocumentElement.InnerText;
                    OnAdArrived(args);
                }

            }
        }

        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
        }


        public event AdEventHandler AdArrived;
        private void OnAdArrived(AdEventArgs e)
        {
            if (AdArrived != null)
                AdArrived(this, e);
        }


        
    }

    delegate void AdEventHandler(object sender, AdEventArgs e);
    class AdEventArgs : EventArgs
    {
        internal AdEventArgs() { }

        public string Text { get; set; }
        public string Link { get;  set; }
    }
}
