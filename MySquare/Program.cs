using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using MySquare.FourSquare;

namespace MySquare
{
    static class Program
    {
        static Program()
        {
            Service = new Service();
        }
        internal static Service Service { get; private set; }

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