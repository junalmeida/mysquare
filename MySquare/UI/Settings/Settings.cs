using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MySquare.UI.Settings
{
    public partial class Settings : UserControl, IPanel
    {
        public Settings()
        {
            InitializeComponent();
            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);
        }

        #region IPanel Members

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
        }


        void leftSoft_Click(object sender, EventArgs e)
        {
        }


        void rightSoft_Click(object sender, EventArgs e)
        {
        }
        #endregion


        #endregion
    }
}
