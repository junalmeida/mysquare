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

        string message;
        public string Message
        {
            get
            {
                if (message == null)
                {
                    foreach (var notif in Response.Notifications)
                    {
                        var msg = notif as MessageNotification;
                        if (msg != null)
                        {
                            message = msg.Message;
                            break;
                        }
                    }
                }
                return message;
            }
        }

        Badge[] badges;
        public Badge[] Badges
        {
            get
            {
                if (badges == null)
                {
                    List<Badge> list = new List<Badge>();
                    foreach (var notif in Response.Notifications)
                    {
                        var item = notif as Badge;
                        if (item != null)
                            list.Add(item);
                    }
                    badges = list.ToArray();
                }
                return badges;
            }
        }

        Score[] score;
        public Score[] Score
        {
            get
            {
                if (score == null)
                {
                    foreach (var notif in Response.Notifications)
                    {
                        var item = notif as ScoreNotification;
                        if (item != null)
                        {
                            score = item.ToArray();
                            break;
                        }
                    }
                }
                return score;
            }
        }

        Special[] specials;
        public Special[] Specials
        {
            get
            {
                if (specials == null)
                {
                    List<Special> list = new List<Special>();
                    foreach (var notif in Response.Notifications)
                    {
                        var item = notif as Special;
                        if (item != null)
                            list.Add(item);
                    }
                    specials = list.ToArray();
                }
                return specials;
            }
        }

        Mayorship mayorship;
        public Mayorship Mayorship
        {
            get
            {
                if (mayorship == null)
                {

                    foreach (var notif in Response.Notifications)
                    {
                        mayorship = notif as Mayorship;
                        if (mayorship != null)
                            break;
                    }
                }
                return mayorship;
            }
        }

    }

    class CheckInResult
    {
        [JsonProperty("checkin")]
        internal CheckIn CheckIn
        { get; private set; }

        [JsonProperty("notifications")]
        internal INotification[] Notifications { get; private set; }

    }

    delegate void CheckInsEventHandler(object serder, CheckInsEventArgs e);
    class CheckInsEventArgs : EnvelopeEventArgs<CheckInsResult>
    {
        internal CheckIn[] CheckIns
        { get { return this.Response.CheckIns; } }
    }

    class CheckInsResult
    {
        [JsonProperty("recent")]
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

        public override string ToString()
        {
            if (Venue != null)
                return string.Format("{0} @ {1}", User, Venue);
            else
                return string.Format("{0} shouted: {1}", User, Shout);
        }
        // photos and comments

    }
}
