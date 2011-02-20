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
using RisingMobility.Mobile.Location;
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
        StringFormat formatWrap = new StringFormat()
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near,
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
                imageListBuffer.ClearImageList();
                imageListBuffer = new Dictionary<string, AlphaImage>();
            }
        }

        Geolocation address;
        internal Geolocation Address
        {
            get { return address; }
            set
            {
                address = value;
                try
                {
                    this.Invoke(new ThreadStart(this.listBox.Invalidate));
                }
                catch (ObjectDisposedException) { }
            }
        }

        float itemPadding;
        Font smallFont;
        private void listBox_DrawItem(object sender, Tenor.Mobile.UI.DrawItemEventArgs e)
        {
            object obj = e.Item.Value;

            Venue venue = e.Item.Value as Venue;
            Tip tip = e.Item.Value as Tip;
            if (tip != null)
                venue = tip.Venue;  


            Brush textBrush = (e.Item.Selected ? brushS : brush);
            if (obj == null)
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
                if (text.EndsWith("Nearby") && Address != null)
                {
                    StringBuilder geo = new StringBuilder();
                    if (!string.IsNullOrEmpty(Address.Neighborhood))
                    {
                        geo.Append(", ");
                        geo.Append(Address.Neighborhood);
                    }
                    if (!string.IsNullOrEmpty(Address.City))
                    {
                        geo.Append(", ");
                        geo.Append(Address.City);
                    }
                    if (string.IsNullOrEmpty(Address.Neighborhood) && string.IsNullOrEmpty(Address.City))
                    {
                        if (!string.IsNullOrEmpty(Address.Province))
                        {
                            geo.Append(", ");
                            geo.Append(Address.Province);
                        }
                        if (!string.IsNullOrEmpty(Address.Country))
                        {
                            geo.Append(", ");
                            geo.Append(Address.Country);
                        }
                    }

                    if (geo.Length > 0)
                    {
                        geo = geo.Remove(0, 2);
                        text += " " + geo.ToString();
                    }
                }


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
                    e.Graphics.DrawSeparator(e.Bounds, color);

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

                string title = null;
                if (tip != null)
                    title = tip.User.ToString() + " @ " + venue.Name;
                else
                    title = venue.Name;

                SizeF measuring = e.Graphics.MeasureString(title, Font);

                RectangleF rect = new RectangleF(listBox.DefaultItemHeight, e.Bounds.Y + itemPadding, measuring.Width, measuring.Height);
                if (tip != null)
                    rect.X = e.Bounds.X + itemPadding;
                e.Graphics.DrawString(title, this.Font, textBrush, rect, format);

                string secondText = null;
                if (tip != null && !string.IsNullOrEmpty(tip.Text))
                    secondText = tip.Text;
                else if (venue.Location != null)
                {
                    if (!string.IsNullOrEmpty(venue.Location.Address))
                        secondText = venue.Location.Address;
                    else if (!string.IsNullOrEmpty(venue.Location.City))
                        secondText = venue.Location.City;
                    else if (!string.IsNullOrEmpty(venue.Location.State))
                        secondText = venue.Location.State;
                }
                if (secondText != null)
                {
                    if (secondFont == null)
                        secondFont = new Font(Font.Name, Font.Size - 1, Font.Style);
                    measuring = e.Graphics.MeasureString(secondText, secondFont);
                    rect = new RectangleF(rect.X, rect.Bottom + itemPadding, e.Bounds.Width - rect.X, e.Bounds.Height);
                    e.Graphics.DrawString(secondText, secondFont, secondBrush, rect, formatWrap);
                }
                if (
                        (
                            (venue.PrimaryCategory == null && imageList.ContainsKey(string.Empty)) ||
                            (venue.PrimaryCategory != null && imageList.ContainsKey(venue.PrimaryCategory.IconUrl))
                        )
                        && tip == null
                    )
                {
                    string iconUrl = string.Empty;
                    if (venue.PrimaryCategory != null)
                        iconUrl = venue.PrimaryCategory.IconUrl;

                    int imageSize = listBox.DefaultItemHeight - Convert.ToInt32(itemPadding * 2);

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

                if (venue.Specials != null)
                {
                    foreach (Special special in venue.Specials)
                    {
                        if (special.Kind != SpecialKind.here)
                            continue;
                        try
                        {
                            int padd = 10 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height;

                            AlphaImage alpha = new AlphaImage(Resources.SpecialHere);
                            Rectangle rectS = new Rectangle(e.Bounds.Right - listBox.DefaultItemHeight - padd, e.Bounds.Y, listBox.DefaultItemHeight + padd, listBox.DefaultItemHeight + padd);
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
            if (!Configuration.DoubleTap)
                listBox_SelectedItemClicked(sender, e);
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
                {
                    Tip tip = listBox.SelectedItem.Value as Tip;
                    if (tip != null)
                        return tip.Venue;
                    else
                        return (Venue)listBox.SelectedItem.Value;
                }
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


        internal int Measure(Tip tip)
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
                Size size1 = Tenor.Mobile.Drawing.Strings.Measure(g, tip.Venue.Name, Font, rect);
                Size size2 = Tenor.Mobile.Drawing.Strings.Measure(g, tip.Text, secondFont, rect);

                int size = size1.Height * 2 + size2.Height;
                if (size < listBox.DefaultItemHeight)
                    size = listBox.DefaultItemHeight;
                return size;
            }
        }
    }
}
