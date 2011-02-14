using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    class BadgeNotification: INotification
    {
        [JsonProperty("id")]
        public string Id
        { get; set; }

        [JsonProperty("name")]
        public string Name
        { get; set; }

        [JsonProperty("icon")]
        public string ImageUrl
        { get; set; }

        [JsonProperty("description")]
        public string Description
        { get; set; }


        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Description);
        }
    }


}
