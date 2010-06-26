using System;
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
            list1.Refresh();
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
                    this.leftSoft.Text = "&Refresh";
                    this.rightSoft.Text = "&Menu";
                    this.rightSoft.MenuItems.Clear();
                    this.rightSoft.MenuItems.Add(new MenuItem()
                    {
                        Text = "&Create Venue"
                    });

                    list1.Dock = DockStyle.Fill;
                    list1.Visible = true;
                    venueDetails1.Visible = false;
                    break;
                case 1:

                    this.leftSoft.Text = "&Back";
                    this.rightSoft.Text = "&Check-in";
                    this.rightSoft.MenuItems.Clear();

                    venueDetails1.Dock = DockStyle.Fill;
                    venueDetails1.Visible = true;
                    list1.Visible = false;
                    break;
            }
        }
    }
}
