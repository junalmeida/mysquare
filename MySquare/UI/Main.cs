using System;
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
using MySquare.Service;
using System.IO;
using System.Diagnostics;

namespace MySquare.UI
{
    internal partial class Main : Form, IView
    {
        public static Image CreateRoundedAvatar(byte[] original, int imageSize, SizeF scaleFactor)
        {
            return CreateRoundedAvatar(original, new Size(imageSize, imageSize), scaleFactor);
        }

        public static Image CreateRoundedAvatar(byte[] original, Size imageSize, SizeF scaleFactor)
        {
            using (MemoryStream mem = new MemoryStream(original))
            using (Bitmap bmp = new Bitmap(mem))
            {
                return CreateRoundedAvatar(bmp, imageSize, scaleFactor);
            }
        }

        public static Image CreateRoundedAvatar(Image original, int imageSize, SizeF scaleFactor)
        {
            return CreateRoundedAvatar(original, new Size(imageSize, imageSize), scaleFactor);
        }

        public static Image CreateRoundedAvatar(Image original, Size imageSize, SizeF scaleFactor)
        {
            using (Bitmap bmp1 = new Bitmap(imageSize.Width, imageSize.Height))
            {
                using (Graphics g = Graphics.FromImage(bmp1))
                    g.DrawImage(original, new Rectangle(0, 0, bmp1.Width, bmp1.Height), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);

                Bitmap bmp2 = new Bitmap(imageSize.Width, imageSize.Height);
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
            header.Tabs.Add(tabTips = new Tenor.Mobile.UI.HeaderTab("Tips Nearby", Resources.To_Do));
            header.Tabs.Add(tabFriends = new Tenor.Mobile.UI.HeaderTab("Friends", Resources.Friends));
            header.Tabs.Add(tabSettings = new Tenor.Mobile.UI.HeaderTab("Settings", Resources.Settings));
            header.Tabs.Add(tabMore = new Tenor.Mobile.UI.HeaderTab("Extra", Resources.MoreActions));
        }
        HeaderTab tabFriends;
        HeaderTab tabPlaces;
        HeaderTab tabTips;
        HeaderTab tabSettings;
        HeaderTab tabMore;
 
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
        internal int lastTab = 0;
        private void header_SelectedTabChanged(object sender, EventArgs e)
        {
            lastTab = header.SelectedIndex;
            Cursor.Current = Cursors.Default;
            Controller.BaseController.Navigation.Clear();
            lblError.Visible = false;
            var tab = header.Tabs[header.SelectedIndex];
            if (tab == tabPlaces)
                Controller.BaseController.OpenController(places1);
            if (tab == tabTips)
                Controller.BaseController.OpenController(places1);
            else if (tab == tabSettings)
                Controller.BaseController.OpenController(settings1);
            else if (tab == tabFriends)
                Controller.BaseController.OpenController(friends1);
            else if (tab == tabMore)
                Controller.BaseController.OpenController(moreActions1);
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
            Program.ResetGps();

            AdjustInputPanel();

            if (timerGps == null)
            {
                timerGps = new Timer();
                timerGps.Tick += new EventHandler(timerGps_Tick);
                timerGps.Interval = 2000;
            }
            timerGps.Enabled = true;

#if DEBUG
            timerAds.Enabled = true;
            return;
#endif
            if (MySquare.Service.Configuration.IsFirstTime)
                timerTutorial.Enabled = true;
            else
                timerAds.Enabled = true;
        }


        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            Program.TurnGpsOff();

            if (timerGps != null)
            {
                timerGps.Enabled = false;
            }
        }


        private void AdjustInputPanel()
        {
            if (inputPanel.Enabled)
            {
                picAd.Height = inputPanel.Bounds.Height;
                picAd.Visible = true;
            }
            else
                picAd.Visible = false;
        }



        internal void Reset()
        {
            foreach (Control c in Controls)
                if (c is MySquare.UI.IView)
                    c.Visible = false;
            help1.Visible = false;
        }


        #region Help Tutorial

        internal List<string> helpText;
        List<Point> helpWidget;
        internal int current = -1;

        private void LoadHelp()
        {
            helpText = new List<string>();
            helpWidget = new List<Point>();


            helpText.Add("Welcome to MySquare!\r\nBrought to you by Rising Mobility. Here you can see more information.");
            helpWidget.Add(
                new Point(180 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width,
                    30 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height));

            helpText.Add("To start, first tap here to set your foursquare account.");
            helpWidget.Add(
                new Point(140 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width,
                    30 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height));

            helpText.Add("Here you will see nearby places to check details and check in.");
            helpWidget.Add(
                new Point(35 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width,
                    30 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height));

            helpText.Add("See nearby tips tapping this tab. You can see what'up around you.");
            helpWidget.Add(
                new Point(55 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width,
                    30 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height));


            helpText.Add("The GPS icon will get solid when you have a precise location.");
            helpWidget.Add(
                new Point(this.Width - (17 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width),
                    47 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height));

            helpText.Add("Here you will see your friends and friend requests. Enjoy!");
            helpWidget.Add(
                new Point(95 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width,
                    30 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height));


            current = -1;
        }

        private void timerTutorial_Tick(object sender, EventArgs e)
        {
            timerTutorial.Enabled = false;
#if !TESTING
            if (helpText == null)
                LoadHelp();
            NextHelp();
#endif
        }

        private void contextHelp1_Click(object sender, EventArgs e)
        {
            timerTutorial.Interval = 300;
            timerTutorial.Enabled = true;
        }

        private void NextHelp()
        {
            current++;
            if (current < helpText.Count)
            {
                contextHelp1.ShowHelp(helpWidget[current], helpText[current]);
            }
            else
                timerAds.Enabled = true;
        }

        #endregion

        #region AdSense
        private void timerAds_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!inputPanel.Enabled && !picAd.Visible && picAd.Tag != null && Configuration.ShowAds)
                {
                    picAd.Height = 1;
                    picAd.Visible = true;

                    int finalHeight = 30 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height;
                    double i = 1;
                    do
                    {
                        if (i > finalHeight)
                        {
                            picAd.Height = finalHeight;
                            break;
                        }
                        picAd.Height = Convert.ToInt32(i);
                        Application.DoEvents();
                        i *= (Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height + .7);
                    } while (true);

                }
            }
            catch (ObjectDisposedException) { }
        }

        private void picAd_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, picAd.Width, 5 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height);
            Tenor.Mobile.Drawing.GradientFill.Fill(
                e.Graphics, rect,
                Color.DarkGray, Color.Black, GradientFill.FillDirection.TopToBottom);

            rect = new Rectangle(
                0, rect.Height, picAd.Width, picAd.Height - rect.Height);
            e.Graphics.FillRectangle(
                new SolidBrush(Color.Black),
                rect);

            if (picAd.Tag != null)
            {
                AdEventArgs ad = picAd.Tag as AdEventArgs;
                if (ad != null && lnkTextLink.Text != ad.Text)
                {
                    if (ad.Text != null)
                    {
                        lnkTextLink.Text = ad.Text.Replace("&", "&&");
                        lnkTextLink.Visible = true;
                    }
                    else
                    {
                        lnkTextLink.Visible = false;
                    }
                }
            }
        }
                        
                        

        private void lnkTextLink_Click(object sender, EventArgs e)
        {
            if (picAd.Tag != null)
            {
                AdEventArgs ad = picAd.Tag as AdEventArgs;
                if (ad != null)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(ad.Link, string.Empty));
                    }
                    catch { }
                }

            }
        }
        #endregion

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Application.DoEvents();
            Size size = new Size(26 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width, 11 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height);
            Point point = new Point(this.Width - size.Width, header.Height - size.Height - (2 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height));
            picGps.Location = point;
            picGps.Size = size;
            if (backBuffer != null)
                backBuffer.Dispose();
            if (backBufferG != null)
                backBufferG.Dispose();
            backBuffer = new Bitmap(size.Width, size.Height);
            backBufferG = Graphics.FromImage(backBuffer);
        }

        bool showGps = false;
        AlphaImage gps;
        Timer timerGps = null;
        Bitmap backBuffer;
        Graphics backBufferG;
        private void picGps_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (backBufferG == null)
                    return;
                e.Graphics.DrawImage(backBuffer, 0, 0);
                backBufferG.Clear(Color.Black);
                Rectangle size;

                size= new Rectangle(0, 0, picGps.Height, picGps.Height);

                if (showGps)
                {
                    if (gps == null)
                        gps = new AlphaImage(Resources.Gps);

                    gps.Draw(backBufferG, size);
                }

                size = new Rectangle(
                    size.Width, 0,
                    14 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width,
                    11 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height);
                if (Program.Location.IsGpsOpen)
                {
                    Debug.Write(Program.Location.Sattelites.ToString());
                    int strength = 0;
                    if (Program.Location.Sattelites.Total > 0)
                    {
                        float active = Program.Location.Sattelites.Active;
                        float total = Program.Location.Sattelites.Total;
                        if (active > 0 && active <= 2)
                            active = 1;
                        else
                            active -= 2;
                        if (total > 4)
                            total = 4;

                        float strengthPercent =
                            (active / total) * 100F;

                        strength = Convert.ToInt32(Math.Floor(4F * strengthPercent / 100F));
                        if (strength > 4)
                            strength = 4;

                        Debug.WriteLine(": " + strengthPercent.ToString() + ": " + strength.ToString());
                    }
                    Image image = null;
                    switch (strength)
                    {
                        case 1:
                            image = Resources.Gps_1;
                            break;
                        case 2:
                            image = Resources.Gps_2;
                            break;
                        case 3:
                            image = Resources.Gps_3;
                            break;
                        case 4:
                            image = Resources.Gps_4;
                            break;
                        default:
                            image = Resources.Gps_0;
                            break;
                    }

                    backBufferG.DrawImage(image,
                        size,
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);
                }

                e.Graphics.DrawImage(backBuffer, 0, 0);
            }
            catch (Exception ex)
            {
                Log.RegisterLog("gdi", ex);
            }
        }

        void timerGps_Tick(object sender, EventArgs e)
        {
            if (Program.Location.FixType == RisingMobility.Mobile.Location.FixType.Gps)
                showGps = true;
            else if (Program.Location.IsGpsOpen)
                showGps = !showGps;
            else
                showGps = false;
            picGps.Invalidate();
        }

    }
}