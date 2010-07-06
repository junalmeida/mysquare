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
            Service.ImageResult += new ImageResultEventHandler(Service_ImageResult);
            Service.AddTipResult += new AddTipEventHandler(Service_AddTipResult);
        }

        void venueDetails_TabChanged(object sender, EventArgs e)
        {
            OpenSection((VenueSection)View.tabStrip1.SelectedIndex);
        }

        public override void Activate()
        {
            UI.Places.Places places = View.Parent as UI.Places.Places;

            UI.Main form = places.Parent as UI.Main;
            form.ChangePlacesName("Place Detail");

            places.Reset();
            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.BringToFront();
            View.Visible = true;

            View.checkIn1.txtShout.Enabled = true;
            View.checkIn1.chkTellFriends.Enabled = true;

            View.checkIn1.txtShout.Text = string.Empty;
            View.checkIn1.chkTellFriends.Checked = true;


            View.tabStrip1.SelectedIndex = 0;

            View.venueMap1.picMap.Image = MySquare.Properties.Resources.HourGlass;
            View.venueMap1.picMap.Tag = null;

            OpenSection(VenueSection.CheckIn);
            if (places.list1.listBox.SelectedItem.Value != null)
                OpenVenue(places.list1.listBox.SelectedItem.Value as Venue);
        }

        internal void OpenVenue(Venue venue)
        {
            this.Venue = venue;

            View.lblVenueName.Text = venue.Name;
            List<string> address = new List<string>();
            if (!string.IsNullOrEmpty(venue.Address))
                address.Add(venue.Address);
            if (!string.IsNullOrEmpty(venue.City))
                address.Add(venue.City);
            if (!string.IsNullOrEmpty(venue.State))
                address.Add(venue.State);
            View.lblAddress.Text = string.Join(", ", address.ToArray());

            View.tabStrip1.SelectedIndex = 0;
            OpenSection(VenueSection.CheckIn);
        }

        enum VenueSection
        {
            CheckIn,
            Info, 
            Map,
            Tips
        }

        void OpenSection(VenueSection section)
        {
            View.checkIn1.txtShout.Enabled = true;
            View.checkIn1.chkTellFriends.Enabled = true;

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


                    LeftSoftButtonEnabled = true;
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
                    View.venueInfo1.Visible = false;
                    View.checkIn1.Visible = false;
                    View.venueTips1.Visible = false;
                    View.venueMap1.Activate();

                    LeftSoftButtonEnabled = false;
                    DownloadMapPosition();
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
            BaseController.OpenController(View.Parent as MySquare.UI.IView);
        }

        internal MySquare.FourSquare.Venue Venue
        { get; set; }

        #region CheckIn
        MySquare.FourSquare.CheckIn checkInResult = null;
        void Service_CheckInResult(object serder, MySquare.FourSquare.CheckInEventArgs e)
        {
            checkInResult = e.CheckIn;
            WaitThread.Set();
        }

        internal void DoCheckIn()
        {
            View.checkIn1.txtShout.Enabled = false;
            View.checkIn1.chkFacebook.Enabled = false;
            View.checkIn1.chkTwitter.Enabled = false;
            View.checkIn1.chkTellFriends.Enabled = false;
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
            WaitThread.WaitOne();

            if (checkInResult != null)
            {
                MessageBox.Show(checkInResult.Message, "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                OnRightSoftButtonClick();
            }
            else
            {
                OpenSection(VenueSection.CheckIn);
            }
                
        }
        #endregion


        #region ExtraInfo

        private void LoadExtraInfo()
        {
            if (View.InvokeRequired)
                View.Invoke(new ThreadStart(this.LoadExtraInfo));
            else
            {

                if (!Venue.fullData)
                {
                    Service.GetVenue(Venue.Id);
                }


                if (Venue.PrimaryCategory != null)
                    View.venueInfo1.lblCategory.Text = Venue.PrimaryCategory.FullName.Replace(":", " > ").Replace("&", "&&");
                else
                    View.venueInfo1.lblCategory.Text = null;

                View.venueInfo1.lblPhone.Text = Venue.Phone;

                if (Venue.Status != null && Venue.Status.Mayor != null)
                    View.venueInfo1.lblMayor.Text =
                        string.Concat(Venue.Status.Mayor.User.FirstName, " ", Venue.Status.Mayor.User.LastName);
                else
                    View.venueInfo1.lblMayor.Text = null;

                if (Venue.Status != null && (Venue.Status.HereNow > 0 || Venue.Status.CheckIns > 0))
                    View.venueInfo1.lblStats.Text =
                        string.Concat(Venue.Status.HereNow, " friends here, ", Venue.Status.CheckIns, " check-ins.");
                else
                    View.venueInfo1.lblStats.Text = null;

                View.venueInfo1.imgCategory.Tag = null;
                View.venueInfo1.imgMayor.Tag = null;

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

    
        void Service_VenueResult(object serder, VenueEventArgs e)
        {
            e.Venue.CopyTo(Venue);
            LoadExtraInfo();
        }
        #endregion


        #region Maps

        void DownloadMapPosition()
        {

            PictureBox box = this.View.venueMap1.picMap;
            if (box.Tag == null)
            {

                CultureInfo culture = CultureInfo.GetCultureInfo("en-us");
                string googleMapsUrl = string.Format(BaseController.googleMapsUrl,
                    box.Width, box.Height,
                    Venue.Latitude.ToString(culture),
                    Venue.Longitude.ToString(culture));

                Service.DownloadImage(googleMapsUrl);
            }
        }


        void Service_ImageResult(object serder, ImageEventArgs e)
        {
            if (e.Url.StartsWith("http://maps.google.com"))
            {
                PictureBox box = this.View.venueMap1.picMap;
                var image = new System.Drawing.Bitmap(new System.IO.MemoryStream(e.Image));

                box.Invoke(new ThreadStart(delegate()
                {
                    box.Image = null;
                    box.Tag = image;
                    box.Invalidate();
                }));
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
                              Tenor.Mobile.Drawing.AlphaImage image =
                                  new Tenor.Mobile.Drawing.AlphaImage(Service.DownloadImageSync(tip.User.ImageUrl));
                              View.venueTips1.imageList[tip.User.ImageUrl] = image;
                              View.Invoke(new ThreadStart(delegate()
                              {
                                  View.venueTips1.listBox.Invalidate();
                              }));
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

            string text = View.venueTips1.txtComment.Text;

            tipResult = null;
            WaitThread.Reset();
            Service.AddTip(Venue.Id, text);
            WaitThread.WaitOne();

            Cursor.Current = Cursors.Default;
            Cursor.Show();
            if (tipResult != null)
            {
                View.venueTips1.txtComment.Text = string.Empty;
                Venue.fullData = false;
                LoadExtraInfo();
                tipResult = null;
            }
            else
            {
                MessageBox.Show(checkInResult.Message, "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
            }
        }


        void Service_AddTipResult(object serder, TipEventArgs e)
        {
            tipResult = e.Tip;
            WaitThread.Set();
        }

        #endregion

    }
}

