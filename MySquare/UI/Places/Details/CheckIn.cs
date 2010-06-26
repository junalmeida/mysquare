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
        }

        void Service_Error(object serder, MySquare.FourSquare.ErrorEventArgs e)
        {
            this.Invoke(new ThreadStart(delegate()
            {
                EnableInterface();
            }));
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
            chkFacebook.Checked = false;
            chkTellFriends.Checked = true;
            chkTwitter.Checked = false;
        }

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
    }
}
