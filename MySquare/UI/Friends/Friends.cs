﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MySquare.FourSquare;
using Tenor.Mobile.Drawing;
using MySquare.Controller;

namespace MySquare.UI.Friends
{
    internal partial class Friends : UserControl, IView
    {
        public Friends()
        {
            InitializeComponent();
        }

        private void listBox_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        SizeF factor; float itemPadding;
        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this.factor = factor;
            itemPadding = 3 * factor.Height;
            base.ScaleControl(factor, specified);
        }


        Dictionary<string, Image> imageList;
        Dictionary<string, Image> brushList;
        internal Dictionary<string, Image> ImageList
        {
            get { return imageList; }
            set { imageList = value; brushList = new Dictionary<string, Image>(); }
        }

        Brush brush = new SolidBrush(Tenor.Mobile.UI.Skin.Current.TextForeColor);
        Brush brushS = new SolidBrush(Tenor.Mobile.UI.Skin.Current.TextHighLight);
        Brush secondBrush = new SolidBrush(Color.LightGray);
        StringFormat format = new StringFormat()
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near,
            FormatFlags = StringFormatFlags.NoWrap
        };
        Font secondFont = null;
        private void listBox_DrawItem(object sender, Tenor.Mobile.UI.DrawItemEventArgs e)
        {
            string text = null;
            string secondText = null;
            string userUrl = null;

            if (e.Item.Value == null)
            {
                text = e.Item.Text;
                Color color = Tenor.Mobile.UI.Skin.Current.ControlBackColor;
                if (e.Item.YIndex % 2 == 0)
                    color = Tenor.Mobile.UI.Skin.Current.AlternateBackColor;
                e.Graphics.FillRectangle(new SolidBrush(color), e.Bounds);

                if (e.Item.YIndex > 0)
                {
                    Rectangle rect2 = new Rectangle(
                       e.Bounds.X, e.Bounds.Y, e.Bounds.Width / 3, 1);
                    Tenor.Mobile.Drawing.GradientFill.Fill(e.Graphics, rect2, color, Color.LightGray, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
                    rect2.X += rect2.Width;
                    Tenor.Mobile.Drawing.GradientFill.Fill(e.Graphics, rect2, Color.LightGray, Color.LightGray, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
                    rect2.X += rect2.Width;
                    Tenor.Mobile.Drawing.GradientFill.Fill(e.Graphics, rect2, Color.LightGray, color, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);

                }
            }
            else if (e.Item.Value != null && e.Item.Value is CheckIn)
            {
                CheckIn checkIn = (CheckIn)e.Item.Value;

                text = checkIn.Display;

                if (checkIn.User != null)
                    userUrl = checkIn.User.ImageUrl;

                TimeSpan time = DateTime.Now - checkIn.Created;
                if (time.TotalMinutes < 60)
                    secondText = string.Format("{0} minutes ago.", time.Minutes);
                else if (time.TotalHours < 24)
                    secondText = string.Format("{0} hours ago.", time.Hours);
                else if (time.TotalDays < 5)
                    secondText = string.Format("{0} days ago.", time.Days);
                else
                    secondText = checkIn.Created.ToShortDateString() + "  " + checkIn.Created.ToShortTimeString();

            }
            else if (e.Item.Value != null && e.Item.Value is User)
            {
                User user = (User)e.Item.Value;
                text = string.Format("{0} {1}", user.FirstName, user.LastName);
                secondText = user.Email;
                userUrl = user.ImageUrl;
            }


            Brush textBrush = (e.Item.Selected ? brushS : brush);
            SizeF measuring = e.Graphics.MeasureString(text, Font);

            RectangleF rect = new RectangleF(e.Bounds.Height, e.Bounds.Y + itemPadding, measuring.Width, measuring.Height);
            e.Graphics.DrawString(text, this.Font, textBrush, rect, format);

            if (!string.IsNullOrEmpty(secondText))
            {
                measuring = e.Graphics.MeasureString(secondText, Font);
                rect = new RectangleF(rect.X, rect.Bottom + itemPadding, measuring.Width, measuring.Height);
                if (secondFont == null)
                    secondFont = new Font(Font.Name, Font.Size - 1, Font.Style);
                e.Graphics.DrawString(secondText, secondFont, secondBrush, rect, format);
            }

            if (userUrl != null && imageList.ContainsKey(userUrl))
            {
                int imageSize = e.Bounds.Height - Convert.ToInt32(itemPadding * 2);

                if (!brushList.ContainsKey(userUrl))
                {
                    Image original = imageList[userUrl];
                    Image bmp = Main.CreateRoundedAvatar(original, imageSize, factor);
                    brushList.Add(userUrl, bmp);
                }

                try
                {
                    AlphaImage alpha = new AlphaImage(brushList[userUrl]);
                    alpha.Draw(e.Graphics,
                                new Rectangle(
                                    e.Bounds.X + Convert.ToInt32(itemPadding),
                                    e.Bounds.Y + Convert.ToInt32(itemPadding),
                                    imageSize,
                                    imageSize), Tenor.Mobile.UI.Skin.Current.SelectedBackColor);

                }
                catch { }
            }
        }


        private void listBox_SelectedItemClicked(object sender, EventArgs e)
        {
            BaseController.OpenController(((Main)Parent).userDetail1);
        }
    }
}
