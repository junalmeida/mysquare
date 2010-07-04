using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MySquare.UI.Places.Details
{
    internal partial class CheckIn : UserControl
    {
        public CheckIn()
        {
            InitializeComponent();
        }

        private void txtShout_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void chkTellFriends_CheckStateChanged(object sender, EventArgs e)
        {
            if (!chkTellFriends.Checked)
            {
                chkTwitter.Enabled = false;
                chkFacebook.Enabled = false;
                chkTwitter.CheckState = CheckState.Unchecked;
                chkFacebook.CheckState = CheckState.Unchecked;
            }
            else
            {
                chkTwitter.Enabled = true;
                chkFacebook.Enabled = true;
                chkTwitter.CheckState = CheckState.Indeterminate;
                chkFacebook.CheckState = CheckState.Indeterminate;
            }
        }

        internal void Activate()
        {
            Dock = DockStyle.Fill;
            BringToFront();
            Visible = true;
        }
    }
}
