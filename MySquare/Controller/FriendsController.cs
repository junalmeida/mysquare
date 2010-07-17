using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Tenor.Mobile.Location;
using MySquare.FourSquare;
using System.Threading;
using System.Drawing;
using System.IO;
using MySquare.UI.Friends;
using MySquare.Service;

namespace MySquare.Controller
{
    class FriendsController : BaseController<Friends>
    {
        public FriendsController(Friends view)
            : base(view)
        {
            Service.CheckInsResult += new MySquare.FourSquare.CheckInsEventHandler(Service_CheckInsResult);
            Service.FriendsResult += new FriendsEventHandler(Service_FriendsResult);
            Service.Error += new MySquare.Service.ErrorEventHandler(MainController.Service_Error);
        }

        public override void Activate()
        {
            UI.Main form = View.Parent as UI.Main;
            form.ChangeFriendsName(null);

            View.BringToFront();
            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.Visible = true;

            LeftSoftButtonEnabled = true;
            LeftSoftButtonText = "&Refresh";
            RightSoftButtonEnabled = false;
            RightSoftButtonText = string.Empty;

            View.listBox.Visible = true;
            form.header.Tabs[1].Selected = true;
            if (View.listBox.Count == 0)
                LoadFriends();
        }

        public override void Deactivate()
        {
            View.Visible = false;
        }

        public override void OnLeftSoftButtonClick()
        {
            LoadFriends();
        }

        WorldPosition position;
        private void LoadFriends()
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            View.listBox.Visible = false;
            View.ImageList = new Dictionary<string, System.Drawing.Image>();
            View.listBox.Clear();
#if DEBUG
            if (Environment.OSVersion.Platform != PlatformID.WinCE || Tenor.Mobile.Device.Device.OemInfo.IndexOf("Emulator") > -1)
            {
                lastLatitude = 40.769362;
                lastLongitude = -73.971033;
                Service.GetFriendsCheckins(lastLatitude.Value, lastLongitude.Value);
                return;
            }
#endif
            position = new WorldPosition(false, false);
            position.LocationChanged += new EventHandler(position_LocationChanged);
            position.Error += new Tenor.Mobile.Location.ErrorEventHandler(position_Error);
            position.PollCell();
        }

        void position_Error(object sender, Tenor.Mobile.Location.ErrorEventArgs e)
        {
            ShowError("Could not get your location, try again later.");
            Log.RegisterLog(e.Error);
            position = null;
        }

        void position_LocationChanged(object sender, EventArgs e)
        {
            if (position.Latitude.HasValue && position.Longitude.HasValue)
            {
                lastLatitude = position.Latitude;
                lastLongitude = position.Longitude;
                Service.GetFriendsCheckins(position.Latitude.Value, position.Longitude.Value);
            }
            else
            {
                ShowError("Could not get your location, try again later.");
                Log.RegisterLog(new Exception("Unknown error from location service."));
            }
            position = null;
        }

        void Service_CheckInsResult(object serder, MySquare.FourSquare.CheckInsEventArgs e)
        {
            View.Invoke(new ThreadStart(delegate()
                {
                    LoadCheckIns(e.CheckIns);
                }));
        }

        CheckIn[] checkIns;
        void LoadCheckIns(CheckIn[] checkIns)
        {
            this.checkIns = checkIns;
            View.listBox.AddItem("Check Ins", null, View.listBox.DefaultItemHeight / 2);
            foreach (CheckIn checkin in checkIns)
            {
                View.listBox.AddItem(null, checkin);
            }
            View.listBox.Visible = true;
            Cursor.Current = Cursors.Default;
            Cursor.Show();


            Thread t = new Thread(new ThreadStart(delegate()
            {
                foreach (CheckIn chk in checkIns)
                {
                    if (chk.User != null && !string.IsNullOrEmpty(chk.User.ImageUrl))
                    {
                        try
                        {

                            using (MemoryStream mem = new MemoryStream(Service.DownloadImageSync(chk.User.ImageUrl)))
                            {
                                Bitmap bmp = new Bitmap(mem);
                                if (!View.ImageList.ContainsKey(chk.User.ImageUrl))
                                {
                                    View.ImageList.Add(chk.User.ImageUrl, bmp);
                                    View.Invoke(new ThreadStart(delegate() { View.listBox.Invalidate(); }));
                                }
                            }
                        }
                        catch { }
                    }
                }
            }));
            t.Start();

            Service.GetFriends(0);
        }


        void Service_FriendsResult(object sender, FriendsEventArgs e)
        {
            this.View.Invoke(new ThreadStart(delegate()
            {
                LoadFriends(e.Friends);
            }));
        }

        private void LoadFriends(User[] user)
        {
            List<User> users = new List<User>();

            foreach (User u in user)
            {
                bool found = false;
                foreach (CheckIn chk in checkIns)
                {
                    if (chk.User != null && chk.User.Id == u.Id)
                    { found = true; break; }

                }
                if (!found) users.Add(u);
            }

            if (users.Count > 0)
            {
                View.listBox.AddItem("Other friends", null, View.listBox.DefaultItemHeight / 2);
                foreach (User u in users)
                    View.listBox.AddItem(null, u);
            }

            Thread t = new Thread(new ThreadStart(delegate()
            {
                foreach (User u in users)
                {
                    if (!string.IsNullOrEmpty(u.ImageUrl))
                    {
                        try
                        {

                            using (MemoryStream mem = new MemoryStream(Service.DownloadImageSync(u.ImageUrl)))
                            {
                                Bitmap bmp = new Bitmap(mem);
                                if (!View.ImageList.ContainsKey(u.ImageUrl))
                                {
                                    View.ImageList.Add(u.ImageUrl, bmp);
                                    View.Invoke(new ThreadStart(delegate() { View.listBox.Invalidate(); }));
                                }
                            }
                        }
                        catch { }
                    }
                }
            }));
            t.Start();
        }


    }
}
