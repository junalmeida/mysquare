using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySquare.Service;
using Tenor.Mobile.Drawing;

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
            if (picMap.Tag != null && picMap.Tag is byte[])
            {
                picMap.Tag = new AlphaImage(Main.CreateRoundedAvatar((byte[])picMap.Tag, picMap.Size, factor));
            }
            if (picMap.Tag != null && picMap.Tag is AlphaImage)
            {
                AlphaImage image = (AlphaImage)picMap.Tag;
                try
                {
                    image.Draw(e.Graphics, new Rectangle(0, 0, image.Width, image.Height));
                }
                catch (Exception ex) { Log.RegisterLog(ex); picMap.Tag = null; }
            }
        }
    }
}
