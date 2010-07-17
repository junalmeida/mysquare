using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Tenor.Mobile.Drawing;
using MySquare.Properties;
using MySquare.FourSquare;
using MySquare.Controller;

namespace MySquare.UI.Friends
{
    public partial class UserInfo : UserControl
    {
        public UserInfo()
        {
            InitializeComponent();
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
        private void UserInfo_Paint(object sender, PaintEventArgs e)
        {
            if (imgEmail == null)
                imgEmail = new AlphaImage(Resources.Email);
            if (imgFacebook == null)
                imgFacebook = new AlphaImage(Resources.Facebook);
            if (imgTwitter == null)
                imgTwitter = new AlphaImage(Resources.Twitter);

            int padd = (3 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width);
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

        }

        private void lblLastSeen_TextChanged(object sender, EventArgs e)
        {
            lblLastSeen.Visible = !string.IsNullOrEmpty(lblLastSeen.Text);
            lblLastSeenT.Visible = lblLastSeen.Visible;

        }

        internal Dictionary<string, byte[]> imageList;
        Badge[] badges;

        internal Badge[] Badges
        {
            get { return badges; }
            set
            {
                badges = value;
                pnlBadges.Visible = value != null && value.Length > 0;
                pnlBadges.Height = 32;
                pnlBadges.Invalidate();
                lblBadgesT.Visible = pnlBadges.Visible;

            }
        }


        Size ellipse = new Size(
            8 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width, 8 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height);
        private void pnlBadges_Paint(object sender, PaintEventArgs e)
        {
            
            int stampSize = 28 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;
            int padding = 4 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;
            int separator = padding * 3;
            Rectangle rect = new Rectangle(
                padding, padding,
                ((stampSize + separator) * Convert.ToInt32((pnlBadges.Width - padding) / (stampSize + separator))) - padding, 
                pnlBadges.Height - (padding * 2));

            rect.X = (pnlBadges.Width / 2) - (rect.Width / 2);

            RoundedRectangle.Fill(e.Graphics, new Pen(Color.Gray), new SolidBrush(Color.White), rect, ellipse);


            int left = rect.Left + padding;
            int top = padding * 2;
            int i = 0;
            foreach (Badge b in Badges)
            {
                if (imageList.ContainsKey(b.ImageUrl))
                {
                    AlphaImage image = new AlphaImage(imageList[b.ImageUrl]);
                    image.Draw(e.Graphics, new Rectangle(left, top, stampSize, stampSize));

                    left += stampSize + separator;
                    if (left + stampSize > rect.Right && i < Badges.Length - 1)
                    {
                        left = rect.Left + padding;
                        top += stampSize + separator;
                    }
                }
                i++;
            }

            if (pnlBadges.Height != top + stampSize + (padding * 2))
                pnlBadges.Height = top + stampSize + (padding * 2);
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