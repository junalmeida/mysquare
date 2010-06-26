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
            this.leftSoft.Text = "&Refresh";

            if (this.rightSoft != rightSoft)
            {
                this.rightSoft = rightSoft;
            }
            this.rightSoft.Text = "&Menu";
            this.rightSoft.MenuItems.Clear();
            this.rightSoft.MenuItems.Add(new MenuItem()
            {
                Text = "&Create Venue"
            });


            list1.Refresh();

        }

        void leftSoft_Click(object sender, EventArgs e)
        {
            if (this.Visible)
                list1.Refresh();
        }

        #endregion

    }
}
