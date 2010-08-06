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

        private void LoadFriends()
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            View.listBox.Visible = false;
            View.ImageList = new Dictionary<string, byte[]>();
            View.listBox.Clear();

            if (Program.Location.FixType == FixType.Gps)
            {
                position_LocationChanged(null, null);
            }
            else
            {
                Program.Location.PollHit += new EventHandler(position_LocationChanged);
                Program.Location.Error += new Tenor.Mobile.Location.ErrorEventHandler(position_Error);
                Program.Location.UseNetwork = true;
                Program.Location.Poll();
            }
        }

        void position_Error(object sender, Tenor.Mobile.Location.ErrorEventArgs e)
        {
            Program.Location.PollHit -= new EventHandler(position_LocationChanged);
            Program.Location.Error -= new Tenor.Mobile.Location.ErrorEventHandler(position_Error);
            ShowError("Could not get your location, try again later.");
            Log.RegisterLog(e.Error);
 
        }

        void position_LocationChanged(object sender, EventArgs e)
        {
            Program.Location.PollHit -= new EventHandler(position_LocationChanged);
            Program.Location.Error -= new Tenor.Mobile.Location.ErrorEventHandler(position_Error);
            if (!Program.Location.WorldPoint.IsEmpty)
            {
                Service.GetFriendsCheckins(Program.Location.WorldPoint.Latitude, Program.Location.WorldPoint.Longitude);
            }
            else
            {
                ShowError("Could not get your location, try again later.");
                Log.RegisterLog(new Exception("Unknown error from location service."));
            }
          
        }

        void Service_CheckInsResult(object serder, MySquare.FourSquare.CheckInsEventArgs e)
        {
            View.Invoke(new ThreadStart(delegate()
                {
                    LoadCheckIns(e.CheckIns);
                }));
        }

        CheckIn[] checkIns;
        User[] friends;
        User[] pendingFriends;
        bool alreadyDone;
        void LoadCheckIns(CheckIn[] checkIns)
        {
            alreadyDone = false;
            friends = null;
            pendingFriends = null;
            this.checkIns = checkIns;


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
                            catch { }
                        }
                    }
                }
            }));
            t.Start();

            Service.GetPendingFriends();
            Service.GetFriends(0);
        }


        void Service_FriendsResult(object sender, FriendsEventArgs e)
        {
            this.View.Invoke(new ThreadStart(delegate()
            {
                friends = e.Friends;
                LoadFriends(e.Friends);
            }));
        }

        void Service_PendingFriendsResult(object serder, PendingFriendsEventArgs e)
        {
            this.View.Invoke(new ThreadStart(delegate()
            {
                pendingFriends = e.Friends;
                LoadFriends(e.Friends);
            }));
        }

        private void LoadFriends(User[] user)
        {
            List<User> otherUsers = new List<User>();
            List<User> pendingUsers = new List<User>();

            foreach (User u in user)
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

            if (pendingUsers.Count > 0)
            {
                View.listBox.AddItem("Pending requests", null, View.listBox.DefaultItemHeight / 2);
                foreach (User u in pendingUsers)
                    View.listBox.AddItem(null, u);
            }

            if (otherUsers.Count > 0)
            {
                View.listBox.AddItem("Other friends", null, View.listBox.DefaultItemHeight / 2);
                foreach (User u in otherUsers)
                    View.listBox.AddItem(null, u);
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
                        catch { }
                    }
                }
            }));
            t.Start();




            if (pendingFriends != null && !alreadyDone)
            {
                alreadyDone = true;

                View.listBox.AddItem("Check Ins", null, View.listBox.DefaultItemHeight / 2);
                foreach (CheckIn checkin in checkIns)
                {
                    View.listBox.AddItem(null, checkin);
                }
                View.listBox.Visible = true;
                Cursor.Current = Cursors.Default;
                Cursor.Show();
            }
        }
    }
}
