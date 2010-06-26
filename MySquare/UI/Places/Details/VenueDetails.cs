using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySquare.FourSquare;

namespace MySquare.UI.Places
{
    public partial class VenueDetails : UserControl
    {
        public VenueDetails()
        {
            InitializeComponent();
            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);

            tabStrip1.Tabs.Add("Check In");
            tabStrip1.Tabs.Add("Info");
            tabStrip1.Tabs.Add("Map");
            tabStrip1.Tabs.Add("Tips");
        }

        Venue venue;
        internal void OpenVenue(Venue venue)
        {
            this.venue = venue;

            lblVenueName.Text = venue.Name;
            List<string> address = new List<string>();
            if (!string.IsNullOrEmpty(venue.Address))
                address.Add(venue.Address);
            if (!string.IsNullOrEmpty(venue.City))
                address.Add(venue.City);
            if (!string.IsNullOrEmpty(venue.State))
                address.Add(venue.State);
            lblAddress.Text = string.Join(", ", address.ToArray());

            ChangeView(0);
        }

        internal void CheckIn()
        {
            checkIn1.DoCheckIn();
        }

        internal void Activate()
        {
            Dock = DockStyle.Fill;
            Visible = true;
        }

        private void tabStrip1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeView(tabStrip1.SelectedIndex);
        }

        private void ChangeView(int i)
        {
            switch (i)
            {
                case 0:
                    venueInfo1.Visible = false;
                    venueMap1.Visible = false;
                    venueTips1.Visible = false;
                    checkIn1.Activate();
                    break;
                case 1:
                    checkIn1.Visible = false;
                    venueMap1.Visible = false;
                    venueTips1.Visible = false;
                    venueInfo1.Activate();
                    break;
                case 2:
                    venueInfo1.Visible = false;
                    checkIn1.Visible = false;
                    venueTips1.Visible = false;
                    venueMap1.Activate();
                    break;
                case 3:
                    venueInfo1.Visible = false;
                    venueMap1.Visible = false;
                    checkIn1.Visible = false;
                    venueTips1.Activate();
                    break;
            }
        }

    }
}
