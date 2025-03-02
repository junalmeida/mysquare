﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using MySquare.Service;
using System.Reflection;

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
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

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
            bool ret = false;
            try
            {
                if (!Configuration.RetrievePings || (Configuration.PingInterval <= 0 &&
                    Configuration.isPremium.HasValue))
                {
                    Quit();
                }
                else
                {
                    if (Configuration.IsPremium)
                        ret = controller.GetCheckIns();
                }
            }
            catch (ObjectDisposedException ex)
            {
                Log.RegisterLog(ex);
                Quit();
            }
            catch (Exception ex)
            {
                Log.RegisterLog(ex);
            }
            finally
            {
                if (pings != null)
                {
                    if (ret)
                        pings.Change(Configuration.PingInterval * (60 * 1000), Timeout.Infinite);
                    else
                        pings.Change((30 * 1000), Timeout.Infinite);
                }
            }
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
            if (control != null)
            {
                control.Dispose();
                control = null;
            }
            if (pings != null)
            {
                pings.Dispose();
                pings = null;
            }
            if (pingsLoop != null)
            {
                pingsLoop.Set();
                pingsLoop = null;
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e != null && e.ExceptionObject is Exception && !(e.ExceptionObject is ThreadAbortException))
            {
                var ex = (Exception)e.ExceptionObject;
                Service.Log.RegisterLog(ex);
            }
        }
    }
}