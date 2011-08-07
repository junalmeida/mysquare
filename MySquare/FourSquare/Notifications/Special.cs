using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{

    enum SpecialType
    {
        mayor, count, frequency, flash, other
    }

    enum SpecialKind
    {
        here,
        nearby
    }

    class Special : INotification
    {
        [JsonProperty("id")]
        public string Id
        { get; set; }


        [JsonProperty("type")]
        public SpecialType Type
        { get; set; }

        [JsonProperty("kind")]
        public SpecialKind Kind
        { get; set; }

        [JsonProperty("message")]
        public string Message
        { get; set; }

        [JsonProperty("venue")]
        public Venue Venue
        { get; set; }
    }
}
