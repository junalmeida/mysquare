using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using System.Reflection;

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

        protected override void OnResult(object result)
        {
            OnAdArrived(result as AdEventArgs);
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
    [Obfuscation(Exclude=true, ApplyToMembers=true)]
    class AdEventArgs : EventArgs
    {
        public AdEventArgs() { }

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
