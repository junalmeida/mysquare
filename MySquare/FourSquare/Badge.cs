using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    class Badge
    {
        [JsonProperty("id")]
        public int Id
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
    }

    class Score
    {
        [JsonProperty("points")]
        public int Points
        { get; set; }


        [JsonProperty("icon")]
        public string ImageUrl
        { get; set; }

        [JsonProperty("message")]
        public string Message
        { get; set; }
    }

    class Special
    {
        [JsonProperty("id")]
        public int Id
        { get; set; }


        [JsonProperty("type")]
        public string Type
        { get; set; }

        [JsonProperty("kind")]
        public string Kind
        { get; set; }

        [JsonProperty("message")]
        public string Message
        { get; set; }

        [JsonProperty("venue")]
        public Venue Venue
        { get; set; }
    }
}
