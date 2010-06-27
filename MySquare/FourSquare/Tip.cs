using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    class Tip
    {
        [JsonProperty("id")]
        public int Id
        { get; set; }

        [JsonProperty("text")]
        public string Text
        { get; set; }

        [JsonProperty("created")]
        public string Created
        { get; set; }


        [JsonProperty("user")]
        public User User
        { get; set; }


    }
}
