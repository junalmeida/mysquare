﻿//#define TEST

using System;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using MySquare.FourSquare;
using RisingMobility.Mobile.Location;
using System.Threading;
using MySquare.Controller;
using System.Drawing;
using MySquare.Service;
using Tenor.Mobile.Drawing;

[assembly: System.Reflection.Obfuscation(Feature = "Apply to MySquare.FourSquare.*: all", Exclude = true, ApplyToMembers = true)]
namespace MySquare
{
    public static class Program
    {
        internal static WorldPosition Location
        { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        public static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            using (UI.Main mainForm = new UI.Main())
            {
#if !DEBUG
                try
                {
#endif
                NotificationsController.Check();

                Location = new WorldPosition(true, Configuration.UseGps, 15000);
                Location.LocationChanged += new EventHandler(Location_LocationChanged);
                Location.PollHit += new EventHandler(Location_PollHit);
                Location.Poll();

                Application.Run(mainForm);
#if !DEBUG
                }
                catch (ObjectDisposedException)
                {
                }
                catch (Exception ex)
                {
                    Service.Log.RegisterLog(ex);
                    MessageBox.Show("Unknown error.\r\n" + ex.Message, "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    Terminate();
                }
                finally
                {
                    Location.Dispose();
                }
                try
                {
                    mainForm.Close();
                    mainForm.Dispose();
                }
                catch (ObjectDisposedException) { }
#endif
            }
            Application.Exit();
        }

        static void Location_LocationChanged(object sender, EventArgs e)
        {
            Log.RegisterLog("lbs-info", new Exception(
                string.Format(@"
bid: {0}
cid: {1}
mcc: {2}
mnc: {3}
lac: {4}
fix: {5}
location: {6}
service: {7}",
         Program.Location.BaseStationId,
         Program.Location.CellId,
         Program.Location.CountryCode,
         Program.Location.NetworkCode,
         Program.Location.AreaCode,
         Program.Location.FixType,
         Program.Location.WorldPoint,
         Program.Location.FixService)
                ));
        }

        static void Location_PollHit(object sender, EventArgs e)
        {
            if (Location != null)
            {
                if (Location.FixType == FixType.GsmNetwork)
                    Location.UseNetwork = true;
                if (KeepGpsOpened)
                    Location.UseGps = true;
                else
                    Location.UseGps = Configuration.UseGps;
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e != null && e.ExceptionObject is Exception)
                Service.Log.RegisterLog(e.ExceptionObject as Exception);
        }

        internal static bool KeepGpsOpened { get; set; }

        internal static void Terminate()
        {
            try
            {
                BaseController.Terminate();
            }
            catch { }
        }


        #region Pings 
        static System.Threading.Timer pings;
        static AutoResetEvent pingsLoop;
        public static void Pings(AutoResetEvent reset)
        {
            Location = new WorldPosition(true, Configuration.UseGps, 15000);
            Location.LocationChanged += new EventHandler(Location_LocationChanged);
            Location.PollHit += new EventHandler(Location_PollHit);
            Location.Poll();

            new NotificationsController();
            pingsLoop = reset;
            pings = new System.Threading.Timer(new TimerCallback(Pings_Tick), null, 1000 * 30, Timeout.Infinite);
            Cursor.Current = Cursors.Default;
        }

        static void Pings_Tick(object state)
        {
            try
            {

                if (Configuration.PingInterval <= 0 && Configuration.isPremium.HasValue)
                {
                    pings.Dispose();
                    pingsLoop.Set();
                }
                else
                {
                    pings.Change(Configuration.PingInterval * 60 * 1000, Timeout.Infinite);
                }
            }
            catch (ObjectDisposedException) { }
        }
        #endregion
    }
}

#if TESTING
namespace Microsoft.WindowsCE.Forms
{
    public class InputPanel : System.ComponentModel.Component
    {
        public InputPanel()
            : base()
        { }

        public InputPanel(System.ComponentModel.IContainer container)
            : this()
        { }

        public bool Enabled { get; set; }
        public System.Drawing.Rectangle Bounds
        {
            get
            {
                return System.Drawing.Rectangle.Empty;
            }
        }
        public event EventHandler EnabledChanged;
    }
}
#endif

