﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySquare.Properties;
using MySquare.Controller;
using Tenor.Mobile.UI;
using Tenor.Mobile.Drawing;

namespace MySquare.UI
{
    internal partial class Main : Form, IView
    {
        public static Image CreateRoundedAvatar(Image original, int imageSize, SizeF scaleFactor)
        {
            using (Bitmap bmp1 = new Bitmap(imageSize, imageSize))
            {
                using (Graphics g = Graphics.FromImage(bmp1))
                    g.DrawImage(original, new Rectangle(0, 0, bmp1.Width, bmp1.Height), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);

                Bitmap bmp2 = new Bitmap(imageSize, imageSize);
                using (TextureBrush textureBrush = new TextureBrush(bmp1))
                using (Graphics g = Graphics.FromImage(bmp2))
                {
                    g.Clear(Tenor.Mobile.UI.Skin.Current.SelectedBackColor);
                    RoundedRectangle.Fill(
                       g, new Pen(Color.DarkGray), textureBrush,
                        new Rectangle(
                            0,
                            0,
                            bmp2.Width,
                            bmp2.Height)
                        , new SizeF(8 * scaleFactor.Width, 8 * scaleFactor.Height).ToSize()
                    );
                }

                return bmp2;
            }
        }




        public Main()
        {
            InitializeComponent();

            this.Icon = Resources.mySquare;
            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);

            header.Tabs.Add(tabPlaces = new Tenor.Mobile.UI.HeaderTab("Places", Resources.PinMap));
            header.Tabs.Add(tabFriends = new Tenor.Mobile.UI.HeaderTab("Friends", Resources.Friends));
            header.Tabs.Add(tabSettings = new Tenor.Mobile.UI.HeaderTab("Settings", Resources.Settings));
            header.Tabs.Add(tabAbout = new Tenor.Mobile.UI.HeaderTab("About", Resources.Help));
        }
        HeaderTab tabFriends;
        HeaderTab tabPlaces;
        HeaderTab tabSettings;
        HeaderTab tabAbout;

        internal void ChangePlacesName(string text)
        {
            if (string.IsNullOrEmpty(text))
                tabPlaces.Text = "Places";
            else
                tabPlaces.Text = text;
            header.Invalidate();
        }

        internal void ChangeFriendsName(string text)
        {
            if (string.IsNullOrEmpty(text))
                tabFriends.Text = "Friends";
            else
                tabFriends.Text = text;
            header.Invalidate();
        }

        private void header_SelectedTabChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;

            lblError.Visible = false;
            var tab = header.Tabs[header.SelectedIndex];
            if (tab == tabPlaces)
                Controller.BaseController.OpenController(places1);
            else if (tab == tabSettings)
                Controller.BaseController.OpenController(settings1);
            else if (tab == tabFriends)
                Controller.BaseController.OpenController(friends1);
            else if (tab == tabAbout)
                Controller.BaseController.OpenController(help1);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            mnuLeft.Text = string.Empty;
            mnuRight.Text = string.Empty;

            Application.DoEvents();
            BaseController.OpenController(this);
        }



        private void inputPanel_EnabledChanged(object sender, EventArgs e)
        {
            AdjustInputPanel();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            AdjustInputPanel();
        }

        private void AdjustInputPanel()
        {
            if (inputPanel.Enabled)
            {
                picAd.Height = inputPanel.Bounds.Height;
                picAd.Visible = true;
            }
            else
            {
                picAd.Height = 30;
                picAd.Visible = false;
            }
        }



        internal void Reset()
        {
            foreach (Control c in Controls)
                if (c is MySquare.UI.IView)
                    c.Visible = false;
            help1.Visible = false;
        }
    }
}