﻿using System;
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
    class TipEventArgs : EventArgs
    {
        public TipEventArgs(Tip tip)
        {
            this.Tip = tip;
        }

        [JsonProperty("tip")]
        internal Tip Tip
        {
            get;
            private set;
        }
    }


    delegate void TipsEventHandler(object serder, TipsEventArgs e);
    class TipsEventArgs : EventArgs
    {
        public TipsEventArgs(Tip[] tip)
        {
            this.Tips = tip;
        }

        [JsonProperty("tips")]
        internal Tip[] Tips
        {
            get;
            private set;
        }
    }
}
