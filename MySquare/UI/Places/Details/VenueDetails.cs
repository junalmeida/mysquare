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
    public partial class VenueDetails : UserControl, IView, IViewWithTabs
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
            //tabStrip1.Tabs.Add("Tips");
        }


        private void tabStrip1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabChanged != null)
                TabChanged(this, e);
        }

        #region IViewWithTabs Members

        public event EventHandler TabChanged;

        #endregion
    }
}
