using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Tenor.Mobile.Drawing;
using MySquare.Service;
using System.Diagnostics;

namespace MySquare.UI.Friends
{
    partial class UserDetail : UserControl, IView
    {
        public UserDetail()
        {
            InitializeComponent();
            tabStrip.Tabs.Add("Info");
            tabStrip.Tabs.Add("Friends");
            tabStrip.Tabs.Add("Badges");
        }

        SizeF factor;
        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this.factor = factor;
            base.ScaleControl(factor, specified);
        }


        private void tabStrip1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        Image avatar;
        internal byte[] Avatar
        {
            set
            {
                if (avatar != null)
                {
                    avatar.Dispose();
                    avatar = null;
                }
                if (value != null)
                {
                    using (MemoryStream mem = new MemoryStream(value))
                    using (Bitmap bmp = new Bitmap(mem))
                        avatar = Main.CreateRoundedAvatar(bmp, picAvatar.Width, factor);
                }
                picAvatar.Invalidate();
                ShowHelp();
            }
        }

        bool alreadyHelp = false;
        private void ShowHelp()
        {
            if (!alreadyHelp && Configuration.IsFirstTime)
            {
                alreadyHelp = true;
                Main main = (Parent as Main);
                main.contextHelp1.ShowHelp(
                    new Point(
                        15 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width,
                        95 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height), 
                        "Tap here to see your friend in full screen!");

                main.helpText = new System.Collections.Generic.List<string>();
            }
        }


        private void picAvatar_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (avatar != null)
                {
                    AlphaImage alpha = new AlphaImage(avatar);
                    alpha.Draw(e.Graphics,
                                new Rectangle(
                                    0,
                                    0,
                                    picAvatar.Width,
                                    picAvatar.Height), Tenor.Mobile.UI.Skin.Current.SelectedBackColor);
                }

            }
            catch (Exception ex) { Log.RegisterLog("gdi", ex); }
        }

        private void tabStrip_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectSection(tabStrip.SelectedIndex);
        }

        internal void SelectSection(int section)
        {
            userInfo1.Visible = false;
            userFriends1.Visible = false;
            userBadges1.Visible = false;
            tabStrip.SelectedIndex = section;
            switch (section)
            {
                case 0:
                    userInfo1.BringToFront();
                    userInfo1.Dock = DockStyle.Fill;
                    userInfo1.Visible = true;
                    break;
                case 1:
                    userFriends1.BringToFront();
                    userFriends1.Dock = DockStyle.Fill;
                    userFriends1.Visible = true;
                    break;
                case 2:
                    userBadges1.BringToFront();
                    userBadges1.Dock = DockStyle.Fill;
                    userBadges1.Visible = true;
                    break;
            }
        }


    }
}
