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
        public DateTime Created
        { get; set; }


        [JsonProperty("user")]
        public User User
        { get; set; }


    }

    delegate void AddTipEventHandler(object serder, TipEventArgs e);
    class TipEventArgs : EventArgs
    {
        internal TipEventArgs(Tip tip)
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
}
