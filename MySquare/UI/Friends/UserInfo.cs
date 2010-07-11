using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

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
    }
}
