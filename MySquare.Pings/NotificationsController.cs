using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MySquare.Service;
using MySquare.FourSquare;
using System.Diagnostics;
using MySquare.Pings.Properties;

namespace MySquare.Pings
{
    class NotificationsController 
    {
        #region Generic Initialization
        AutoResetEvent waitThread = new AutoResetEvent(false);
        MySquare.Service.FourSquare Service;
            
        public NotificationsController()
        {
            Service = new MySquare.Service.FourSquare();
            Service.CheckInsResult += new MySquare.FourSquare.CheckInsEventHandler(Service_CheckInsResult);
            Service.Error += new ErrorEventHandler(Service_Error);
        }
        #endregion





        bool error;
        public bool GetCheckIns()
        {
            checkIns = null;
            Service.GetFriendsCheckins(null, null);
            waitThread.WaitOne();
            if (!error && checkIns != null)
            {
                List<CheckIn> checkInsToAlert = new List<CheckIn>();
                for (int i = 0; i < checkIns.Length; i++)
                {
                    if ((DateTime.Now - checkIns[i].Created).TotalHours < 10 &&
                        (checkIns[i].Shout != null || checkIns[i].Venue != null) &&
                        checkIns[i].Ping && checkIns[i].Created > Configuration.LastCheckIn)
                    {
                        checkInsToAlert.Add(checkIns[i]);
                    }
                }
                StringBuilder message = new StringBuilder();
                if (checkInsToAlert.Count > 0)
                {
                    Configuration.LastCheckIn = checkInsToAlert[0].Created;
                    message.Append("<ul style=\"padding: 0 0 0 10px; margin: 0 0 0 5px;list-style-type: square;\">");
                    for (int i = 0; i < checkInsToAlert.Count && i < 4; i++)
                    {
                        var chkin = checkInsToAlert[i];

                        if (i < 3 || checkInsToAlert.Count == 4)
                        {
                            message.Append("<li style=\"padding:0;margin-bottom:4px;\">");
                            message.Append(chkin.Display);
                            message.Append(", ");
                            message.Append(chkin.Created.ToHumanTime());
                            message.Append("</li>");
                        }

                        if (i == 2 && checkInsToAlert.Count > 4)
                        {
                            message.Append("<li style=\"padding:0;margin-bottom:4px;\">");
                            message.Append(string.Format("More {0} friend(s) have checked-in.", checkInsToAlert.Count - 3));
                            message.Append("</li>");
                        }
                    }

                    message.Append("</ul>");

                }
                if (message.Length > 0)
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
                            notConfig.Duration = 1;
                        }
                    }

                    Tenor.Mobile.UI.NotificationWithSoftKeys.Show(guid,
                        "MySquare", message.ToString(), false, Resources.mySquare);
                }
                message = null;
                return true;
            }
            else
                return false;
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

    }



}