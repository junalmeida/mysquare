﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySquare.Controller;
using MySquare.FourSquare;

namespace MySquare.UI.Places
{
    internal partial class Places : UserControl, IView
    {
        public Places()
        {
            InitializeComponent();
            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);
        }

        private void list1_ItemSelected(object sender, EventArgs e)
        {
            if (list1.SelectedVenue == null)
            {
                BaseController.OpenController(((Main)Parent).createVenue1);
            }
            else
            {
                Venue venue = list1.listBox.SelectedItem.Value as Venue;
                if (venue == null)
                    venue = (list1.listBox.SelectedItem.Value as Tip).Venue;

                ((VenueDetailsController)BaseController.OpenController(((Main)Parent).venueDetails1)).OpenVenue(venue);
            }
        }

        public void Reset()
        {
            foreach (Control c in Controls)
                if (c is IView)
                    c.Visible = false;
        }
 
    }
}
