using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using RisingMobility.Mobile.Location;
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
            Service.PendingFriendsResult += new PendingFriendsEventHandler(Service_PendingFriendsResult);
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
            form.header.Tabs[2].Selected = true;
            if (View.listBox.Count == 0)
                GetFriends();
        }

        public override void Deactivate()
        {
            View.Visible = false;
        }

        public override void OnLeftSoftButtonClick()
        {
            GetFriends();
        }

        private void GetFriends()
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            View.listBox.Visible = false;
            View.ImageList = new Dictionary<string, byte[]>();
            View.listBox.Clear();


            Program.Location.Stop();
            Program.Location.PollHit += new EventHandler(position_LocationChanged);
            Program.Location.Error += new RisingMobility.Mobile.Location.ErrorEventHandler(position_Error);
            Program.Location.UseNetwork = true;
            Program.Location.UseGps = Configuration.UseGps;
            Program.Location.Poll();
        }

        void position_Error(object sender, RisingMobility.Mobile.Location.ErrorEventArgs e)
        {
            Program.Location.PollHit -= new EventHandler(position_LocationChanged);
            Program.Location.Error -= new RisingMobility.Mobile.Location.ErrorEventHandler(position_Error);
            ShowError("Could not get your location, try again later.");
            Log.RegisterLog("lbs", e.Error);
 
        }

        void position_LocationChanged(object sender, EventArgs e)
        {
            Program.Location.PollHit -= new EventHandler(position_LocationChanged);
            Program.Location.Error -= new RisingMobility.Mobile.Location.ErrorEventHandler(position_Error);
            if (!Program.Location.WorldPoint.IsEmpty)
            {
                friends = null;
                pendingFriends = null;
                checkIns = null;
                Service.GetFriendsCheckins(Program.Location.WorldPoint.Latitude, Program.Location.WorldPoint.Longitude);
            }
            else
            {
                ShowError("Could not get your location, try again later.");
                Log.RegisterLog("lbs", new Exception("Unknown error from location service."));
            }
          
        }

        void Service_CheckInsResult(object serder, MySquare.FourSquare.CheckInsEventArgs e)
        {
            try
            {
                View.Invoke(new ThreadStart(delegate()
                    {
                        checkIns = e.CheckIns;
                        LoadCheckIns();
                    }));
            }
            catch (ObjectDisposedException) { }
        }

        CheckIn[] checkIns;
        User[] friends;
        User[] pendingFriends;
        void LoadCheckIns()
        {
            friends = null;
            pendingFriends = null;

            ////don't know if I should do this
            //if (checkIns.Length > 0)
            //    Configuration.LastCheckIn = checkIns[0].Id;

            Thread t = new Thread(new ThreadStart(delegate()
            {
                foreach (CheckIn chk in checkIns)
                {
                    if (chk.User != null && !string.IsNullOrEmpty(chk.User.ImageUrl))
                    {
                        if (!View.ImageList.ContainsKey(chk.User.ImageUrl))
                        {
                            try
                            {
                                byte[] buffer = Service.DownloadImageSync(chk.User.ImageUrl);
                                View.ImageList.Add(chk.User.ImageUrl, buffer);
                                View.Invoke(new ThreadStart(delegate() { View.listBox.Invalidate(); }));
                            }
                            catch (ObjectDisposedException)
                            {
                                break;
                            }
                            catch (Exception) { }
                        }
                    }
                }
            }));
            t.StartThread();

            Service.GetPendingFriends();
            Service.GetFriends(null);
        }


        void Service_FriendsResult(object sender, FriendsEventArgs e)
        {
            try
            {
                this.View.Invoke(new ThreadStart(delegate()
                          {
                              friends = e.Friends;
                              LoadFriends();
                          }));
            }
            catch (ObjectDisposedException) { }
        }

        void Service_PendingFriendsResult(object serder, PendingFriendsEventArgs e)
        {
            try
            {
                this.View.Invoke(new ThreadStart(delegate()
                {
                    pendingFriends = e.Friends;
                    LoadFriends();
                }));
            }
            catch (ObjectDisposedException) { }
        }

        private void LoadFriends()
        {
            if (pendingFriends == null || checkIns == null || friends == null)
                return;

            List<User> list = new List<User>();
            list.AddRange(pendingFriends);
            list.AddRange(friends);
            List<User> otherUsers = new List<User>();
            List<User> pendingUsers = new List<User>();

            foreach (User u in list)
            {
                bool found = false;
                foreach (CheckIn chk in checkIns)
                {
                    if (chk.User != null && chk.User.Id == u.Id)
                    { found = true; break; }

                }
                if (!found)
                {
                    if (u.FriendStatus == FriendStatus.pendingyou)
                        pendingUsers.Add(u);
                    else
                        otherUsers.Add(u);
                }
            }



            Thread t = new Thread(new ThreadStart(delegate()
            {
                List<User> all = new List<User>(otherUsers);
                all.AddRange(pendingUsers);
                foreach (User u in all)
                {
                    if (!string.IsNullOrEmpty(u.ImageUrl) && !View.ImageList.ContainsKey(u.ImageUrl))
                    {
                        try
                        {
                            byte[] buffer = Service.DownloadImageSync(u.ImageUrl);
                            View.ImageList.Add(u.ImageUrl, buffer);
                            View.Invoke(new ThreadStart(delegate() { View.listBox.Invalidate(); }));
                        }
                        catch (ObjectDisposedException)
                        {
                            break;
                        }
                        catch { }
                    }
                }
            }));
            t.StartThread();

            if (pendingUsers.Count > 0)
            {
                View.listBox.AddItem("Pending requests", null, View.listBox.DefaultItemHeight / 2);
                foreach (User u in pendingUsers)
                    View.listBox.AddItem(null, u);
            }

            View.listBox.AddItem("Check Ins", null, View.listBox.DefaultItemHeight / 2);
            foreach (CheckIn checkin in checkIns)
            {
                View.listBox.AddItem(null, checkin);
            }


            if (otherUsers.Count > 0)
            {
                View.listBox.AddItem("Other friends", null, View.listBox.DefaultItemHeight / 2);
                foreach (User u in otherUsers)
                    View.listBox.AddItem(null, u);
            }


            View.listBox.Visible = true;
            Cursor.Current = Cursors.Default;
            Cursor.Show();

        }
    }
}
