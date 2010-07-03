using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    class GeocodeResponse
    {
        [JsonProperty("status")]
        public string Status
        { get; set; }

        [JsonProperty("results")]
        public Geocode[] Results
        { get; set; }
    }

    class Geocode
    {
        [JsonProperty("types")]
        public string[] Types
        { get; set; }

        [JsonProperty("formattedaddress")]
        public string FormattedAddress
        { get; set; }

        [JsonProperty("address_components")]
        public AddressComponent[] AddressComponents
        { get; set; }
    }

    class AddressComponent
    {
        [JsonProperty("long_name")]
        public string LongName
        { get; set; }

        [JsonProperty("short_name")]
        public string ShortName
        { get; set; }

        [JsonProperty("types")]
        public string[] Types
        { get; set; }

    }

    delegate void GeocodeEventHandler(object serder, GeocodeEventArgs e);
    class GeocodeEventArgs : EventArgs
    {
        internal GeocodeEventArgs(Geocode[] geocodes)
        {
            this.Geocodes = geocodes;
        }

        internal Geocode[] Geocodes
        {
            get;
            private set;
        }
    }
}
