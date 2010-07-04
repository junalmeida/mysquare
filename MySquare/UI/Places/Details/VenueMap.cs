using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MySquare.UI.Places.Details
{
    internal partial class VenueMap : UserControl
    {
        public VenueMap()
        {
            InitializeComponent();
        }

        internal void Activate()
        {
            Dock = DockStyle.Fill;
            BringToFront();
            Visible = true;
        }

        SizeF factor;
        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this.factor = factor;
            base.ScaleControl(factor, specified);
        }

        private void picMap_Paint(object sender, PaintEventArgs e)
        {
            Image image = picMap.Tag as Image;
            if (image != null)
            {
                TextureBrush brush = new TextureBrush(image);
                Tenor.Mobile.Drawing.RoundedRectangle.Fill(
                    e.Graphics, new Pen(Color.White), brush, new Rectangle(0, 0, picMap.Width, picMap.Height),
                    new SizeF(8 * factor.Width, 8 * factor.Height).ToSize());
            }
        }
    }
}
