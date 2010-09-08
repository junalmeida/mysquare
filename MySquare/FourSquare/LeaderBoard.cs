using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MySquare.FourSquare
{
    class LeaderboardUser
    {
        public string User { get; set; }
        public int Percentage { get; set; }
        public string Points { get; set; }
        public bool Self { get; set; }
    }


    delegate void LeaderboardEventHandler(object serder, LeaderboardEventArgs e);
    class LeaderboardEventArgs : EventArgs
    {
        public LeaderboardEventArgs() { }

        public LeaderboardUser[] Users { get; set; }
        public string RefreshTime { get; set; }
        public string AllText { get; set; }
    }


    enum View
    {
        Friends, 
        All
    }
}
