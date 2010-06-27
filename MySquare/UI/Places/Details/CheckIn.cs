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
            Program.Service.CheckInResult += new MySquare.FourSquare.CheckInEventHandler(Service_CheckInResult);
        }

        MySquare.FourSquare.CheckIn result = null;
        void Service_CheckInResult(object serder, MySquare.FourSquare.CheckInEventArgs e)
        {
            result = e.CheckIn;
            Program.WaitThread.Set();
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
            Program.WaitThread.Reset();
            Program.Service.CheckIn(Venue, txtShout.Text, chkTellFriends.Checked, facebook, twitter);
            Program.WaitThread.WaitOne();
            if (result != null)
                MessageBox.Show(result.Message);
            EnableInterface();
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
