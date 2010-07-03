using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Tenor.Mobile.Drawing;

namespace MySquare.UI.Places.Create
{
    public partial class CreateVenue : UserControl, IView
    {
        public CreateVenue()
        {
            InitializeComponent();
            font = new Font(this.Font.Name, 7, FontStyle.Regular);
        }

        SizeF factor;
        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this.factor = factor;
            base.ScaleControl(factor, specified);
        }

        const string helpText = "Tap to fill address";
        Font font;
        SolidBrush brush1 = new SolidBrush(Color.White);
        SolidBrush brush2 = new SolidBrush(Color.Black);

        TextureBrush map;
        string fixType;
        internal Image Map
        {
            get { return map == null ? null : map.Image; }
            set { map = new TextureBrush(value); picMap.Invalidate(); }
        }

        internal string FixType
        {
            get { return fixType; }
            set { fixType = value; picMap.Invalidate(); }
        }

        Pen penBorder = new Pen(Color.White);
        private void picMap_Paint(object sender, PaintEventArgs e)
        {
            if (map != null && !string.IsNullOrEmpty(fixType))
            {
                string text = string.Format("{0}\r\n{1}", fixType, helpText);
                try
                {
                    RoundedRectangle.Fill(
                        e.Graphics, penBorder, map,
                        new Rectangle(0, 0, picMap.Width, picMap.Height), new SizeF(8 * factor.Width, 8 * factor.Height).ToSize());

                    e.Graphics.DrawString(
                        text,
                        font, brush1, 4 * factor.Width, 4 * factor.Height);

                    e.Graphics.DrawString(
                        text,
                        font, brush2, 3 * factor.Width, 3 * factor.Height);
                }
                catch (OutOfMemoryException) { }
            }
        }

        private void CreateVenue_Resize(object sender, EventArgs e)
        {
            foreach (Control c in Controls)
            {
                if (c.Focused)
                {
                    this.AutoScrollPosition = new Point(0, c.Top - (c.Height * 2));
                    break;
                }
            }
        }

        #region Scroll Control
        int originalMouse;
        int originalScroll;
        private void CreateVenue_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originalMouse = e.Y;
                originalScroll = this.AutoScrollPosition.Y;
            }
        }

        private void CreateVenue_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.AutoScrollPosition = new Point(0, (originalMouse - e.Y) - originalScroll); 
            }
        }
        #endregion
    }
}
