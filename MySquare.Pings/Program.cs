using System;
using System.Collections.Generic;
using System.Threading;
using RisingMobility.Mobile.Location;
using System.Windows.Forms;
using MySquare.Service;

namespace MySquare.Pings
{
    static class Program
    {
        static NotificationsController controller;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            pingsLoop = new AutoResetEvent(false);
            Pings();
            pingsLoop.WaitOne();
            Location.Dispose();
        }

        static System.Threading.Timer pings;
        static AutoResetEvent pingsLoop;
        internal static WorldPosition Location;
        public static void Pings()
        {
            controller = new NotificationsController();
            Location = new WorldPosition(true, Configuration.UseGps, 15000);
            Location.Poll();

            pings = new System.Threading.Timer(new TimerCallback(Pings_Tick), null, 1000 * 30, Timeout.Infinite);
            Cursor.Current = Cursors.Default;
        }

        static void Pings_Tick(object state)
        {
            try
            {
                if ((!Configuration.RetrievePings || Configuration.PingInterval <= 0) && Configuration.isPremium.HasValue)
                {
                    pings.Dispose();
                    pingsLoop.Set();
                }
                else
                {
                    if (Configuration.IsPremium)
                        controller.GetCheckIns();
                    pings.Change(Configuration.PingInterval * 60 * 1000, Timeout.Infinite);
                }
            }
            catch (ObjectDisposedException) { pingsLoop.Set(); }
        }

    }
}