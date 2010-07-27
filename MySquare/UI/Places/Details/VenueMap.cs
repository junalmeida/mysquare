using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySquare.Service;
using Tenor.Mobile.Drawing;
using MySquare.Properties;

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
        Size ellipse;
        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this.factor = factor;
            ellipse = new SizeF(8 * factor.Width, 8 * factor.Height).ToSize();
            base.ScaleControl(factor, specified);
        }

        private void picMap_Paint(object sender, PaintEventArgs e)
        {
            if (picMap.Tag != null && picMap.Tag is byte[])
            {
                try
                {
                    picMap.Tag = new AlphaImage(Main.CreateRoundedAvatar((byte[])picMap.Tag, picMap.Size, factor));
                }
                catch (Exception)
                {
                    GC.Collect();
                }
            }
            if (picMap.Tag != null && picMap.Tag is AlphaImage)
            {
                AlphaImage image = (AlphaImage)picMap.Tag;
                Rectangle picMapRect = new Rectangle(0, 0, image.Width, image.Height);
                try
                {
                    image.Draw(e.Graphics, picMapRect);
                }
                catch (Exception ex) { Log.RegisterLog(ex); picMap.Tag = null; }
            }
            else if (!ellipse.IsEmpty)
            {
                Rectangle picMapRect = new Rectangle(0, 0, picMap.Width, picMap.Height);
                TextureBrush brush = new TextureBrush(Resources.MapBg);
                Tenor.Mobile.Drawing.RoundedRectangle.Fill(e.Graphics, new Pen(Color.Gray), brush, picMapRect, ellipse);
            }
        }
    }
}
