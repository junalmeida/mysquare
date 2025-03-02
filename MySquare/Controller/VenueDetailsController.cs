﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.FourSquare;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using MySquare.UI.Places;
using MySquare.UI;
using MySquare.Service;
using System.Drawing;
using System.IO;

namespace MySquare.Controller
{
    class VenueDetailsController : BaseController<VenueDetails>
    {

        public VenueDetailsController(UI.Places.VenueDetails view)
            : base(view)
        {
            this.View.TabChanged += new EventHandler(venueDetails_TabChanged);
            Service.CheckInResult += new CheckInEventHandler(Service_CheckInResult);
            Service.VenueResult += new VenueEventHandler(Service_VenueResult);
            Service.AddTipResult += new AddTipEventHandler(Service_AddTipResult);
            Service.Error += new ErrorEventHandler(Service_Error);
            Service.FlagResult += new FlagEventHandler(Service_FlagResult);
        }



        void venueDetails_TabChanged(object sender, EventArgs e)
        {

            OpenSection(View.SelectedTab);
        }

        public override void Activate()
        {

            UI.Main form = (Main)View.Parent;
            form.ChangePlacesName("Place Detail");

            form.Reset();

            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.BringToFront();
            View.Visible = true;


            View.tabStrip1.SelectedIndex = 0;

            View.venueMap1.picMap.Tag = null;

            form.header.Tabs[0].Selected = true;
            View.checkIn1.pnlCheckInResult.Visible = false;
            View.checkIn1.pnlShout.Visible = true;
            View.venueTips1.Enabled = true;

            View.checkIn1.chkTwitter.Checked = Configuration.SiteSettings != null ? Configuration.SiteSettings.SendToTwitter : false;
            View.checkIn1.chkFacebook.Checked = Configuration.SiteSettings != null ? Configuration.SiteSettings.SendToFacebook : false;

            OpenSection(VenueSection.CheckIn);

        }

        public override void Deactivate()
        {
            View.Visible = false;
        }


        internal enum VenueSection
        {
            CheckIn,
            Info, 
            Map,
            Tips,
            People
        }

        void OpenSection(VenueSection section)
        {
            View.tabStrip1.Enabled = true;
            View.checkIn1.pnlShout.Enabled = true;


            RightSoftButtonText = "&Back";
            RightSoftButtonEnabled = true;

            LeftSoftButtonText = "&Check In";

                    View.venueInfo1.Visible = false;
                    View.venueMap1.Visible = false;
                    View.venueTips1.Visible = false;
                    View.peopleHere.Visible = false;
                    View.checkIn1.Visible = false;

            switch (section)
            {
                case VenueSection.CheckIn:
                    View.checkIn1.Activate();
                    LeftSoftButtonEnabled = View.checkIn1.pnlShout.Visible;
                    break;
                case VenueSection.Info:
                    View.venueInfo1.Activate();

                    LoadExtraInfo();
                    BuildFlagMenus();
                    break;
                case VenueSection.Map:
                    if (Configuration.IsPremium)
                    {
                        View.venueMap1.Activate();

                        LeftSoftButtonEnabled = false;
                        DownloadMapPosition();
                    }
                    else
                    {
                        View.tabStrip1.SelectedIndex = 0;
                        OpenSection(VenueSection.CheckIn);
                        View.tabStrip1.Invalidate();
                        MessageBox.Show("The map tab is a premium feature. Check MySquare website.", "MySquare");
                    }
                    break;
                case VenueSection.Tips:
                    View.venueTips1.Activate();

                    LoadExtraInfo();
                    LeftSoftButtonText = "&Comment";
                    LeftSoftButtonEnabled = true;
                    break;
                case VenueSection.People:
                    View.peopleHere.Activate();

                    LoadExtraInfo();
                    LeftSoftButtonText = "&Refresh";
                    LeftSoftButtonEnabled = true;
                    break;
            }
            Cursor.Current = Cursors.Default;
        }

        #region Flags
        private void BuildFlagMenus()
        {
            LeftSoftButtonText = "&Flag";
            LeftSoftButtonEnabled = true;
            MenuItem menu;
            menu = new MenuItem()
            {
                Text = "As Closed"
            };
            menu.Click += new EventHandler(MnuAsClosed_Click);
            AddLeftSubMenu(menu);
            menu = new MenuItem()
            {
                Text = "As Duplicated"
            };
            menu.Click += new EventHandler(MnuAsDuplicated_Click);
            AddLeftSubMenu(menu);
            menu = new MenuItem()
            {
                Text = "As Mislocated"
            };
            menu.Click += new EventHandler(MnuAsMislocated_Click);
            AddLeftSubMenu(menu);
        }


        void MnuAsClosed_Click(object sender, EventArgs e)
        {
            Service.FlagAsClosed(Venue.Id);
        }

        void MnuAsDuplicated_Click(object sender, EventArgs e)
        {
            Service.FlagAsDuplicated(Venue.Id);
        }

        void MnuAsMislocated_Click(object sender, EventArgs e)
        {
            Service.FlagAsMislocated(Venue.Id);
        }

        void Service_FlagResult(object sender, FlagEventArgs e)
        {
            try
            {
                View.Invoke(new ThreadStart(delegate()
                {
                    string message = null;
                    if (e.Success)
                        message = "This place was flagged.";
                    else
                        message = "Unable to flag this place.";
                    MessageBox.Show(message, "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                }));
            }
            catch (ObjectDisposedException) { }
        }

        #endregion

        internal void OpenVenue(Venue venue)
        {
            this.Venue = venue;
            this.SaveNavigation(venue);
            FillAddress();
            if (venue.Tags != null && venue.Tags.Length > 0)
                lastTags = venue.Tags;

            View.checkIn1.txtShout.Text = string.Empty;
            View.tabStrip1.SelectedIndex = 0;
            OpenSection(VenueSection.CheckIn);
        }


        private void FillAddress()
        {
            View.lblVenueName.Text = (Venue.Name ?? "").Replace("&", "&&");
            List<string> address = new List<string>();
            if (Venue.Location != null)
            {
                if (!string.IsNullOrEmpty(Venue.Location.Address))
                    address.Add(Venue.Location.Address);
                if (!string.IsNullOrEmpty(Venue.Location.City))
                    address.Add(Venue.Location.City);
                if (!string.IsNullOrEmpty(Venue.Location.State))
                    address.Add(Venue.Location.State);
            }
            View.lblAddress.Text = string.Join(", ", address.ToArray()).Replace("&", "&&");
        }

        public override void OnLeftSoftButtonClick()
        {
            if (View.checkIn1.Visible)
                DoCheckIn();
            else if (View.venueTips1.Visible)
                Comment();
            else if (View.peopleHere.Visible)
                Refresh();
        }

        private void Refresh()
        {
            Venue.fullData = false;
            LoadExtraInfo();
        }


        public override void OnRightSoftButtonClick()
        {
            if (RightSoftButtonText == "&Cancel")
            {
                Service.Abort();
                Activate();
            }
            else
            {
                if (!base.GoBack())
                    BaseController.OpenController(View.Parent as MySquare.UI.IView);
            }
        }

        internal MySquare.FourSquare.Venue Venue
        { get; set; }



        void Service_Error(object sender, ErrorEventArgs e)
        {
            if (e.Exception is UnauthorizedAccessException || e.Exception is ServerException || (waitingCheckIn && !(e.Exception is RequestAbortException)))
            {
                ShowError(e.Exception);
                waitingCheckIn = false;
            }
            else
            {
                try
                {
                    View.Invoke(new ThreadStart(delegate()
                    {
                        View.lblAddress.Text = "unable to read data.";
                    }));
                }
                catch (ObjectDisposedException) { return; }
                catch (InvalidOperationException) { }
                Log.RegisterLog(e.Exception);
            }
        }

        #region CheckIn
        bool waitingCheckIn;
        MySquare.FourSquare.CheckInEventArgs checkInResult = null;

        internal void DoCheckIn()
        {
            waitingCheckIn = true;
            View.checkIn1.pnlShout.Enabled = false;
            View.tabStrip1.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();

            bool? twitter = null;
            bool? facebook = null;
            if (View.checkIn1.chkTwitter.CheckState != CheckState.Indeterminate)
                twitter = View.checkIn1.chkTwitter.CheckState == CheckState.Checked;
            if (View.checkIn1.chkFacebook.CheckState != CheckState.Indeterminate)
                facebook = View.checkIn1.chkFacebook.CheckState == CheckState.Checked;

            checkInResult = null;
            WaitThread.Reset();
            if (!Program.Location.WorldPoint.IsEmpty && Program.Location.FixType == RisingMobility.Mobile.Location.FixType.Gps)
            {
                Service.CheckIn(Venue,
                    View.checkIn1.txtShout.Text,
                    View.checkIn1.chkTellFriends.Checked,
                    facebook, twitter, 
                        Program.Location.WorldPoint.Latitude,
                        Program.Location.WorldPoint.Longitude, 
                        Program.Location.WorldPoint.Altitude, 
                        Program.Location.WorldPoint.HorizontalDistance);
            }
            else
            {
                Service.CheckIn(Venue,
                    View.checkIn1.txtShout.Text,
                    View.checkIn1.chkTellFriends.Checked,
                    facebook, twitter);
            }
            RightSoftButtonText = "&Cancel";
                
        }

        void Service_CheckInResult(object serder, MySquare.FourSquare.CheckInEventArgs e)
        {
            waitingCheckIn = false;
            checkInResult = e;
            CheckInResult();
        }

        private void CheckInResult()
        {
            try
            {
                if (View.InvokeRequired)
                {
                    View.Invoke(new ThreadStart(CheckInResult));
                    return;
                }
            }
            catch (ObjectDisposedException) { return;  }
            if (checkInResult != null)
            {

                View.tabStrip1.Enabled = true;
                View.checkIn1.pnlShout.Visible = false;
                View.checkIn1.pnlCheckInResult.Visible = true;

                View.checkIn1.message = checkInResult.Message;
                if (checkInResult.Mayorship != null)
                {
                    View.checkIn1.mayorship = checkInResult.Mayorship.Message;
                    View.checkIn1.showCrown = checkInResult.Mayorship.Type != MayorType.nochange;
                }
                else
                {
                    View.checkIn1.mayorship = null;
                    View.checkIn1.showCrown = false;
                }

                if (checkInResult.Badges != null && checkInResult.Badges.Length > 0)
                    View.checkIn1.badges = checkInResult.Badges;
                else
                    View.checkIn1.badges = null;

                if (checkInResult.Score != null && checkInResult.Score.Length > 0)
                    View.checkIn1.scoring = checkInResult.Score;
                else
                    View.checkIn1.scoring = null;

                if (checkInResult.Specials != null && checkInResult.Specials.Length > 0)
                    View.checkIn1.specials = checkInResult.Specials;
                else
                    View.checkIn1.specials = null;
            }

            OpenSection(VenueSection.CheckIn);

            Thread t = new Thread(new ThreadStart(delegate()
            {
                View.checkIn1.badgeImageList.ClearImageList();
                View.checkIn1.badgeImageList = new Dictionary<string, Bitmap>();
                if (checkInResult.Badges != null)
                    foreach (var badge in checkInResult.Badges)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(badge.ImageUrl.ToString()))
                                using (MemoryStream mem = new MemoryStream(Service.DownloadImageSync(badge.ImageUrl.ToString())))
                                {
                                    Bitmap bmp = new Bitmap(mem);
                                    View.checkIn1.badgeImageList.Add(badge.ImageUrl.ToString(), bmp);
                                    View.checkIn1.pnlCheckInResult.Invoke(new ThreadStart(delegate() { View.checkIn1.pnlCheckInResult.Invalidate(); }));
                                }
                        }
                        catch (ObjectDisposedException) { return; }
                        catch { }
                    }
                View.checkIn1.scoreImageList.ClearImageList();
                View.checkIn1.scoreImageList = new Dictionary<string, Bitmap>();
                if (checkInResult.Score != null)
                    foreach (var score in checkInResult.Score)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(score.ImageUrl))
                                using (MemoryStream mem = new MemoryStream(Service.DownloadImageSync(score.ImageUrl)))
                                {
                                    Bitmap bmp = new Bitmap(mem);
                                    View.checkIn1.scoreImageList.Add(score.ImageUrl, bmp);
                                    View.checkIn1.pnlCheckInResult.Invoke(new ThreadStart(delegate() { View.checkIn1.pnlCheckInResult.Invalidate(); }));
                                }
                        }
                        catch (ObjectDisposedException) { return; }
                        catch { }
                    }
            }));
            t.StartThread();
        }


        #endregion


        #region ExtraInfo

        private void LoadExtraInfo()
        {
            if (View.InvokeRequired)
            {
                try
                {
                    View.Invoke(new ThreadStart(this.LoadExtraInfo));
                }
                catch (ObjectDisposedException) { }
            }
            else
            {

                if (!Venue.fullData)
                {
                    View.lblAddress.Text = "reading place details...";
                    Service.GetVenue(Venue.Id);
                }
                else
                    FillAddress();

                if (Venue.PrimaryCategory != null)
                    View.venueInfo1.lblCategory.Text = Venue.PrimaryCategory.FullName.Replace(":", " > ").Replace("&", "&&");
                else
                    View.venueInfo1.lblCategory.Text = null;


                if (Venue.Contact != null && !string.IsNullOrEmpty(Venue.Contact.Phone))
                {
                    View.venueInfo1.lblPhone.Text = Venue.Contact.Phone;
                    View.venueInfo1.lblPhone.Enabled = true;
                }
                else
                {
                    View.venueInfo1.lblPhone.Text = "Not available";
                    View.venueInfo1.lblPhone.Enabled = false;
                }

                if (Venue.Status != null && Venue.Mayor != null && Venue.Mayor.User != null)
                {
                    View.venueInfo1.lblMayor.Text = Venue.Mayor.User.ToString();
                    View.venueInfo1.lblMayor.Enabled = true;
                    View.venueInfo1.lblMayor.Tag = Venue.Mayor.User;
                }
                else
                {
                    View.venueInfo1.lblMayor.Text = "Not available";
                    View.venueInfo1.lblMayor.Enabled = false;
                }
                if (Venue.Status != null && (Venue.Status.CheckIns > 0))
                    View.venueInfo1.lblStats.Text =
                        string.Concat(Venue.Status.CheckIns, " check-ins.");
                else
                    View.venueInfo1.lblStats.Text = null;

                View.venueInfo1.imgCategory.Tag = null;
                View.venueInfo1.imgMayor.Tag = null;
                StringBuilder txtSpecials = new StringBuilder();
                if (Venue.Specials != null)
                    foreach (var sp in Venue.Specials.Where(s => s.Kind == SpecialKind.here))
                    {
                        if (txtSpecials.Length > 0)
                        {
                            txtSpecials.AppendLine(); txtSpecials.AppendLine();
                        }
                        txtSpecials.Append(sp.Message);
                    }
                View.venueInfo1.lblSpecials.Text = txtSpecials.ToString();

                LoadTips();
                LoadPeople();

                Thread t = new Thread(new ThreadStart(delegate()
                {
                    try
                    {
                        if (Venue.PrimaryCategory != null && !string.IsNullOrEmpty(Venue.PrimaryCategory.IconUrl.ToString()))
                        {
                            byte[] image = Service.DownloadImageSync(Venue.PrimaryCategory.IconUrl.ToString());
                            View.Invoke(new ThreadStart(delegate()
                            {
                                View.venueInfo1.imgCategory.Tag = image;
                                View.venueInfo1.imgCategory.Invalidate();
                            }));
                        }

                        if (Venue.Status != null && Venue.Mayor != null && Venue.Mayor.User != null && !string.IsNullOrEmpty(Venue.Mayor.User.ImageUrl))
                        {
                            byte[] image = Service.DownloadImageSync(Venue.Mayor.User.ImageUrl);
                            View.Invoke(new ThreadStart(delegate()
                            {
                                View.venueInfo1.imgMayor.Tag = image;
                                View.venueInfo1.imgMayor.Invalidate();
                            }));
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        return;
                    }

                }));
                t.StartThread();
            }
        }


    
        void Service_VenueResult(object sender, VenueEventArgs e)
        {
            this.Venue = e.Venue;

            LoadExtraInfo();
        }
        #endregion


        #region Maps

        void DownloadMapPosition()
        {
            int zoom = 16;
            if (Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height < 2)
                zoom = 15;
            
            ((Main)this.View.Parent).inputPanel.Enabled = false;

            PictureBox box = this.View.venueMap1.picMap;
            if (box.Tag == null)
            {
                Size size = box.Size;
                box.Tag = "downloading";

                Thread t = new Thread(new ThreadStart(delegate()
                {
                    try
                    {
                        CultureInfo culture = CultureInfo.GetCultureInfo("en-us");
                        string googleMapsUrl = string.Format(BaseController.googleMapsUrl,
                            size.Width, size.Height,
                            Venue.Location.Latitude.ToString(culture),
                            Venue.Location.Longitude.ToString(culture), zoom, Configuration.MapType.ToString().ToLower());

                        byte[] buffer = Service.DownloadImageSync(googleMapsUrl, false);

                        this.View.Invoke(new ThreadStart(delegate()
                        {
                            box.Image = null;
                            box.Tag = buffer;
                            box.Invalidate();
                        }));
                    }
                    catch (ObjectDisposedException)
                    {
                        return;
                    }
                }));
                t.StartThread();
            }
        }

        #endregion

        #region Tips
        private void LoadTips()
        {
            View.venueTips1.listBox.Clear();
            View.venueTips1.imageList.ClearImageList();
            View.venueTips1.imageList = new Dictionary<string, Bitmap>();
            if (Venue.TipGroups != null)
            {
                foreach (var group in Venue.TipGroups)
                    foreach (Tip tip in group)
                    {
                        View.venueTips1.listBox.AddItem(null, tip, View.venueTips1.MeasureHeight(tip));
                    }
                View.venueTips1.lblError.Visible = false;
            }
            else
                View.venueTips1.lblError.Visible = true;

            Thread t = new Thread(new ThreadStart(delegate()
            {
                try
                {
                    if (Venue.TipGroups != null)
                    {
                        foreach (var group in Venue.TipGroups)
                            foreach (Tip tip in group)
                            {
                                if (tip.User != null)
                                {
                                    byte[] buffer = Service.DownloadImageSync(tip.User.ImageUrl);
                                    if (buffer != null)
                                    {
                                        using (MemoryStream mem = new MemoryStream(buffer))
                                        {
                                            Bitmap image =
                                                new Bitmap(mem);
                                            View.venueTips1.imageList[tip.User.ImageUrl] = image;
                                        }
                                        View.Invoke(new ThreadStart(delegate()
                                        {
                                            View.venueTips1.listBox.Invalidate();
                                        }));
                                    }
                                    buffer = null;
                                }
                            }
                    }
                }
                catch (ObjectDisposedException) { }
            }));
            t.StartThread();

        }

        private Tip tipResult;
        private void Comment()
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            View.venueTips1.Enabled = false;
            View.tabStrip1.Enabled = false;
            LeftSoftButtonEnabled = false;

            string text = View.venueTips1.txtComment.Text;

            tipResult = null;
            Service.AddTip(Venue.Id, text);

            RightSoftButtonText = "&Cancel";
        }


        void Service_AddTipResult(object serder, TipEventArgs e)
        {
            tipResult = e.Tip;
            try
            {
                if (View.InvokeRequired)
                    View.Invoke(new ThreadStart(AddTipResult));
                else
                    AddTipResult();
            }
            catch (ObjectDisposedException) { }

        }

        void AddTipResult()
        {
            Cursor.Current = Cursors.Default;
            Cursor.Show();
            if (tipResult != null)
            {
                View.venueTips1.txtComment.Text = string.Empty;
                View.venueTips1.Enabled = true;
                View.tabStrip1.Enabled = true;
                LeftSoftButtonEnabled = true;

                Venue.fullData = false;
                LoadExtraInfo();
                tipResult = null;
            }
        }

        #endregion

        #region People
        private void LoadPeople()
        {
            try
            {
                if (View.InvokeRequired)
                {
                    View.Invoke(new ThreadStart(LoadPeople));
                    return;
                }
            }
            catch (ObjectDisposedException) { return; }
            if (!View.peopleHere.Visible)
                return;
            View.peopleHere.lblError.Visible = false;
            View.peopleHere.ImageList = new Dictionary<string, byte[]>();

            if (Venue != null && Venue.HereNow != null && Venue.HereNow.Count > 0)
            {
                foreach (var chkin in Venue.HereNow)
                {
                    chkin.User.CheckIn = chkin;

                    View.peopleHere.listBox.AddItem(null, chkin.User);
                }


                Thread t = new Thread(new ThreadStart(delegate()
                {
                    foreach (var chkin in Venue.HereNow)
                    {
                        User u = chkin.User;
                        if (!string.IsNullOrEmpty(u.ImageUrl))
                        {
                            try
                            {

                                byte[] image = Service.DownloadImageSync(u.ImageUrl);

                                if (!View.peopleHere.ImageList.ContainsKey(u.ImageUrl))
                                {
                                    View.peopleHere.ImageList.Add(u.ImageUrl, image);
                                    View.Invoke(new ThreadStart(delegate() { View.peopleHere.listBox.Invalidate(); }));
                                }
                            }
                            catch (ObjectDisposedException) { return; }
                            catch { }
                        }
                    }
                }));
                t.StartThread();
            }
            else if (Venue.fullData)
            {
                View.peopleHere.lblError.Visible = true;
            }
        }

        #endregion

    }
}

