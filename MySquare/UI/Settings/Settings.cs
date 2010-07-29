using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MySquare.UI.Settings
{
    internal partial class Settings : UserControl, IView
    {
        public Settings()
        {
            InitializeComponent();
            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Program.DrawSeparator(e.Graphics, new Rectangle(0, 0, pnlPremium.Width, 5), pnlPremium.BackColor);
        }
    }
}
