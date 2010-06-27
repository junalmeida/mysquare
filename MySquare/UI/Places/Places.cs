using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySquare.Controller;

namespace MySquare.UI.Places
{
    public partial class Places : UserControl, IView
    {
        public Places()
        {
            InitializeComponent();
            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);
        }

        private void list1_ItemSelected(object sender, EventArgs e)
        {
            VenueDetailsController controller = (VenueDetailsController)BaseController.OpenController(venueDetails1);
            controller.OpenVenue(list1.SelectedVenue);
        }
 
    }
}
