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
            main.header.Tabs[1].Selected = true;
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
            if (e.Exception is UnauthorizedAccessException)
                ShowError(e.Exception);
            else
                View.Invoke(new ThreadStart(delegate()
                {
                    View.lblFriendStatus.Text = "unable to read data.";
                }));
        }


        User user;
        internal void LoadUser(MySquare.FourSquare.User user)
        {

            if (View.InvokeRequired)
            {
                View.Invoke(new ThreadStart(delegate()
                {
                    LoadUser(user);
                }));
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
                View.Avatar = Service.DownloadImageSync(user.ImageUrl);
            else
                View.Avatar = null;

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

            View.userInfo1.Badges = user.Badges;

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
                            break;
                        case FriendStatus.pendingthem:
                            View.lblFriendStatus.Text = "have not answered yet";
                            break;
                        default:
                            break;
                    }
                else
                    View.lblFriendStatus.Text = string.Empty;
            }


            if (user.fullData && user.Friends == null)
                Service.GetFriends(user.Id);
            else
                LoadFriends(user.Friends);

            LoadBadges(user.Badges);
        }

        void RejectUser_Click(object sender, EventArgs e)
        {
            Service.RejectFriend(user.Id);
        }

        void AcceptUser_Click(object sender, EventArgs e)
        {
            Service.AcceptFriend(user.Id);
        }


        private void LoadBadges(Badge[] badges)
        {
            View.userInfo1.imageList = new Dictionary<string, byte[]>();
            if (badges != null)
            {
                Thread t = new Thread(new ThreadStart(delegate()
                {
                    foreach (Badge b in badges)
                    {
                        if (!string.IsNullOrEmpty(b.ImageUrl))
                        {
                            try
                            {
                                byte[] image = Service.DownloadImageSync(b.ImageUrl);

                                if (!View.userInfo1.imageList.ContainsKey(b.ImageUrl))
                                {
                                    View.userInfo1.imageList.Add(b.ImageUrl, image);
                                    View.Invoke(new ThreadStart(delegate() { View.userInfo1.pnlBadges.Invalidate(); }));
                                }
                            }
                            catch { }
                        }
                    }
                }));
                t.Start();
            }
        }


        void Service_FriendsResult(object serder, FriendsEventArgs e)
        {
            LoadFriends(e.Friends);
        }



        private void LoadFriends(User[] users)
        {
            this.user.Friends = users;
            if (View.InvokeRequired)
            {
                View.Invoke(new ThreadStart(delegate() { LoadFriends(users); }));
                return;
            }

            View.userFriends1.listBox.Clear();
            if (users != null)
            {
                foreach (var user in users)
                {
                    View.userFriends1.listBox.AddItem(null, user);
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
                                    if (!View.userFriends1.imageList.ContainsKey(u.ImageUrl))
                                    {
                                        View.userFriends1.imageList.Add(u.ImageUrl, bmp);
                                        View.Invoke(new ThreadStart(delegate() { View.userFriends1.listBox.Invalidate(); }));
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
}
