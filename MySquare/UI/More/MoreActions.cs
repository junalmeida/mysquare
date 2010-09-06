using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MySquare.Properties;
using Tenor.Mobile.Drawing;
using MySquare.Service;
using MySquare.Controller;

namespace MySquare.UI.More
{
    partial class MoreActions : UserControl, IView
    {
        public MoreActions()
        {
            InitializeComponent();

            listBox.AddItem("Shout\r\nBroadcast a message to your friends.", new AlphaImage(Resources.Shout));
            listBox.AddItem("The Leaderboard\r\nSee the weekly ranking.", new AlphaImage(Resources.Leaderboard));
            listBox.AddItem("Check for updates\r\nDo you have the latest version?", new AlphaImage(Resources.Update));
            listBox.AddItem("About\r\nCurrent version information.", new AlphaImage(Resources.Help));
        }

        SizeF factor;
        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this.factor = factor;
            itemPadding = Convert.ToInt32(3 * factor.Height);
            base.ScaleControl(factor, specified);
        }

        Font smallFont;
        StringFormat format;
        SolidBrush textBrush;
        SolidBrush secondBrush;

        int itemPadding;
        private void listBox_DrawItem(object sender, Tenor.Mobile.UI.DrawItemEventArgs e)
        {
            if (smallFont == null)
                smallFont = new Font(this.Font.Name, this.Font.Size - 1, this.Font.Style);
            if (format == null)
                format = new StringFormat();
            if (textBrush == null)
                textBrush = new SolidBrush(Color.White);
            if (secondBrush == null)
                secondBrush = new SolidBrush(Color.LightGray);



            Font thisFont = listBox.Font;
            float thisLeft = e.Bounds.Height;


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
            //if (!e.Item.Selected)
            //    e.Graphics.FillRectangle(new SolidBrush(color), e.Bounds);
            if (e.Item.YIndex > 0)
                e.Graphics.DrawSeparator(e.Bounds, color);

            e.Graphics.DrawString(text, thisFont, textBrush, rect, format);
            if (secondText != null)
            {
                measuring = e.Graphics.MeasureString(secondText, smallFont);
                rect = new RectangleF(rect.X, rect.Bottom + itemPadding, measuring.Width, measuring.Height);
                e.Graphics.DrawString(secondText, smallFont, secondBrush, rect, format);
            }

            AlphaImage alpha = e.Item.Value as AlphaImage;
            if (alpha != null)
            {
                try
                {
                    int imageSize = e.Bounds.Height - Convert.ToInt32(itemPadding * 2);
                    alpha.Draw(e.Graphics,
                              new Rectangle(
                                  e.Bounds.X + Convert.ToInt32(itemPadding),
                                  e.Bounds.Y + Convert.ToInt32(itemPadding),
                                  imageSize,
                                  imageSize));


                }
                catch (Exception ex)
                {
                    Log.RegisterLog("gdi", ex);
                }
            }

        }



        private void listBox_SelectedItemChanged(object sender, EventArgs e)
        {
        }

    }
}
