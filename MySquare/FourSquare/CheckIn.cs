using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    delegate void CheckInEventHandler(object serder, CheckInEventArgs e);
    class CheckInEventArgs : EnvelopeEventArgs<CheckInResult>
    {
        public CheckIn CheckIn
        {
            get
            {
                return Response == null ? null : Response.CheckIn;
            }
        }
    }

    class CheckInResult
    {
        [JsonProperty("checkin")]
        internal CheckIn CheckIn
        { get; private set; }
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

    enum CheckInType
    {
        checkin,
        shout,
        venueless
    }

    class CheckIn
    {
        [JsonProperty("id")]
        public string Id
        { get; set; }

        [JsonProperty("type")]
        public CheckInType Type
        { get; set; }

        [JsonProperty("private")]
        public bool IsPrivate
        { get; set; }

        [JsonProperty("shout")]
        public string Shout
        { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone
        { get; set; }

        [JsonProperty("createdAt")]
        public DateTime Created
        { get; set; }

        [JsonProperty("venue")]
        public Venue Venue
        { get; set; }

        [JsonProperty("user")]
        public User User
        { get; set; }

        [JsonProperty("message")]
        public string Message
        { get; set; }

        [JsonProperty("display")]
        public string Display
        { get; set; }


        [JsonProperty("mayor")]
        public MayorshipNotification Mayor
        { get; set; }

        [JsonProperty("badges")]
        public BadgeNotification[] Badges
        { get; set; }


        [JsonProperty("scores")]
        public ScoreNotification[] Scoring
        { get; set; }

        [JsonProperty("specials")]
        public SpecialNotification[] Specials
        { get; set; }

        [JsonProperty("ping")]
        public bool Ping
        { get; set; }
    }
}
