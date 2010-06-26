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

            txtShout.Text = string.Empty;
            chkFacebook.Checked = false;
            chkTellFriends.Checked = true;
            chkTwitter.Checked = false;
        }

        internal void CheckIn()
        {
        }
    }
}
