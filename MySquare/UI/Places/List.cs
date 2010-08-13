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
using Tenor.Mobile.Drawing;
using MySquare.Service;

namespace MySquare.UI.Places
{
    internal partial class List : UserControl
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


        Dictionary<string, byte[]> imageList;
        Dictionary<string, AlphaImage> imageListBuffer;
        internal Dictionary<string, byte[]> ImageList
        {
            get { return imageList; }
            set
            {
                imageList = value;
                Program.ClearImageList(imageListBuffer);
                imageListBuffer = new Dictionary<string, AlphaImage>();
            }
        }

        float itemPadding;
        Font smallFont;
        private void listBox_DrawItem(object sender, Tenor.Mobile.UI.DrawItemEventArgs e)
        {

            Venue venue = (Venue)e.Item.Value;
            Brush textBrush = (e.Item.Selected ? brushS : brush);
            if (venue == null)
            {
                if (smallFont == null)
                    smallFont = new Font(this.Font.Name, this.Font.Size - 1, this.Font.Style);

                Font thisFont;
                float thisLeft;


                if (e.Item.YIndex < listBox.Count - 1)
                {
                    thisLeft = itemPadding;
                    thisFont = smallFont;
                }
                else
                {
                    thisLeft = e.Bounds.Height;
                    thisFont = Font;
                }

                string text = e.Item.Text;
                string secondText = null;

                if (text.IndexOf("\r\n") > -1)
                {
                    string[] textS = text.Split('\r', '\n');
                    text = textS[0];
                    secondText = textS[2];
                }

                SizeF measuring = e.Graphics.MeasureString(text, thisFont);
                RectangleF rect = new RectangleF(thisLeft, e.Bounds.Y + itemPadding, measuring.Width, measuring.Height);

                Color color = Tenor.Mobile.UI.Skin.Current.ControlBackColor;
                if (e.Item.YIndex % 2 == 0)
                    color = Tenor.Mobile.UI.Skin.Current.AlternateBackColor;
                if (e.Item.YIndex < listBox.Count - 1)
                    e.Graphics.FillRectangle(new SolidBrush(color), e.Bounds);
                if (e.Item.YIndex > 0)
                    Program.DrawSeparator(e.Graphics, e.Bounds, color);

                e.Graphics.DrawString(text, thisFont, textBrush, rect, format);
                if (secondText != null)
                {
                    if (secondFont == null)
                        secondFont = new Font(Font.Name, Font.Size - 1, Font.Style);
                    measuring = e.Graphics.MeasureString(secondText, secondFont);
                    rect = new RectangleF(rect.X, rect.Bottom + itemPadding, measuring.Width, measuring.Height);
                    e.Graphics.DrawString(secondText, secondFont, secondBrush, rect, format);
                }
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
                if (secondText != null)
                {
                    if (secondFont == null)
                        secondFont = new Font(Font.Name, Font.Size - 1, Font.Style);
                    measuring = e.Graphics.MeasureString(secondText, secondFont);
                    rect = new RectangleF(rect.X, rect.Bottom + itemPadding, measuring.Width, measuring.Height);
                    e.Graphics.DrawString(secondText, secondFont, secondBrush, rect, format);
                }
                if (
                    (venue.PrimaryCategory == null && imageList.ContainsKey(string.Empty)) ||
                    (venue.PrimaryCategory != null &&
                    imageList.ContainsKey(venue.PrimaryCategory.IconUrl))
                    )
                {
                    string iconUrl = string.Empty;
                    if (venue.PrimaryCategory != null)
                        iconUrl = venue.PrimaryCategory.IconUrl;

                    int imageSize = e.Bounds.Height - Convert.ToInt32(itemPadding * 2);

                    if (!imageListBuffer.ContainsKey(iconUrl))
                    {
                        imageListBuffer.Add(iconUrl, new AlphaImage(Main.CreateRoundedAvatar(imageList[iconUrl], imageSize, factor)));
                    }
                    AlphaImage alpha = imageListBuffer[iconUrl];
                    try
                    {
                        alpha.Draw(e.Graphics,
                                  new Rectangle(
                                      e.Bounds.X + Convert.ToInt32(itemPadding),
                                      e.Bounds.Y + Convert.ToInt32(itemPadding),
                                      imageSize,
                                      imageSize), Tenor.Mobile.UI.Skin.Current.SelectedBackColor);


                    }
                    catch (Exception ex)
                    {
                        imageListBuffer.Remove(iconUrl);
                        Log.RegisterLog("gdi", ex);
                    }
                }

                if (venue.Specials != null && venue.Specials.Length > 0)
                {
                    try
                    {
                        int padd = 10 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height;
                        AlphaImage alpha = new AlphaImage(Resources.SpecialHere);
                        Rectangle rectS = new Rectangle(e.Bounds.Right - e.Bounds.Height - padd, e.Bounds.Y, e.Bounds.Height + padd, e.Bounds.Height + padd);
                        rectS.Y = rectS.Y - (padd / 2);

                        alpha.Draw(e.Graphics, rectS);
                    }
                    catch (Exception ex)
                    {
                        Log.RegisterLog("gdi", ex);
                    }
                }

            }
           
        }

        
        ~List()
        {
            if (imageList != null)
                imageList = null;
            if (imageListBuffer != null)
            {
                foreach (var key in imageListBuffer.Keys)
                    if (imageListBuffer[key] != null)
                        imageListBuffer[key].Dispose();
                imageListBuffer = null;
            }
        }

        private void listBox_SelectedItemChanged(object sender, EventArgs e)
        {
        }

        private void listBox_SelectedItemClicked(object sender, EventArgs e)
        {
            if (SelectedVenue == null && listBox.SelectedItem.YIndex < listBox.Count - 1)
                return;

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
  
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                if (Search != null)
                    Search(this, e);
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
                e.Handled = true;
        }

        internal event EventHandler Search;

    }
}
