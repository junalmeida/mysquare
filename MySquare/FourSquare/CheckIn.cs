using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    class CheckInResponse
    {
        [JsonProperty("checkin")]
        public CheckIn CheckIn
        { get; set; }
    }


    delegate void CheckInEventHandler(object serder, CheckInEventArgs e);
    class CheckInEventArgs : EventArgs
    {
        internal CheckInEventArgs(CheckIn checkIn)
        {
            this.CheckIn = checkIn;
        }

        internal CheckIn CheckIn
        {
            get;
            private set;
        }
    }


    class CheckIn
    {
        [JsonProperty("id")]
        public int Id
        { get; set; }

        [JsonProperty("message")]
        public string Message
        { get; set; }

        [JsonProperty("venue")]
        public Venue Venue
        { get; set; }

        [JsonProperty("mayor")]
        public Mayor Mayor
        { get; set; }

        [JsonProperty("badges")]
        public Badge[] Badges
        { get; set; }


        [JsonProperty("scoring")]
        public Score[] Scoring
        { get; set; }

        [JsonProperty("specials")]
        public Special[] Specials
        { get; set; }
    }
}
