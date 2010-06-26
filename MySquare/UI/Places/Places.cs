﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MySquare.UI.Places
{
    public partial class Places : UserControl, IPanel
    {
        public Places()
        {
            InitializeComponent();
            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);

        }

        #region IPanel Members

        MenuItem leftSoft, rightSoft;
        bool firstTime = true;
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
            ChangeView(0);
            if (firstTime)
            {
                list1.Refresh();
                firstTime = false;
            }
            
        }


        void leftSoft_Click(object sender, EventArgs e)
        {
            if (this.Visible && list1.Visible)
                list1.Refresh();
            else if (this.Visible && venueDetails1.Visible)
                ChangeView(0);
        }


        void rightSoft_Click(object sender, EventArgs e)
        {
            if (this.Visible && venueDetails1.Visible)
                venueDetails1.CheckIn();
        }
        #endregion

        private void list1_ItemSelected(object sender, EventArgs e)
        {
            venueDetails1.OpenVenue(list1.SelectedVenue);
            ChangeView(1);
        }

        private void ChangeView(int index)
        {
            switch (index)
            {
                case 0:
                    leftSoft.Text = "&Refresh";
                    rightSoft.Text = "&Menu";
                    rightSoft.MenuItems.Clear();
                    rightSoft.MenuItems.Add(new MenuItem()
                    {
                        Text = "&Create Venue"
                    });

                    list1.Activate();
                    venueDetails1.Visible = false;
                    break;
                case 1:

                    this.leftSoft.Text = "&Back";
                    this.rightSoft.Text = "&Check-in";
                    this.rightSoft.MenuItems.Clear();

                    venueDetails1.Activate();
                    list1.Visible = false;
                    break;
            }
        }
    }
}
