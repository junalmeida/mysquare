using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using MySquare.Controller;
using MySquare.FourSquare;
using MySquare.Properties;
using Tenor.Mobile.Drawing;

namespace MySquare.UI.Friends
{
    partial class UserInfo : UserControl
    {
        public UserInfo()
        {
            InitializeComponent();
        }


        private void lnkFoursquare_Click(object sender, EventArgs e)
        {
            if (lnkFoursquare.Tag != null)
            {
                ProcessStartInfo psi =
                      new ProcessStartInfo("http://foursquare.com/mobile/" + (string)lnkFoursquare.Tag, string.Empty);
                Process.Start(psi);
            }

        }

        private void lblEmail_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblEmail.Text))
            {
                ProcessStartInfo psi =
                      new ProcessStartInfo("mailto:" + lblEmail.Text, string.Empty);
                Process.Start(psi);
            }
        }

        private void lblTwitter_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblTwitter.Text))
            {
                ProcessStartInfo psi =
                   new ProcessStartInfo("http://twitter.com/" + lblTwitter.Text, string.Empty);
                Process.Start(psi);
            }
        }

        private void lblFacebook_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblFacebook.Text))
            {
                ProcessStartInfo psi =
                    new ProcessStartInfo("http://m.facebook.com/profile.php?id=" + lblFacebook.Tag as string, string.Empty);
                Process.Start(psi);
            }
        }

        AlphaImage imgEmail = null;
        AlphaImage imgFacebook = null;
        AlphaImage imgTwitter = null;
        AlphaImage imgFoursquare = null;
        private void UserInfo_Paint(object sender, PaintEventArgs e)
        {
            if (imgEmail == null)
                imgEmail = new AlphaImage(Resources.Email);
            if (imgFacebook == null)
                imgFacebook = new AlphaImage(Resources.Facebook);
            if (imgTwitter == null)
                imgTwitter = new AlphaImage(Resources.Twitter);
            if (imgFoursquare == null)
                imgFoursquare = new AlphaImage(Resources.Foursquare);

            int padd = (3 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width);
            imgFoursquare.Draw(e.Graphics,
                new Rectangle(
                    lnkFoursquare.Left - lnkFoursquare.Height,
                    lnkFoursquare.Top,
                    lnkFoursquare.Height - padd, lnkFoursquare.Height - padd));
            imgEmail.Draw(e.Graphics,
                new Rectangle(
                    lblEmail.Left - lblEmail.Height,
                    lblEmail.Top,
                    lblEmail.Height - padd, lblEmail.Height - padd));
            imgFacebook.Draw(e.Graphics,
                new Rectangle(
                    lblFacebook.Left - lblFacebook.Height,
                    lblFacebook.Top,
                    lblFacebook.Height - padd, lblFacebook.Height - padd));
            imgTwitter.Draw(e.Graphics,
                new Rectangle(
                    lblTwitter.Left - lblTwitter.Height,
                    lblTwitter.Top,
                    lblTwitter.Height - padd, lblTwitter.Height - padd));
        }



        private void lblShout_TextChanged(object sender, EventArgs e)
        {
            lblShout.Visible = !string.IsNullOrEmpty(lblShout.Text);
            lblShoutT.Visible = lblShout.Visible;
#if DEBUG
            if (Environment.OSVersion.Platform != PlatformID.WinCE)
                return;
#endif
            using (Graphics g = this.CreateGraphics())
            {
                Size size = Tenor.Mobile.Drawing.Strings.Measure(g, lblShout.Text, lblShout.Font, new Rectangle(0, 0, lblShout.Width, this.Height));
                lblShout.Height = size.Height + (5 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height);
            }
        }

        private void lblLastSeen_TextChanged(object sender, EventArgs e)
        {
            lblLastSeen.Visible = !string.IsNullOrEmpty(lblLastSeen.Text);
            lblLastSeenT.Visible = lblLastSeen.Visible;

        }




        #region Scroll Control
        int originalMouse;
        int originalScroll;
        private void VenueInfo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originalMouse = e.Y;
                originalScroll = this.AutoScrollPosition.Y;
            }
        }

        private void VenueInfo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.AutoScrollPosition = new Point(0, (originalMouse - e.Y) - originalScroll);
            }
        }
        #endregion

        private void lblLastSeen_Click(object sender, EventArgs e)
        {
            Venue venue = lblLastSeen.Tag as Venue;
            if (venue != null)
                (BaseController.OpenController((Parent.Parent as Main).venueDetails1) as VenueDetailsController).OpenVenue(venue);
        }


    }
}