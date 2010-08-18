using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MySquare.Service;
using MySquare.FourSquare;
using MySquare.Properties;
using System.Diagnostics;

namespace MySquare.Controller
{
    class NotificationsController : BaseController
    {
        #region Generic Initialization
        public NotificationsController()
        {
            Controllers.Add(this);
            Initialize();
            Service.CheckInsResult += new MySquare.FourSquare.CheckInsEventHandler(Service_CheckInsResult);
            Service.Error += new ErrorEventHandler(Service_Error);
        }


        private AutoResetEvent waitThread = new AutoResetEvent(false);
        public override AutoResetEvent WaitThread { get { return waitThread; } }
        #endregion

        Timer timer;
        private void Initialize()
        {
            timer = new Timer(new TimerCallback(Ping), null, 1000 * 60, Timeout.Infinite);
        }


        private void Ping(object state)
        {
            try
            {
                int interval = Configuration.PingInterval;
                if (interval <= 0)
                {
                    timer.Change(1000 * 60, Timeout.Infinite);
                }
                else
                {
                    GetCheckIns();
                    timer.Change(interval * 1000 * 60, Timeout.Infinite);
                }
            }
            catch (ObjectDisposedException) { }
        }

        bool error;
        private void GetCheckIns()
        {
            Program.Location.Stop();
            Program.Location.PollHit += new EventHandler(Location_PollHit);
            Program.Location.Error += new RisingMobility.Mobile.Location.ErrorEventHandler(Location_Error);
            Program.Location.UseNetwork = true;
            Program.Location.UseGps = Configuration.UseGps;
            error = false;
            Program.Location.Poll();
            waitThread.WaitOne();
            if (!error)
            {
                Service.GetFriendsCheckins(Program.Location.WorldPoint.Latitude, Program.Location.WorldPoint.Longitude);
                waitThread.WaitOne();
                if (!error)
                {
                    List<CheckIn> checkInsToAlert = new List<CheckIn>();
                    for (int i = 0; i < checkIns.Length; i++)
                    {
                        if ((DateTime.Now - checkIns[i].Created).TotalHours < 24 &&
                            (checkIns[i].Shout != null || checkIns[i].Venue != null))
                        {
                            if (checkIns[i].Id == Configuration.LastCheckIn)
                                break;
                            checkInsToAlert.Add(checkIns[i]);
                        }
                    }
                    string message = null;
                    if (checkInsToAlert.Count > 0)
                    {
                        Configuration.LastCheckIn = checkInsToAlert[0].Id;
                        if (checkInsToAlert.Count == 1)
                        {
                            message = checkInsToAlert[0].Display + ", " + checkInsToAlert[0].Created.ToHumanTime();
                        }
                        else
                        {
                            message = string.Format("{0} friends have checked-in.", checkInsToAlert.Count);
                        }
                    }
                    if (!string.IsNullOrEmpty(message))
                    {
                        Guid guid = Configuration.GetAppGuid();
                        if (!Tenor.Mobile.Device.Notification.Exists(guid))
                        {
                            var notConfig = Tenor.Mobile.Device.Notification.Create(guid);
                            notConfig.Text = "MySquare: Recent check-in";
                            string file = "\\Windows\\Alarm1.wma";
                            if (System.IO.File.Exists(file))
                            {
                                notConfig.Options = Tenor.Mobile.Device.NotificationOptions.DisplayBubble | Tenor.Mobile.Device.NotificationOptions.Sound;
                                notConfig.Wave = file;
                            }
                        }
                        else
                        {
                            var notConfig = new Tenor.Mobile.Device.Notification(guid);
                        }

                        Tenor.Mobile.UI.NotificationWithSoftKeys.Show(guid,
                            "MySquare", message, false, Resources.mySquare);
                    }
   
                }
            }
        }

        void Location_Error(object sender, RisingMobility.Mobile.Location.ErrorEventArgs e)
        {
            error = true;
            Log.RegisterLog("lbs", e.Error);
            waitThread.Set();
        }

        void Service_Error(object serder, ErrorEventArgs e)
        {
            error = true;
            Log.RegisterLog("notification", e.Exception);
            waitThread.Set();
        }


        void Location_PollHit(object sender, EventArgs e)
        {
            waitThread.Set();
        }


        CheckIn[] checkIns;
        void Service_CheckInsResult(object serder, MySquare.FourSquare.CheckInsEventArgs e)
        {
            checkIns = e.CheckIns;
            waitThread.Set();
        }

        internal static void Check()
        {
            Thread t = new Thread(new ThreadStart(delegate()
            {
                try
                {
                    bool doOpen = Configuration.PingInterval > 0;
                    if (doOpen)
                    {
                        Tenor.Mobile.Diagnostics.Process process = null;
                        foreach (var p in Tenor.Mobile.Diagnostics.Process.GetProcesses())
                        {
                            if (System.IO.Path.GetFileNameWithoutExtension(p.FileName) == "MySquare.Pings")
                            {
                                process = p;
                                break;
                            }
                        }
                        if (process == null)
                        {
                            string path = Configuration.GetAppPath();
                            Process.Start(System.IO.Path.Combine(path, "MySquare.Pings.exe"), string.Empty);
                        }
                    }
                }
                catch { }
            }));

            t.Start();
        }
    }



}