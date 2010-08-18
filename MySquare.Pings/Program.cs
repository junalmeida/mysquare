using System;
using System.Collections.Generic;
using System.Threading;

namespace MySquare.Pings
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            AutoResetEvent reset = new AutoResetEvent(false);
            MySquare.Program.Pings(reset);
            reset.WaitOne();
        }
    }
}