using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.UI.Friends;
using MySquare.UI;
using MySquare.FourSquare;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using MySquare.Service;
using System.Diagnostics;

namespace MySquare.Controller
{
    class UserController : BaseController<UserDetail>
    {

        public UserController(UserDetail view)
            : base(view)
        {
            Service.UserResult += new UserEventHandler(Service_UserResult);
            Service.FriendsResult += new FriendsEventHandler(Service_FriendsResult);
            Service.Error += new MySquare.Service.ErrorEventHandler(Service_Error);
            View.picAvatar.Click += new EventHandler(picAvatar_Click);
        }


        public override void Activate()
        {
            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.BringToFront();
            View.Visible = true;

            Main main = (View.Parent as Main);
            View.SelectSection(0);

            RightSoftButtonText = "&Back";
            RightSoftButtonEnabled = true;

            LeftSoftButtonText = string.Empty;
            LeftSoftButtonEnabled = false;
            main.header.Tabs[2].Selected = true;
        }

        public override void OnLeftSoftButtonClick()
        {
            base.OnLeftSoftButtonClick();
        }

        public override void OnRightSoftButtonClick()
        {
            if (!base.GoBack())
                OpenController((View.Parent as Main).friends1);
        }

        public override void Deactivate()
        {
            View.Avatar = null;
            View.Visible = false;
        }

        void Service_UserResult(object serder, MySquare.FourSquare.UserEventArgs e)
        {
            this.user = e.User;
            LoadUser(this.user);
        }


        void Service_Error(object serder, MySquare.Service.ErrorEventArgs e)
        {
            try
            {
                if (e.Exception is UnauthorizedAccessException)
                    ShowError(e.Exception);
                else
                    View.Invoke(new ThreadStart(delegate()
                    {
                        View.lblFriendStatus.Text = "unable to read data.";
                    }));
            }
            catch (ObjectDisposedException) { }
        }


        User user;
        internal void LoadUser(MySquare.FourSquare.User user)
        {
            try
            {

                if (View.InvokeRequired)
                {
                    View.Invoke(new ThreadStart(delegate()
                    {
                        LoadUser(user);
                    }));
                    return;
                }
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            this.user = user;

            base.SaveNavigation(user);
            if (!user.fullData)
            {
                View.lblFriendStatus.Text = "reading user data...";
                Service.GetUser(user.Id);
            }

            
            View.lblUserName.Text = string.Format("{0} {1}", user.FirstName, user.LastName);
            if (!string.IsNullOrEmpty(user.ImageUrl))
            {
                View.Avatar = Service.DownloadImageSync(user.ImageUrl);
            }
            else
            {
                View.Avatar = null;
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                View.userInfo1.lblEmail.Text = user.Email;
                View.userInfo1.lblEmail.Enabled = true;
            }
            else
            {
                View.userInfo1.lblEmail.Text = "Not available";
                View.userInfo1.lblEmail.Enabled = false;
            }


            {
                if (!string.IsNullOrEmpty(user.Twitter))
                    View.userInfo1.lnkFoursquare.Tag = user.Twitter;
                else
                    View.userInfo1.lnkFoursquare.Tag = "user/" + user.Id.ToString();

                View.userInfo1.lnkFoursquare.Text = user.FirstName + "'s web profile";
                View.userInfo1.lnkFoursquare.Enabled = true;
            }



            if (!string.IsNullOrEmpty(user.Facebook))
            {
                View.userInfo1.lblFacebook.Tag = user.Facebook;
                View.userInfo1.lblFacebook.Text = user.FirstName + "'s profile";
                View.userInfo1.lblFacebook.Enabled = true;
            }
            else
            {
                View.userInfo1.lblFacebook.Text = "Not available";
                View.userInfo1.lblFacebook.Enabled = false;
            }

            if (!string.IsNullOrEmpty(user.Twitter))
            {
                View.userInfo1.lblTwitter.Text = user.Twitter;
                View.userInfo1.lblTwitter.Enabled = true;
            }
            else
            {
                View.userInfo1.lblTwitter.Text = "Not available";
                View.userInfo1.lblTwitter.Enabled = false;
            }

            if (user.CheckIn == null)
            {
                View.userInfo1.lblShout.Text = null;
                View.userInfo1.lblLastSeen.Text = null;
            }
            else
            {
                View.userInfo1.lblShout.Text = user.CheckIn.Shout;
                if (user.CheckIn.Venue == null)
                {
                    View.userInfo1.lblLastSeen.Text = null;
                    View.userInfo1.lblLastSeen.Tag = null;
                }
                else
                {
                    View.userInfo1.lblLastSeen.Text = user.CheckIn.Venue.ToString();
                    View.userInfo1.lblLastSeen.Tag = user.CheckIn.Venue;
                }
            }


            LeftSoftButtonEnabled = false;
            LeftSoftButtonText = string.Empty;
            if (user.fullData)
            {
                if (user.FriendStatus.HasValue)
                    switch (user.FriendStatus.Value)
                    {
                        case FriendStatus.friend:
                            View.lblFriendStatus.Text = "is your friend";
                            break;
                        case FriendStatus.pendingyou:
                            {
                                View.lblFriendStatus.Text = "is waiting you to accept";
                                LeftSoftButtonEnabled = true;
                                LeftSoftButtonText = "&Actions";
                                MenuItem item = new MenuItem()
                                {
                                    Text = "&Accept"
                                };
                                item.Click += new EventHandler(AcceptUser_Click);
                                AddLeftSubMenu(item);
                                item = new MenuItem()
                                {
                                    Text = "&Reject"
                                };
                                item.Click += new EventHandler(RejectUser_Click);
                                AddLeftSubMenu(item);
                            }
                            break;
                        case FriendStatus.pendingthem:
                            View.lblFriendStatus.Text = "have not answered yet";
                            break;
                        default:
                            View.lblFriendStatus.Text = string.Empty;
                            break;
                    }
                else
                {
                    View.lblFriendStatus.Text = string.Empty;
                    LeftSoftButtonEnabled = true;
                    LeftSoftButtonText = "&Actions";

                    MenuItem item = new MenuItem()
                    {
                        Text = "&Add as Friend"
                    };
                    item.Click += new EventHandler(AddUser_Click);
                    AddLeftSubMenu(item);
                }
            }


            if (user.fullData && user.Friends == null)
                Service.GetFriends(user.Id);
            else
                LoadFriends(user.Friends);

            LoadBadges(user.Badges);
        }

        void picAvatar_Click(object sender, EventArgs e)
        {
            if (user != null && user.ImageUrl != null && user.ImageUrl.IndexOf("blank_") == -1)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    string path = user.ImageUrl.Replace("_thumbs", string.Empty);
                    byte[] file = Service.DownloadImageSync(path);
                    path = Network.GetCachePath(path);
                    file = null;


                    using (var key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("jpegimage\\Shell\\Open\\Command"))
                    {
                        string viewer = key.GetValue(string.Empty) as string;

                        viewer = viewer.Replace("%1", "").Trim();
                        viewer = viewer.Replace("\"", "").Trim();

                        ProcessStartInfo psi =
                           new ProcessStartInfo();

                        psi.UseShellExecute = true;
                        psi.FileName = viewer;
                        psi.Arguments = "\"" + path + "\"";

                        Process.Start(psi);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot open picture viewer.");
                    Log.RegisterLog("avatar", ex);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }
        

        void RejectUser_Click(object sender, EventArgs e)
        {
            Service.RejectFriend(user.Id);
        }

        void AcceptUser_Click(object sender, EventArgs e)
        {
            Service.AcceptFriend(user.Id);
        }

        void AddUser_Click(object sender, EventArgs e)
        {
            Service.RequestFriend(user.Id);
        }


        private void LoadBadges(BadgeNotification[] badges)
        {
            View.userBadges1.Badges = badges;
            if (badges != null)
            {
                Thread t = new Thread(new ThreadStart(delegate()
                {
                    foreach (BadgeNotification b in badges)
                    {
                        if (!string.IsNullOrEmpty(b.ImageUrl))
                        {
                            try
                            {
                                byte[] image = Service.DownloadImageSync(b.ImageUrl);

                                if (!View.userBadges1.imageList.ContainsKey(b.ImageUrl))
                                {
                                    View.userBadges1.imageList.Add(b.ImageUrl, image);
                                    View.Invoke(new ThreadStart(delegate() { View.userBadges1.listBox.Invalidate(); }));
                                }
                            }
                            catch (ObjectDisposedException) { return; }
                            catch (Exception ex) { Log.RegisterLog("image", ex); }
                        }
                    }
                }));
                t.StartThread();
            }

        }


        void Service_FriendsResult(object serder, FriendsEventArgs e)
        {
            LoadFriends(e.Friends);
        }



        private void LoadFriends(User[] users)
        {
            this.user.Friends = users;
            try
            {
                if (View.InvokeRequired)
                {
                    View.Invoke(new ThreadStart(delegate() { LoadFriends(users); }));
                    return;
                }
            }
            catch (ObjectDisposedException) { return; }

            View.userFriends1.ImageList = new Dictionary<string, byte[]>();
            if (users != null)
            {
                foreach (var user in users)
                {
                    View.userFriends1.listBox.AddItem(null, user);
                }


                Thread t = new Thread(new ThreadStart(delegate()
                {
                    int i = 0;
                    foreach (User u in users)
                    {
                        if (!string.IsNullOrEmpty(u.ImageUrl))
                        {
                            try
                            {
                                //TODO: Load friends of friends avatars on demand.
                                //This is a workaround to not let the dataplan leak.
                                if (i > 10)
                                    break;
                                if (!Service.IsInCache(u.ImageUrl))
                                    i++;
                                
                                byte[] image = Service.DownloadImageSync(u.ImageUrl);

                                if (!View.userFriends1.ImageList.ContainsKey(u.ImageUrl))
                                {
                                    View.userFriends1.ImageList.Add(u.ImageUrl, image);
                                    View.Invoke(new ThreadStart(delegate() { View.userFriends1.listBox.Invalidate(); }));
                                }
                            }
                            catch (ObjectDisposedException) { return; }
                            catch { }
                        }
                    }
                }));
                t.StartThread();
            }
        }

    }
}
