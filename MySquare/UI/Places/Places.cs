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
            BringToFront();
            Dock = DockStyle.Fill;
            Visible = true;

            ChangeView(0);
            if (firstTime)
            {
                list1.RefreshList();
                firstTime = false;
            }
            
        }


        void leftSoft_Click(object sender, EventArgs e)
        {
            if (this.Visible && list1.Visible)
                list1.RefreshList();
        }


        void rightSoft_Click(object sender, EventArgs e)
        {
            if (this.Visible && venueDetails1.Visible)
                ChangeView(0);
        }
        #endregion

        private void list1_ItemSelected(object sender, EventArgs e)
        {
            ChangeView(1);
            venueDetails1.OpenVenue(list1.SelectedVenue);
        }

        private void ChangeView(int index)
        {
            switch (index)
            {
                case 0:
                    ResetMenus();
                    leftSoft.Text = "&Refresh";
                    leftSoft.Enabled = true;
                    rightSoft.Text = "&Create";
                    rightSoft.Enabled = true;

                    list1.Activate();
                    venueDetails1.Visible = false;
                    break;
                case 1:

                    ResetMenus();
                    venueDetails1.ActivateControl(leftSoft, rightSoft);
                    list1.Visible = false;
                    break;
            }
        }

        private void ResetMenus()
        {
            leftSoft.MenuItems.Clear();
            rightSoft.MenuItems.Clear();
        }
    }
}
