using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySquare.Properties;
using MySquare.FourSquare;
using System.Threading;
using Tenor.Mobile.Location;
using System.IO;

namespace MySquare.UI.Places
{
    public partial class List : UserControl
    {
        public List()
        {
            InitializeComponent();

            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);
        }

        Brush brush = new SolidBrush(Tenor.Mobile.UI.Skin.Current.TextForeColor);
        Brush brushS = new SolidBrush(Tenor.Mobile.UI.Skin.Current.TextHighLight);
        Brush secondBrush = new SolidBrush(Color.LightGray);
        Font secondFont;

        StringFormat format = new StringFormat()
        {
             Alignment = StringAlignment.Near,
             LineAlignment = StringAlignment.Near,
             FormatFlags = StringFormatFlags.NoWrap
        };

        SizeF factor;
        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this.factor = factor;
            itemPadding = 3 * factor.Height;
            base.ScaleControl(factor, specified);
        }

        internal Dictionary<string, Tenor.Mobile.Drawing.AlphaImage> imageList;

        float itemPadding;
        private void listBox_DrawItem(object sender, Tenor.Mobile.UI.DrawItemEventArgs e)
        {

            Venue venue = (Venue)e.Item.Value;
            Brush textBrush = (e.Item.Selected ? brushS : brush);
            if (venue == null)
            {
                string text = "Create a new place";
                SizeF measuring = e.Graphics.MeasureString(text, Font);
                RectangleF rect = new RectangleF(e.Bounds.Height, e.Bounds.Y + itemPadding, measuring.Width, measuring.Height);
                e.Graphics.DrawString(text, this.Font, textBrush, rect, format);
            }
            else
            {

                SizeF measuring = e.Graphics.MeasureString(venue.Name, Font);

                RectangleF rect = new RectangleF(e.Bounds.Height, e.Bounds.Y + itemPadding, measuring.Width, measuring.Height);
                e.Graphics.DrawString(venue.Name, this.Font, textBrush, rect, format);

                string secondText = null;
                if (!string.IsNullOrEmpty(venue.Address))
                    secondText = venue.Address;
                else if (!string.IsNullOrEmpty(venue.City))
                    secondText = venue.City;
                else if (!string.IsNullOrEmpty(venue.State))
                    secondText = venue.State;


                measuring = e.Graphics.MeasureString(secondText, Font);
                rect = new RectangleF(rect.X, rect.Bottom + itemPadding, measuring.Width, measuring.Height);
                if (secondFont == null)
                    secondFont = new Font(Font.Name, Font.Size - 1, Font.Style);
                e.Graphics.DrawString(secondText, secondFont, secondBrush, rect, format);

                if (
                    venue.PrimaryCategory != null &&
                    imageList.ContainsKey(venue.PrimaryCategory.IconUrl)
                    )
                {
                    Tenor.Mobile.Drawing.AlphaImage image = imageList[venue.PrimaryCategory.IconUrl];
                    try
                    {
                        image.Draw(e.Graphics,
                                    new Rectangle(
                                        e.Bounds.X + Convert.ToInt32(itemPadding),
                                        e.Bounds.Y + Convert.ToInt32(itemPadding),
                                        e.Bounds.Height - Convert.ToInt32(itemPadding * 2),
                                        e.Bounds.Height - Convert.ToInt32(itemPadding * 2)));
                    }
                    catch { }
                }
            }
        }


        private void listBox_SelectedItemChanged(object sender, EventArgs e)
        {
        }

        private void listBox_SelectedItemClicked(object sender, EventArgs e)
        {
            ItemSelected(this, new EventArgs());
        }

        public event EventHandler ItemSelected;
        internal Venue SelectedVenue
        {
            get
            {
                if (listBox.SelectedItem != null)
                    return (Venue)listBox.SelectedItem.Value;
                else
                    return null;
            }
        }

        private void pnlSearch_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect2 = new Rectangle(
               0, 0, pnlSearch.Width / 3, 1);
            Tenor.Mobile.Drawing.GradientFill.Fill(e.Graphics, rect2, this.BackColor, Color.WhiteSmoke, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
            rect2.X += rect2.Width;
            Tenor.Mobile.Drawing.GradientFill.Fill(e.Graphics, rect2, Color.WhiteSmoke, Color.WhiteSmoke, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
            rect2.X += rect2.Width;
            Tenor.Mobile.Drawing.GradientFill.Fill(e.Graphics, rect2, Color.WhiteSmoke, this.BackColor, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);

        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                
            }

        }
    }
}
