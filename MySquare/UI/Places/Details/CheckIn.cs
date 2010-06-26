using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MySquare.UI.Places.Details
{
    public partial class CheckIn : UserControl
    {
        public CheckIn()
        {
            InitializeComponent();
        }

        internal void Activate()
        {
            Dock = DockStyle.Fill;
            BringToFront();
            Visible = true;

            txtShout.Text = string.Empty;
            chkFacebook.Checked = false;
            chkTellFriends.Checked = true;
            chkTwitter.Checked = false;
        }

        internal void DoCheckIn()
        {
            MessageBox.Show("Test");
        }
    }
}
