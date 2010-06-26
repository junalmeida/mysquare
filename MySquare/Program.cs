using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using MySquare.FourSquare;
using Tenor.Mobile.Location;

namespace MySquare
{
    static class Program
    {
        static Program()
        {
            Service = new Service();
            //Position = new WorldPosition(false, false);
        }
        internal static Service Service { get; private set; }
        internal static WorldPosition Position { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Application.Run(new UI.Main());
        }
    }
}