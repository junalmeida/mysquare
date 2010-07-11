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
    }
}
