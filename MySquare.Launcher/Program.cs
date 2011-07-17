using System;

namespace MySquare.Launcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main(string[] args)
        {
            //following the advices on this article:
            //http://robtiffany.com/mobile-development/memmaker-for-the-net-compact-framework
            global::MySquare.Program.Main(args);
        }
    }
}