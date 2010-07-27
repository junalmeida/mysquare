using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    delegate void CheckInEventHandler(object serder, CheckInEventArgs e);
    class CheckInEventArgs : EventArgs
    {
        [JsonProperty("checkin")]
        internal CheckIn CheckIn
        {
            get;
            private set;
        }
    }


    delegate void CheckInsEventHandler(object serder, CheckInsEventArgs e);
    class CheckInsEventArgs : EventArgs
    {
        [JsonProperty("checkins")]
        internal CheckIn[] CheckIns
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

        [JsonProperty("shout")]
        public string Shout
        { get; set; }

        [JsonProperty("display")]
        public string Display
        { get; set; }

        [JsonProperty("created")]
        public DateTime Created
        { get; set; }

        [JsonProperty("venue")]
        public Venue Venue
        { get; set; }

        [JsonProperty("user")]
        public User User
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

        public override string ToString()
        {
            return string.IsNullOrEmpty(Display) ? Shout : Display;
        }
    }
}
