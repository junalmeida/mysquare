using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.FourSquare;
using System.Windows.Forms;
using System.Threading;

namespace MySquare.Controller
{
    class VenueDetailsController : BaseController
    {
        UI.Places.VenueDetails View;
        public VenueDetailsController(UI.Places.VenueDetails view)
        {
            this.View = view;
            base.view = view;
            this.View.TabChanged += new EventHandler(venueDetails_TabChanged);
            Service.CheckInResult+=new CheckInEventHandler(Service_CheckInResult);
            Service.VenueResult += new VenueEventHandler(Service_VenueResult);
        }



        void venueDetails_TabChanged(object sender, EventArgs e)
        {
            OpenSection((VenueSection)View.tabStrip1.SelectedIndex);
        }

        protected override void Activate()
        {
            UI.Places.Places places = View.Parent as UI.Places.Places;

            places.list1.Visible = false;

            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.BringToFront();
            View.Visible = true;

            View.checkIn1.txtShout.Enabled = true;
            View.checkIn1.chkTellFriends.Enabled = true;

            View.checkIn1.txtShout.Text = string.Empty;
            View.checkIn1.chkTellFriends.Checked = true;


            View.tabStrip1.SelectedIndex = 0;

            OpenSection(VenueSection.CheckIn);
        }

        internal void OpenVenue(Venue venue)
        {
            this.Venue = venue;

            View.lblVenueName.Text = venue.Name;
            List<string> address = new List<string>();
            if (!string.IsNullOrEmpty(venue.Address))
                address.Add(venue.Address);
            if (!string.IsNullOrEmpty(venue.City))
                address.Add(venue.City);
            if (!string.IsNullOrEmpty(venue.State))
                address.Add(venue.State);
            View.lblAddress.Text = string.Join(", ", address.ToArray());

            View.tabStrip1.SelectedIndex = 0;
            OpenSection(VenueSection.CheckIn);
        }

        enum VenueSection
        {
            CheckIn,
            Info, 
            Map,
            Tips
        }

        void OpenSection(VenueSection section)
        {
            RightSoftButtonText = "&Back";
            RightSoftButtonEnabled = true;

            LeftSoftButtonText = "&Check In";
            switch (section)
            {
                case VenueSection.CheckIn:
                    View.venueInfo1.Visible = false;
                    View.venueMap1.Visible = false;
                    View.venueTips1.Visible = false;

                    View.checkIn1.Activate();


                    LeftSoftButtonEnabled = true;
                    break;
                case VenueSection.Info:
                    View.checkIn1.Visible = false;
                    View.venueMap1.Visible = false;
                    View.venueTips1.Visible = false;
                    View.venueInfo1.Activate();

                    LoadExtraInfo();

                    LeftSoftButtonEnabled = false;
                    break;
                case VenueSection.Map:
                    View.venueInfo1.Visible = false;
                    View.checkIn1.Visible = false;
                    View.venueTips1.Visible = false;
                    View.venueMap1.Activate();

                    LeftSoftButtonEnabled = false;
                    break;
                case VenueSection.Tips:
                    View.venueInfo1.Visible = false;
                    View.venueMap1.Visible = false;
                    View.checkIn1.Visible = false;
                    View.venueTips1.Activate();

                    LeftSoftButtonEnabled = false;
                    break;
            }
        }


        protected override void OnLeftSoftButtonClick()
        {
            DoCheckIn();
        }

        protected override void OnRightSoftButtonClick()
        {
            BaseController.OpenController(View.Parent as MySquare.UI.IView);
        }

        internal MySquare.FourSquare.Venue Venue
        { get; set; }

        #region CheckIn
        MySquare.FourSquare.CheckIn result = null;
        void Service_CheckInResult(object serder, MySquare.FourSquare.CheckInEventArgs e)
        {
            result = e.CheckIn;
            WaitThread.Set();
        }

        internal void DoCheckIn()
        {
            View.checkIn1.txtShout.Enabled = false;
            View.checkIn1.chkFacebook.Enabled = false;
            View.checkIn1.chkTwitter.Enabled = false;
            View.checkIn1.chkTellFriends.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();

            bool? twitter = null;
            bool? facebook = null;
            if (View.checkIn1.chkTwitter.CheckState != CheckState.Indeterminate)
                twitter = View.checkIn1.chkTwitter.CheckState == CheckState.Checked;
            if (View.checkIn1.chkFacebook.CheckState != CheckState.Indeterminate)
                facebook = View.checkIn1.chkFacebook.CheckState == CheckState.Checked;

            result = null;
            WaitThread.Reset();
            Service.CheckIn(Venue,
                View.checkIn1.txtShout.Text,
                View.checkIn1.chkTellFriends.Checked, 
                facebook, twitter);
            WaitThread.WaitOne();

            if (result != null)
                MessageBox.Show(result.Message, "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            else
                OnRightSoftButtonClick();
        }
        #endregion


        #region ExtraInfo

        private void LoadExtraInfo()
        {
            if (View.InvokeRequired)
                View.Invoke(new ThreadStart(this.LoadExtraInfo));
            else
            {

                if (!Venue.fullData)
                {
                    Service.GetVenue(Venue.Id);
                }


                if (Venue.PrimaryCategory != null)
                    View.venueInfo1.lblCategory.Text = Venue.PrimaryCategory.FullName.Replace(":", " > ");
                else
                    View.venueInfo1.lblCategory.Text = null;

                View.venueInfo1.lblPhone.Text = Venue.Phone;

                if (Venue.Status != null && Venue.Status.Mayor != null)
                    View.venueInfo1.lblMayor.Text =
                        string.Concat(Venue.Status.Mayor.User.FirstName, " ", Venue.Status.Mayor.User.LastName);
                else
                    View.venueInfo1.lblMayor.Text = null;

                if (Venue.Status != null && (Venue.Status.HereNow > 0 || Venue.Status.CheckIns > 0))
                    View.venueInfo1.lblStats.Text =
                        string.Concat(Venue.Status.HereNow, " friends here, ", Venue.Status.CheckIns, " check-ins.");
                else
                    View.venueInfo1.lblStats.Text = null;

                View.venueInfo1.imgCategory.Image = null;
                if (Venue.PrimaryCategory != null && !string.IsNullOrEmpty(Venue.PrimaryCategory.IconUrl))
                    DownloadImageToPictureBox(Venue.PrimaryCategory.IconUrl, View.venueInfo1.imgCategory);

                View.venueInfo1.imgMayor.Image = null;
                if (Venue.Status != null && Venue.Status.Mayor != null && !string.IsNullOrEmpty(Venue.Status.Mayor.User.ImageUrl))
                    DownloadImageToPictureBox(Venue.Status.Mayor.User.ImageUrl, View.venueInfo1.imgMayor);
            }
        }
        void Service_VenueResult(object serder, VenueEventArgs e)
        {
            Venue = e.Venue;
            LoadExtraInfo();
        }
        #endregion

    }
}

