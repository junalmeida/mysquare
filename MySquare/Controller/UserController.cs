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

namespace MySquare.Controller
{
    class UserController : BaseController<UserDetail>
    {

        public UserController(UserDetail view)
            : base(view)
        {
            Service.UserResult+=new UserEventHandler(Service_UserResult);
            Service.FriendsResult += new FriendsEventHandler(Service_FriendsResult);
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
        }

        public override void OnLeftSoftButtonClick()
        {
            base.OnLeftSoftButtonClick();
        }

        public override void OnRightSoftButtonClick()
        {
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
            if (!user.fullData)
                Service.GetUser(user.Id);


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


            if (user.FriendStatus.HasValue)
                switch (user.FriendStatus.Value)
                {
                    case FriendStatus.friend:
                        View.lblFriendStatus.Text = "is your friend";
                        break;
                    case FriendStatus.pendingyou:
                        View.lblFriendStatus.Text = "is waiting you to accept";
                        break;
                    case FriendStatus.pendingthem:
                        View.lblFriendStatus.Text = "have not answered yet";
                        break;
                    default:
                        break;
                }
            else
                View.lblFriendStatus.Text = string.Empty;

            if (user.fullData && user.Friends == null)
                Service.GetFriends(user.Id);
            else
                LoadFriends(user.Friends);


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
