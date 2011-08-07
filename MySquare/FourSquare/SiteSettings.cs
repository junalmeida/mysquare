using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    class SiteSettings
    {
        [JsonProperty("receivePings")]
        public bool ReceivePings { get; set; }
        [JsonProperty("receiveCommentPings")]
        public bool ReceiveCommentPings { get; set; }
        [JsonProperty("twitter")]
        public string Twitter { get; set; }
        [JsonProperty("sendToTwitter")]
        public bool SendToTwitter { get; set; }
        [JsonProperty("sendMayorshipsToTwitter")]
        public bool SendMayorshipsToTwitter { get; set; }
        [JsonProperty("sendBadgesToTwitter")]
        public bool SendBadgesToTwitter { get; set; }
        [JsonProperty("facebook")]
        public long Facebook { get; set; }
        [JsonProperty("sendToFacebook")]
        public bool SendToFacebook { get; set; }
        [JsonProperty("sendMayorshipsToFacebook")]
        public bool SendMayorshipsToFacebook { get; set; }
        [JsonProperty("sendBadgesToFacebook")]
        public bool SendBadgesToFacebook { get; set; }
        [JsonProperty("foreignConsent")]
        public string ForeignConsent { get; set; }
    }

    delegate void SiteSettingsEventHandler(object serder, SiteSettingsEventArgs e);
    class SiteSettingsEventArgs : EnvelopeEventArgs<SiteSettingsResult>
    {
        public SiteSettings SiteSettings
        {
            get { return this.Response.Settings; }
        }
    }

    class SiteSettingsResult
    {
        [JsonProperty("settings")]
        public SiteSettings Settings { get; set; }
    }
}
