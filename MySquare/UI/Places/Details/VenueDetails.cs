using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySquare.FourSquare;
using MySquare.Service;

namespace MySquare.UI.Places
{
    internal partial class VenueDetails : UserControl, IView, IViewWithTabs
    {
        public VenueDetails()
        {
            InitializeComponent();
            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);
            Color bgColor = Tenor.Mobile.Drawing.Strings.ToColor("#C5CCD4");
            venueInfo1.BackColor = bgColor;
            venueMap1.BackColor = bgColor;
            venueMap1.picMap.BackColor = bgColor;
            venueTips1.BackColor = bgColor;
            venueTips1.listBox.BackColor = bgColor;
            venueTips1.lblError.BackColor = bgColor;
            venueTips1.panel1.BackColor = bgColor;
            venueTips1.txtComment.BackColor = bgColor;


            tabStrip1.Tabs.Add("Check In");
            tabStrip1.Tabs.Add("Info");
            tabStrip1.Tabs.Add("Map");
            tabStrip1.Tabs.Add("Tips");
            tabStrip1.Tabs.Add("Here");
        }


        private void tabStrip1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabChanged != null)
                TabChanged(this, e);
        }
        internal MySquare.Controller.VenueDetailsController.VenueSection SelectedTab
        {
            get
            {
                return
                    (MySquare.Controller.VenueDetailsController.VenueSection)
                        tabStrip1.SelectedIndex;
            }
        }


        public event EventHandler TabChanged;

    }
}
