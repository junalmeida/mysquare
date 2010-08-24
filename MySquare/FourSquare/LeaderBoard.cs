using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MySquare.FourSquare
{
    class LeaderBoard
    {
        public string User { get; set; }
        public int Percentage { get; set; }
        public string Points { get; set; }
    }

    enum View
    {
        Friends, 
        All
    }
}
