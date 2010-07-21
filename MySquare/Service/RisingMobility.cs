﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Newtonsoft.Json;

namespace MySquare.Service
{
    class RisingMobility : Service.Network
    {
        protected override Type GetJsonType(int key)
        {
            return typeof(AdEventArgs);
        }

        internal const string adService = "http://risingmobility.com/admob.ashx";

        internal void GetAd(double? latitude, double? longitude, string[] keywords)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("u", Configuration.Cookie);
            if (keywords != null && keywords.Length > 0)
                param.Add("k", string.Join(",", keywords));

            if (latitude.HasValue && longitude.HasValue)
            {
                param.Add("lat", latitude.Value.ToString(culture));
                param.Add("lng", longitude.Value.ToString(culture));
            }
            base.Post(0, adService, false, null, null, param);
        }



//        internal const string adMob = "http://r.admob.com/ad_source.php";
//        internal const string appId = "a14c41b27254abd";
//        internal void GetAd(double? latitude, double? longitude, string[] keywords)
//        {
//            Dictionary<string, string> param = new Dictionary<string, string>();
//            param.Add("rt", "0"); //ad request
//            param.Add("u", UserAgent); //user-agent
//            param.Add("i", "127.0.0.1"); //ip??
//            param.Add("o", Configuration.Cookie);
             
//#if DEBUG
//            //param.Add("m", "test"); // test mode
//#endif
//            param.Add("ma", "wml");
//            param.Add("f", "html_no_js");
//            param.Add("y", "banner");
//            if (keywords != null && keywords.Length > 0)
//                param.Add("k", string.Join(" ", keywords));


//            param.Add("s", appId);

//            if (latitude.HasValue && longitude.HasValue)
//                param.Add("d[coord]", latitude.Value.ToString(culture) + "," + longitude.Value.ToString(culture));


//            base.Post(0, adMob, true, null, null, param);
//        }

        protected override void OnResult(object result)
        {
            OnAdArrived(result as AdEventArgs);
            //if (!string.IsNullOrEmpty(result as string))
            //{
            //    string r = ((string)result).Trim();
            //    if (r != string.Empty && r.StartsWith("<"))
            //    {
            //        XmlDocument doc = new XmlDocument();
            //        doc.LoadXml(r);

            //        AdEventArgs args = new AdEventArgs();
            //        if (doc.DocumentElement != null && doc.DocumentElement.Name == "a")
            //        {
            //            args.Link = doc.DocumentElement.Attributes["href"].Value;
            //            args.Text = doc.DocumentElement.InnerText;
            //            OnAdArrived(args);
            //        }
            //    }
            //}
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

        [JsonProperty("link")]
        public string Link
        { get; set; }

        [JsonProperty("text")]
        public string Text
        { get; set; }

        [JsonProperty("image")]
        public byte[] Image
        { get; set; }
    }
}
