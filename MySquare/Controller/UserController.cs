using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.UI.Friends;
using MySquare.UI;
using MySquare.FourSquare;

namespace MySquare.Controller
{
    class UserController : BaseController<UserDetail>
    {

        public UserController(UserDetail view)
            : base(view)
        {
        }


        public override void Activate()
        {
            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.BringToFront();
            View.Visible = true;

            Service.UserResult += new MySquare.FourSquare.UserEventHandler(Service_UserResult);

            Main main = (View.Parent as Main);
            User user = null;
            if (main.friends1.listBox.SelectedItem.Value != null) 
                if (main.friends1.listBox.SelectedItem.Value is User)
                    user = (User)main.friends1.listBox.SelectedItem.Value;
                else
                    user = ((CheckIn)main.friends1.listBox.SelectedItem.Value).User;
            if (user != null)
                LoadUser(user);
        }

        public override void Deactivate()
        {
            View.Avatar = null;
            View.Visible = false;
            Service.UserResult -= new MySquare.FourSquare.UserEventHandler(Service_UserResult);
        }

        void Service_UserResult(object serder, MySquare.FourSquare.UserEventArgs e)
        {
            LoadUser(e.User);
        }

        User user;
        private void LoadUser(MySquare.FourSquare.User user)
        {
            this.user = user;

            View.lblUserName.Text = string.Format("{0} {1}", user.FirstName, user.LastName);
            if (!string.IsNullOrEmpty(user.ImageUrl))
                View.Avatar = Service.DownloadImageSync(user.ImageUrl);
            else
                View.Avatar = null;
        }

    }
}
