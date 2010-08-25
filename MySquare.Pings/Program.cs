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
        }

        static System.Threading.Timer control;
        static System.Threading.Timer pings;
        static AutoResetEvent pingsLoop;
        public static void Pings()
        {
            controller = new NotificationsController();

            pings = new System.Threading.Timer(new TimerCallback(Pings_Tick), null, 1000 * 30, Timeout.Infinite);
            control = new System.Threading.Timer(new TimerCallback(Control_Tick), null, 1000 * 30, 1000 * 60);
            Cursor.Current = Cursors.Default;
        }

        static void Pings_Tick(object state)
        {
            try
            {
                if (!Configuration.RetrievePings || (Configuration.PingInterval <= 0 &&
                    Configuration.isPremium.HasValue))
                {
                    Quit();
                }
                else
                {
                    bool ret = false;
                    if (Configuration.IsPremium)
                        ret = controller.GetCheckIns();
                    if (ret)
                        pings.Change(Configuration.PingInterval * (60 * 1000), Timeout.Infinite);
                    else
                        pings.Change((30 * 1000), Timeout.Infinite);
                }
            }
            catch (ObjectDisposedException) { pingsLoop.Set(); }
        }

        static void Control_Tick(object state)
        {
            try
            {
                if (!Configuration.RetrievePings || (Configuration.PingInterval <= 0 &&
                    Configuration.isPremium.HasValue))
                {
                    Quit();
                }
            }
            catch (ObjectDisposedException) { }
        }

        private static void Quit()
        {
            control.Dispose();
            pings.Dispose();
            pingsLoop.Set();
        }

    }
}