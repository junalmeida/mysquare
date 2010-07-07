using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
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
        [JsonProperty("results")]
        public Geocode[] Geocodes
        { get; private set; }

        [JsonProperty("status")]
        public string Status
        { get; private set; }
    }
}
