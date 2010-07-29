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
            checkIn1.BackColor = bgColor;
            checkIn1.txtShout.BackColor = bgColor;
            venueInfo1.BackColor = bgColor;
            venueMap1.BackColor = bgColor;
            venueMap1.picMap.BackColor = bgColor;
            venueTips1.BackColor = bgColor;
            venueTips1.listBox.BackColor = bgColor;
            venueTips1.lblError.BackColor = bgColor;
            venueTips1.panel1.BackColor = bgColor;
            venueTips1.txtComment.BackColor = bgColor;


            tabs.Add("Check In"); tabStrip1.Tabs.Add(tabs[0]);
            tabs.Add("Info"); tabStrip1.Tabs.Add(tabs[1]);
            tabs.Add("Map"); if (Configuration.IsPremium) tabStrip1.Tabs.Add(tabs[2]);
            tabs.Add("Tips"); tabStrip1.Tabs.Add(tabs[3]);


        }


        private void tabStrip1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TabChanged != null)
                TabChanged(this, e);
        }
        List<string> tabs = new List<string>();
        internal MySquare.Controller.VenueDetailsController.VenueSection SelectedTab
        {
            get
            {
                return
                    (MySquare.Controller.VenueDetailsController.VenueSection)
                        tabs.IndexOf(tabStrip1.Tabs[tabStrip1.SelectedIndex]);
            }
        }

        #region IViewWithTabs Members

        public event EventHandler TabChanged;

        #endregion
    }
}
