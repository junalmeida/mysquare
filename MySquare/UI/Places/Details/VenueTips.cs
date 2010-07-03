using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySquare.FourSquare;
using System.IO;

namespace MySquare.UI.Places.Details
{
    public partial class VenueTips : UserControl
    {
        public VenueTips()
        {
            InitializeComponent();
            listBox.BackColor = this.BackColor;
            lblError.BackColor = this.BackColor;

            font = new Font(Font.Name, 7, FontStyle.Regular);
            fontBold = new Font(Font.Name, 7, FontStyle.Bold);
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
            base.ScaleControl(factor, specified);
            this.factor = factor;
        }

        StringFormat format = new StringFormat()
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near
        };
        SolidBrush brush = new SolidBrush(Color.Black);
        Font font;
        Font fontBold;

        internal Dictionary<string, Tenor.Mobile.Drawing.AlphaImage> imageList = new Dictionary<string, Tenor.Mobile.Drawing.AlphaImage>();

        internal int MeasureHeight(Tip tip)
        {
#if DEBUG
            if (Environment.OSVersion.Platform != PlatformID.WinCE)
                return listBox.DefaultItemHeight + 10;
#endif
            using (Graphics g = this.listBox.CreateGraphics())
            {
                Rectangle rect = new Rectangle
                        (0,
                         0,
                         listBox.Width - listBox.DefaultItemHeight,
                         listBox.Height);
                Size size1 = Tenor.Mobile.Drawing.Strings.Measure(g, tip.User.FirstName, fontBold, rect);
                Size size2 = Tenor.Mobile.Drawing.Strings.Measure(g, tip.Text, font, rect);

                int size = size1.Height + size2.Height;
                if (size < listBox.DefaultItemHeight)
                    size = listBox.DefaultItemHeight;
                return size;
            }
        }

        SolidBrush selectedBrush = new SolidBrush(Color.PaleGoldenrod);
        Pen borderPen = new Pen(Color.White);
        private void listBox_DrawItem(object sender, Tenor.Mobile.UI.DrawItemEventArgs e)
        {
            Tip tip = (Tip)e.Item.Value;


            if (!string.IsNullOrEmpty(tip.Text))
            {
                float padding = 5 * factor.Width;
                int imageSize = Convert.ToInt32(listBox.DefaultItemHeight - padding);

                RectangleF rect = new RectangleF
                    (e.Bounds.X + imageSize + (padding * 2),
                     e.Bounds.Y,
                     e.Bounds.Width - (imageSize) - (padding * 2),
                     e.Bounds.Height);

                if (e.Item.Selected)
                {
                    Tenor.Mobile.Drawing.RoundedRectangle.Fill(e.Graphics,
                        borderPen, selectedBrush, e.Bounds, new SizeF(8 * factor.Width, 8 * factor.Height).ToSize());
                }

                if (imageList != null && imageList.ContainsKey(tip.User.ImageUrl))
                {
                    Tenor.Mobile.Drawing.AlphaImage image = imageList[tip.User.ImageUrl];

                    Rectangle imgRect =
                        new Rectangle(0 + Convert.ToInt32(padding),
                           Convert.ToInt32(rect.Y + (rect.Height / 2) - (imageSize / 2)), imageSize, imageSize);
                    try
                    {
                        image.Draw(e.Graphics, imgRect);
                    }
                    catch { }
                }

                e.Graphics.DrawString(
                    tip.User.ToString(), fontBold, brush, rect);

                SizeF size = e.Graphics.MeasureString(tip.User.ToString(), fontBold);
                rect.Y += size.Height;
                e.Graphics.DrawString(
                    tip.Text, font, brush, rect, format);

                if (!e.Item.Selected)
                {
                    Rectangle rect2 = new Rectangle(
                                   e.Bounds.X, e.Bounds.Bottom - 2, e.Bounds.Width / 3, 1);
                    Tenor.Mobile.Drawing.GradientFill.Fill(e.Graphics, rect2, this.BackColor, Color.Gray, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
                    rect2.X += rect2.Width;
                    Tenor.Mobile.Drawing.GradientFill.Fill(e.Graphics, rect2, Color.Gray, Color.Gray, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
                    rect2.X += rect2.Width;
                    Tenor.Mobile.Drawing.GradientFill.Fill(e.Graphics, rect2, Color.Gray, this.BackColor, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
                }
            }

        }


        private void listBox_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void listBox_SelectedItemClicked(object sender, EventArgs e)
        {

        }
 


    }
}
