using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.UI;
using System.Windows.Forms;
using MySquare.FourSquare;
using System.Threading;
using System.Drawing;
using System.IO;

namespace MySquare.Controller
{
    class ShoutController : BaseController<UI.More.Shout>
    {
        public ShoutController(UI.More.Shout view)
            : base(view)
        {
            Service.CheckInResult += new MySquare.FourSquare.CheckInEventHandler(Service_CheckInResult);
            Service.Error += new MySquare.Service.ErrorEventHandler(Service_Error);
        }

        void Service_Error(object serder, MySquare.Service.ErrorEventArgs e)
        {
            ShowError(e.Exception);
        }



        public override void Activate()
        {
            (View.Parent as Main).Reset();
            View.BringToFront();
            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.Visible = true;

            LeftSoftButtonEnabled = true;
            LeftSoftButtonText = "&Shout";
            RightSoftButtonEnabled = true;
            RightSoftButtonText = "&Back";

            View.checkIn1.pnlShout.Visible = true;
            View.checkIn1.pnlCheckInResult.Visible = false;
            View.checkIn1.pnlShout.Enabled = true;

        }


        public override void OnLeftSoftButtonClick()
        {
            DoShout();
        }

 
        public override void OnRightSoftButtonClick()
        {
            if (RightSoftButtonText == "&Cancel")
            {
                Service.Abort();
                Activate();
            }
            else
                OpenController((View.Parent as Main).moreActions1);
        }

        private void DoShout()
        {
            if (View.checkIn1.txtShout.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Type in a message to shout.", "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            View.checkIn1.pnlShout.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();

            bool? twitter = null;
            bool? facebook = null;
            if (View.checkIn1.chkTwitter.CheckState != CheckState.Indeterminate)
                twitter = View.checkIn1.chkTwitter.CheckState == CheckState.Checked;
            if (View.checkIn1.chkFacebook.CheckState != CheckState.Indeterminate)
                facebook = View.checkIn1.chkFacebook.CheckState == CheckState.Checked;

            Service.CheckIn(
                View.checkIn1.txtShout.Text,
                View.checkIn1.chkTellFriends.Checked,
                facebook, twitter,
                    Program.Location.WorldPoint.Latitude,
                    Program.Location.WorldPoint.Longitude, 
                    Program.Location.WorldPoint.Altitude,
                    Program.Location.WorldPoint.HorizontalDistance);

            RightSoftButtonText = "&Cancel";
        }

        CheckIn checkInResult;
        void Service_CheckInResult(object serder, MySquare.FourSquare.CheckInEventArgs e)
        {
            checkInResult = e.CheckIn;
            CheckInResult();
        }
        private void CheckInResult()
        {
            if (View.InvokeRequired)
            {
                View.Invoke(new ThreadStart(CheckInResult));
                return;
            }
            if (checkInResult != null)
            {

                View.checkIn1.pnlShout.Visible = false;
                View.checkIn1.pnlCheckInResult.Visible = true;

                View.checkIn1.message = checkInResult.Message;

                if (checkInResult.Badges != null && checkInResult.Badges.Length > 0)
                    View.checkIn1.badges = checkInResult.Badges;
                else
                    View.checkIn1.badges = null;

                if (checkInResult.Scoring != null && checkInResult.Scoring.Length > 0)
                    View.checkIn1.scoring = checkInResult.Scoring;
                else
                    View.checkIn1.scoring = null;

                if (checkInResult.Specials != null && checkInResult.Specials.Length > 0)
                    View.checkIn1.specials = checkInResult.Specials;
                else
                    View.checkIn1.specials = null;
            }

            View.checkIn1.txtShout.Text = string.Empty;
            Cursor.Current = Cursors.Default;
            LeftSoftButtonEnabled = false;
            RightSoftButtonText = "&Back";
        }

    }
}
