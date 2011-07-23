using System;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    class Tip
    {
        [JsonProperty("id")]
        public string Id
        { get; set; }


        [JsonProperty("text")]
        public string Text
        { get; set; }

        [JsonProperty("createdAt")]
        public DateTime Created
        { get; set; }


        [JsonProperty("user")]
        public User User
        { get; set; }

        [JsonProperty("venue")]
        public Venue Venue
        { get; set; }


    }

    delegate void AddTipEventHandler(object serder, TipEventArgs e);
    class TipEventArgs : EnvelopeEventArgs<TipResult>
    {
        internal Tip Tip
        {
            get { return Response.Tip; }
        }
    }

    class TipResult
    {
        [JsonProperty("tip")]
        internal Tip Tip
        {
            get;
            private set;
        }
    }


    delegate void TipsEventHandler(object serder, TipsEventArgs e);
    class TipsEventArgs : EnvelopeEventArgs<TipsResult>
    {

        public Tip[] Tips
        {
            get
            {
                return Response == null ? null : Response.Tips;
            }
        }
    }

    class TipsResult
    {
        [JsonProperty("tips")]
        public Tip[] Tips
        {
            get;
            private set;
        }

    }

    [JsonObject]
    class TipGroupCollection : System.Collections.ObjectModel.Collection<TipGroup>
    {
        [JsonProperty("count")]
        public int TotalTips
        { get; private set; }

        [JsonProperty("groups")]
        private TipGroup[] Groups
        {
            get
            {
                return null;
            }
            set
            {
                base.Items.Clear();

                foreach (var obj in value)
                    base.Items.Add(obj);
            }

        }


    }

    [JsonObject]
    class TipGroup : System.Collections.ObjectModel.Collection<Tip>
    {
        [JsonProperty("name")]
        public string Name
        { get; private set; }

        [JsonProperty("items")]
        private Tip[] Tips
        {
            get
            {
                return null;
            }
            set
            {
                base.Items.Clear();

                foreach (var obj in value)
                    base.Items.Add(obj);
            }

        }
    }
}
