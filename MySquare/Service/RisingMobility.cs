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
        enum ServiceKey
        {
            Ad,
            Premium
        }


        protected override Type GetJsonType(int key)
        {
            ServiceKey service = (ServiceKey)key;
            switch (service)
            {
                case ServiceKey.Ad:
                    return typeof(AdEventArgs);
                case ServiceKey.Premium:
                    return typeof(RisingMobilityEventArgs);
                default:
                    throw new NotImplementedException();
            }
        }

        internal const string adService = "http://risingmobility.com/admob.ashx";
        //internal const string rService = "http://risingmobility.com/mysquare/service.ashx";
        internal const string rService = "http://localhost:49617/mysquare/service.ashx";

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
            base.Post((int)ServiceKey.Ad, adService, false, null, null, param);
        }

        internal void GetPremiumInfo(string username)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            string token = DateTime.UtcNow.ToString("yyyy-MM");
            token += "||" + username;

            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] crypt = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(token));
            token = Convert.ToBase64String(crypt, 0, crypt.Length);

            param.Add("u", token);
            param.Add("t", ".prm");

            base.Post((int)ServiceKey.Premium, rService, true, null, null, param);
        }


        protected override void OnResult(object result, int key)
        {
            ServiceKey service = (ServiceKey)key;
            switch (service)
            {
                case ServiceKey.Ad:
                    OnAdArrived(result as AdEventArgs);
                    break;
                case ServiceKey.Premium:
                    OnPremiumArrived(result as RisingMobilityEventArgs);
                    break;
                default:
                    break;
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


        public event RisingMobilityEventHandler PremiumArrived;
        private void OnPremiumArrived(RisingMobilityEventArgs e)
        {
            if (PremiumArrived != null)
                PremiumArrived(this, e);
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



    delegate void RisingMobilityEventHandler(object sender, RisingMobilityEventArgs e);
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    class RisingMobilityEventArgs : EventArgs
    {
        [JsonProperty("result")]
        public byte[] Result
        { get; set; }
    }
}
