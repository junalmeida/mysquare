using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.FourSquare;
using System.Windows.Forms;

namespace MySquare.Controller
{
    class VenueDetailsController : BaseController
    {
        UI.Places.VenueDetails view;
        public VenueDetailsController(UI.Places.VenueDetails view)
        {
            this.view = view;
            this.view.TabChanged += new EventHandler(venueDetails_TabChanged);
            Service.CheckInResult+=new CheckInEventHandler(Service_CheckInResult);
        }

        void venueDetails_TabChanged(object sender, EventArgs e)
        {
            OpenSection((VenueSection)view.tabStrip1.SelectedIndex);
        }

        protected override void Activate()
        {
            UI.Places.Places places = view.Parent as UI.Places.Places;

            places.list1.Visible = false;

            view.Dock = System.Windows.Forms.DockStyle.Fill;
            view.BringToFront();
            view.Visible = true;

            view.checkIn1.txtShout.Text = string.Empty;
            view.checkIn1.chkTellFriends.Checked = true;

            view.tabStrip1.SelectedIndex = 0;

            OpenSection(VenueSection.CheckIn);
        }

        Venue venue;
        internal void OpenVenue(Venue venue)
        {
            this.venue = venue;

            view.lblVenueName.Text = venue.Name;
            List<string> address = new List<string>();
            if (!string.IsNullOrEmpty(venue.Address))
                address.Add(venue.Address);
            if (!string.IsNullOrEmpty(venue.City))
                address.Add(venue.City);
            if (!string.IsNullOrEmpty(venue.State))
                address.Add(venue.State);
            view.lblAddress.Text = string.Join(", ", address.ToArray());

            view.tabStrip1.SelectedIndex = 0;
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
                    view.venueInfo1.Visible = false;
                    view.venueMap1.Visible = false;
                    view.venueTips1.Visible = false;

                    view.checkIn1.Activate();


                    LeftSoftButtonEnabled = true;
                    break;
                case VenueSection.Info:
                    view.checkIn1.Visible = false;
                    view.venueMap1.Visible = false;
                    view.venueTips1.Visible = false;
                    view.venueInfo1.Activate();

                    LeftSoftButtonEnabled = false;
                    break;
                case VenueSection.Map:
                    view.venueInfo1.Visible = false;
                    view.checkIn1.Visible = false;
                    view.venueTips1.Visible = false;
                    view.venueMap1.Activate();

                    LeftSoftButtonEnabled = false;
                    break;
                case VenueSection.Tips:
                    view.venueInfo1.Visible = false;
                    view.venueMap1.Visible = false;
                    view.checkIn1.Visible = false;
                    view.venueTips1.Activate();

                    LeftSoftButtonEnabled = false;
                    break;
            }
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
            view.checkIn1.txtShout.Enabled = false;
            view.checkIn1.chkFacebook.Enabled = false;
            view.checkIn1.chkTwitter.Enabled = false;
            view.checkIn1.chkTellFriends.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();

            bool? twitter = null;
            bool? facebook = null;
            if (view.checkIn1.chkTwitter.CheckState != CheckState.Indeterminate)
                twitter = view.checkIn1.chkTwitter.CheckState == CheckState.Checked;
            if (view.checkIn1.chkFacebook.CheckState != CheckState.Indeterminate)
                facebook = view.checkIn1.chkFacebook.CheckState == CheckState.Checked;

            result = null;
            WaitThread.Reset();
            Service.CheckIn(Venue,
                view.checkIn1.txtShout.Text,
                view.checkIn1.chkTellFriends.Checked, 
                facebook, twitter);
            WaitThread.WaitOne();
            if (result != null)
            {
                MessageBox.Show(result.Message, "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            }
        }
        #endregion


    }
}

