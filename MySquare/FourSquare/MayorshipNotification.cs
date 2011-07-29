using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    class Mayorship : INotification
    {
        [JsonProperty("type")]
        public MayorType Type
        { get; set; }

        [JsonProperty("checkins")]
        public int CheckIns
        { get; set; }

        [JsonProperty("count")]
        public int Count
        { get; set; }


        [JsonProperty("user")]
        public User User
        { get; set; }

        [JsonProperty("message")]
        public string Message
        { get; set; }


    }

    enum MayorType
    {
        @new, nochange, stolen, none
    }
}
