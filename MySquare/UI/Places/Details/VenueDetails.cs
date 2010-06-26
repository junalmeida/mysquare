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
    public partial class VenueDetails : UserControl, IPanel
    {
        public VenueDetails()
        {
            InitializeComponent();
            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);
            Color bgColor = Tenor.Mobile.Drawing.Strings.ToColor("#C5CCD4");
            checkIn1.BackColor = bgColor;
            venueInfo1.BackColor = bgColor;
            venueMap1.BackColor = bgColor;
            venueTips1.BackColor = bgColor;  

            tabStrip1.Tabs.Add("Check In");
            tabStrip1.Tabs.Add("Info");
            tabStrip1.Tabs.Add("Map");
            tabStrip1.Tabs.Add("Tips");
        }
        MenuItem leftSoft; MenuItem rightSoft;
        public void ActivateControl(MenuItem leftSoft, MenuItem rightSoft)
        {
            if (this.leftSoft != leftSoft)
            {
                this.leftSoft = leftSoft;
                this.leftSoft.Click += new EventHandler(leftSoft_Click);
            }

            if (this.rightSoft != rightSoft)
            {
                this.rightSoft = rightSoft;
                this.rightSoft.Click += new EventHandler(rightSoft_Click);
            }

            leftSoft.Text = "&Back";
            rightSoft.Text = "&Check in";

            BringToFront();
            Dock = DockStyle.Fill;
            Visible = true;
        }

        void rightSoft_Click(object sender, EventArgs e)
        {
            if (Visible)
                checkIn1.DoCheckIn();
        }

        void leftSoft_Click(object sender, EventArgs e)
        {
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

            tabStrip1.SelectedIndex = 0;
            ChangeView(0);
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
                    rightSoft.Enabled = true;
                    break;
                case 1:
                    checkIn1.Visible = false;
                    venueMap1.Visible = false;
                    venueTips1.Visible = false;
                    venueInfo1.Activate();
                    rightSoft.Enabled = false;
                    break;
                case 2:
                    venueInfo1.Visible = false;
                    checkIn1.Visible = false;
                    venueTips1.Visible = false;
                    venueMap1.Activate();
                    rightSoft.Enabled = false;
                    break;
                case 3:
                    venueInfo1.Visible = false;
                    venueMap1.Visible = false;
                    checkIn1.Visible = false;
                    venueTips1.Activate();
                    rightSoft.Enabled = false;
                    break;
            }
        }

    }
}
