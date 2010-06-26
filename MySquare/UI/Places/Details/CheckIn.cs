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
    public partial class CheckIn : UserControl, IPanel
    {
        public CheckIn()
        {
            InitializeComponent();
            Program.Service.Error += new MySquare.FourSquare.ErrorEventHandler(Service_Error);
            Program.Service.CheckInResult += new MySquare.FourSquare.CheckInEventHandler(Service_CheckInResult);
        }

        AutoResetEvent wait = new AutoResetEvent(false);
        MySquare.FourSquare.CheckIn result = null;
        void Service_CheckInResult(object serder, MySquare.FourSquare.CheckInEventArgs e)
        {
            result = e.CheckIn;
            wait.Set();
        }

        void Service_Error(object serder, MySquare.FourSquare.ErrorEventArgs e)
        {
            wait.Set();
        }

        MenuItem leftSoft; MenuItem rightSoft;
        public void ActivateControl(MenuItem leftSoft, MenuItem rightSoft)
        {
            this.leftSoft = leftSoft;
            this.rightSoft = rightSoft;

            Dock = DockStyle.Fill;
            BringToFront();
            Visible = true;

            txtShout.Text = string.Empty;

            chkTellFriends.Checked = true;
            chkFacebook.CheckState = CheckState.Indeterminate;
            chkTwitter.CheckState = CheckState.Indeterminate;

            EnableInterface();

        }

        internal MySquare.FourSquare.Venue Venue
        { get; set; }

        internal void DoCheckIn()
        {
            rightSoft.Enabled = false;
            leftSoft.Enabled = false;
            txtShout.Enabled = false;
            chkFacebook.Enabled = false;
            chkTwitter.Enabled = false;
            chkTellFriends.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();

            bool? twitter = null;
            bool? facebook = null;
            if (chkTwitter.CheckState != CheckState.Indeterminate)
                twitter = chkTwitter.CheckState == CheckState.Checked;
            if (chkFacebook.CheckState != CheckState.Indeterminate)
                facebook = chkFacebook.CheckState == CheckState.Checked;

            result = null;
            wait.Reset();
            Program.Service.CheckIn(Venue, txtShout.Text, chkTellFriends.Checked, facebook, twitter);
            if (wait.WaitOne() && result != null)
                MessageBox.Show(result.Message);
            else
                MessageBox.Show("Cannot check in. Try again later.");

        }

        private void EnableInterface()
        {
            rightSoft.Enabled = true;
            leftSoft.Enabled = true;
            txtShout.Enabled = true;
            chkFacebook.Enabled = true;
            chkTwitter.Enabled = true;
            chkTellFriends.Enabled = true;
            Cursor.Current = Cursors.Default;
            Cursor.Show();

        }

        private void txtShout_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }
    }
}
