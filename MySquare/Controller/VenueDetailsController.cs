using System;
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
            View.checkIn1.txtShout.Text = string.Empty;
            View.venueTips1.Enabled = true;
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
            Tips
        }

        void OpenSection(VenueSection section)
        {
            View.tabStrip1.Enabled = true;
            View.checkIn1.pnlShout.Enabled = true;


            RightSoftButtonText = "&Back";
            RightSoftButtonEnabled = true;

            LeftSoftButtonText = "&Check In";
            switch (section)
            {
                case VenueSection.CheckIn:
                    View.venueInfo1.Visible = false;
                    View.venueMap1.Visible = false;
                    View.venueTips1.Visible = false;

                    View.checkIn1.Activate();


                    LeftSoftButtonEnabled = View.checkIn1.pnlShout.Visible;
                    break;
                case VenueSection.Info:
                    View.checkIn1.Visible = false;
                    View.venueMap1.Visible = false;
                    View.venueTips1.Visible = false;
                    View.venueInfo1.Activate();

                    LoadExtraInfo();

                    LeftSoftButtonEnabled = false;
                    break;
                case VenueSection.Map:
                    if (Configuration.IsPremium)
                    {
                        View.venueInfo1.Visible = false;
                        View.checkIn1.Visible = false;
                        View.venueTips1.Visible = false;
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
                    View.venueInfo1.Visible = false;
                    View.venueMap1.Visible = false;
                    View.checkIn1.Visible = false;
                    View.venueTips1.Activate();

                    LoadExtraInfo();
                    LeftSoftButtonText = "&Comment";
                    LeftSoftButtonEnabled = true;
                    break;
            }
            Cursor.Current = Cursors.Default;
        }

        internal void OpenVenue(Venue venue)
        {
            this.Venue = venue;
            this.SaveNavigation(venue);
            FillAddress();
            if (venue.Tags != null && venue.Tags.Length > 0)
                lastTags = venue.Tags;

            View.tabStrip1.SelectedIndex = 0;
            OpenSection(VenueSection.CheckIn);
        }


        private void FillAddress()
        {
            View.lblVenueName.Text = Venue.Name;
            List<string> address = new List<string>();
            if (!string.IsNullOrEmpty(Venue.Address))
                address.Add(Venue.Address);
            if (!string.IsNullOrEmpty(Venue.City))
                address.Add(Venue.City);
            if (!string.IsNullOrEmpty(Venue.State))
                address.Add(Venue.State);
            View.lblAddress.Text = string.Join(", ", address.ToArray()).Replace("&", "&&");
        }

        public override void OnLeftSoftButtonClick()
        {
            if (View.checkIn1.Visible)
                DoCheckIn();
            else if (View.venueTips1.Visible)
                Comment();
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
            if (e.Exception is UnauthorizedAccessException || (waitingCheckIn && !(e.Exception is RequestAbortException)))
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
                catch (InvalidOperationException) { }
                Log.RegisterLog(e.Exception);
            }
        }

        #region CheckIn
        bool waitingCheckIn;
        MySquare.FourSquare.CheckIn checkInResult = null;

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
            Service.CheckIn(Venue,
                View.checkIn1.txtShout.Text,
                View.checkIn1.chkTellFriends.Checked, 
                facebook, twitter);

            RightSoftButtonText = "&Cancel";
                
        }

        void Service_CheckInResult(object serder, MySquare.FourSquare.CheckInEventArgs e)
        {
            waitingCheckIn = false;
            checkInResult = e.CheckIn;
            CheckInResult();
        }

        private void CheckInResult()
        {
            if (View.InvokeRequired)
            {
                View.Invoke(new ThreadStart(CheckInResult));
                return;
            }
            if (checkInResult != null)
            {

                View.tabStrip1.Enabled = true;
                View.checkIn1.pnlShout.Visible = false;
                View.checkIn1.pnlCheckInResult.Visible = true;

                View.checkIn1.message = checkInResult.Message;
                if (checkInResult.Mayor != null && checkInResult.Mayor.Type != MayorType.nochange)
                    View.checkIn1.mayorship = checkInResult.Mayor.Message;
                else
                    View.checkIn1.mayorship = null;

                if (checkInResult.Badges != null && checkInResult.Badges.Length > 0)
                    View.checkIn1.badges = checkInResult.Badges;
                else
                    View.checkIn1.badges = null;

                if (checkInResult.Scoring != null && checkInResult.Scoring.Length > 0)
                    View.checkIn1.scoring = checkInResult.Scoring;
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
                View.checkIn1.badgeImageList = new Dictionary<string, Image>();
                if (checkInResult.Badges != null)
                    foreach (var badge in checkInResult.Badges)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(badge.ImageUrl))
                                using (MemoryStream mem = new MemoryStream(Service.DownloadImageSync(badge.ImageUrl)))
                                {
                                    Bitmap bmp = new Bitmap(mem);
                                    View.checkIn1.badgeImageList.Add(badge.ImageUrl, bmp);
                                    View.checkIn1.pnlCheckInResult.Invoke(new ThreadStart(delegate() { View.checkIn1.pnlCheckInResult.Invalidate(); }));
                                }
                        }
                        catch { }
                    }
                View.checkIn1.scoreImageList = new Dictionary<string, Image>();
                if (checkInResult.Scoring != null)
                    foreach (var score in checkInResult.Scoring)
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
                        catch { }
                    }
            }));
            t.Start();
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


                if (!string.IsNullOrEmpty(Venue.Phone))
                {
                    View.venueInfo1.lblPhone.Text = Venue.Phone;
                    View.venueInfo1.lblPhone.Enabled = true;
                }
                else
                {
                    View.venueInfo1.lblPhone.Text = "Not available";
                    View.venueInfo1.lblPhone.Enabled = false;
                }

                if (Venue.Status != null && Venue.Status.Mayor != null)
                {
                    View.venueInfo1.lblMayor.Text =
                        string.Concat(Venue.Status.Mayor.User.FirstName, " ", Venue.Status.Mayor.User.LastName);
                    View.venueInfo1.lblMayor.Enabled = true;
                    View.venueInfo1.lblMayor.Tag = Venue.Status.Mayor.User;
                }
                else
                {
                    View.venueInfo1.lblMayor.Text = "Not available";
                    View.venueInfo1.lblMayor.Enabled = false;
                }
                if (Venue.Status != null && (Venue.Status.HereNow > 0 || Venue.Status.CheckIns > 0))
                    View.venueInfo1.lblStats.Text =
                        string.Concat(Venue.Status.HereNow, " here, ", Venue.Status.CheckIns, " check-ins.");
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

                Thread t = new Thread(new ThreadStart(delegate()
                {
                    if (Venue.PrimaryCategory != null && !string.IsNullOrEmpty(Venue.PrimaryCategory.IconUrl))
                    {
                        byte[] image = Service.DownloadImageSync(Venue.PrimaryCategory.IconUrl);
                        View.Invoke(new ThreadStart(delegate()
                        {
                            View.venueInfo1.imgCategory.Tag = image;
                            View.venueInfo1.imgCategory.Invalidate();
                        }));
                    }

                    if (Venue.Status != null && Venue.Status.Mayor != null && !string.IsNullOrEmpty(Venue.Status.Mayor.User.ImageUrl))
                    {
                        byte[] image = Service.DownloadImageSync(Venue.Status.Mayor.User.ImageUrl);
                        View.Invoke(new ThreadStart(delegate()
                        {
                            View.venueInfo1.imgMayor.Tag = image;
                            View.venueInfo1.imgMayor.Invalidate();
                        }));
                    }

                }));
                t.Start();
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
            ((Main)this.View.Parent).inputPanel.Enabled = false;

            PictureBox box = this.View.venueMap1.picMap;
            if (box.Tag == null)
            {
                Size size = box.Size;
                box.Tag = "downloading";

                Thread t = new Thread(new ThreadStart(delegate()
                {
                    CultureInfo culture = CultureInfo.GetCultureInfo("en-us");
                    string googleMapsUrl = string.Format(BaseController.googleMapsUrl,
                        size.Width, size.Height,
                        Venue.Latitude.ToString(culture),
                        Venue.Longitude.ToString(culture));

                    byte[] buffer = Service.DownloadImageSync(googleMapsUrl, false);

                    this.View.Invoke(new ThreadStart(delegate()
                    {
                        box.Image = null;
                        box.Tag = buffer;
                        box.Invalidate();
                    }));
                }));
                t.Start();
            }
        }

        #endregion

        #region Tips
        private void LoadTips()
        {
            View.venueTips1.listBox.Clear();
            if (Venue.Tips != null)
            {
                foreach (Tip tip in Venue.Tips)
                {
                    View.venueTips1.listBox.AddItem(null, tip, View.venueTips1.MeasureHeight(tip));
                }
                View.venueTips1.lblError.Visible = false;
            }
            else
                View.venueTips1.lblError.Visible = true;

            Thread t = new Thread(new ThreadStart(delegate()
              {
                  if (Venue.Tips != null)
                  {
                      foreach (Tip tip in Venue.Tips)
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
              }));
            t.Start();

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
            if (View.InvokeRequired)
                View.Invoke(new ThreadStart(AddTipResult));
            else
                AddTipResult();

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

    }
}

