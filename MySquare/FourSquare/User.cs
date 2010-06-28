using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    class User
    {
        [JsonProperty("id")]
        public int Id
        { get; set; }

        [JsonProperty("firstname")]
        public string FirstName
        { get; set; }

        [JsonProperty("lastname")]
        public string LastName
        { get; set; }

        [JsonProperty("photo")]
        public string ImageUrl
        { get; set; }

        [JsonProperty("gender")]
        public string Gender
        { get; set; }

        public override string ToString()
        {
            return string.Concat(FirstName, " ", LastName);
        }
    }
}
