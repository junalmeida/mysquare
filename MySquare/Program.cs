//#define TEST

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
#if DEBUG
           // if (System.Diagnostics.Debugger.IsAttached)
                //Application.Run(new MySquare.UI.MapTest());
#endif
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            using (UI.Main mainForm = new UI.Main())
            {
#if !DEBUG
                try
                {
#endif
                Configuration.CheckNotifications();
                Network.CheckCacheFiles();

                Location = new WorldPosition(true, Configuration.UseGps, 15000);
                Location.LocationChanged += new EventHandler(Location_LocationChanged);
                Location.PollHit += new EventHandler(Location_PollHit);
                Location.Poll();
                
                Application.Run(mainForm);
                foreach (var obj in BaseController.Controllers)
                {
                    try
                    {
                        obj.Service.Abort();
                    }
                    catch (Exception)
                    { }
                }
                Configuration.abortCheck = true;
                if (Configuration.PingInterval > 0)
                {
                    Configuration.RetrievePings = (MessageBox.Show("Keep recieveing check-in notifications after closing MySquare?", "MySquare", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes);
                }
                if (lastException != null)
                {
                    MessageBox.Show("Unknkown error: " + lastException.Message + "\r\n");
                }
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
                    DisposeThings();
                }
                try
                {
                    mainForm.Close();
                    mainForm.Dispose();
                }
                catch (ObjectDisposedException) { }
#else
                DisposeThings();
#endif
            }
            Application.Exit();
        }

        private static void DisposeThings()
        {
            if (Location != null)
            {
                Location.Dispose();
                Location = null;
            }
            if (timerGpsOff != null)
            {
                timerGpsOff.Dispose();
                timerGpsOff = null;
            }
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
service: {7},
sattelites: {8}",
         Program.Location.BaseStationId,
         Program.Location.CellId,
         Program.Location.CountryCode,
         Program.Location.NetworkCode,
         Program.Location.AreaCode,
         Program.Location.FixType,
         Program.Location.WorldPoint,
         Program.Location.FixService,
         Program.Location.Sattelites.ToString())));
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


        internal static void ResetGps()
        {
            if (Program.Location != null)
            {
                //reset gps
                Program.Location.UseGps = Configuration.UseGps;
                Program.Location.Poll();
                if (timerGpsOff != null)
                {
                    timerGpsOff.Dispose();
                    timerGpsOff = null;
                }
            }
        }
        internal static void TurnGpsOff()
        {
            if (Program.Location != null)
            {
                if (timerGpsOff != null)
                    timerGpsOff.Dispose();
                //turn polling off after five minutes on background
                timerGpsOff = new System.Threading.Timer(new System.Threading.TimerCallback(TimerGpsOff_Tick), null,
                    5 * (60 * 1000) 
                    , System.Threading.Timeout.Infinite);

            }
        }
        static void TimerGpsOff_Tick(object state)
        {
            if (Program.Location != null)
            {
                Program.Location.Stop();
                Program.Location.UseGps = false;
            }
        }

        static System.Threading.Timer timerGpsOff = null;

        static Exception lastException;
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e != null && e.ExceptionObject is Exception)
            {
                lastException = (Exception)e.ExceptionObject;
                Service.Log.RegisterLog(e.ExceptionObject as Exception);
            }
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

