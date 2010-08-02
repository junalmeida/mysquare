using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.UI;
using MySquare.UI.Places.Create;
using MySquare.UI.Places;
using Tenor.Mobile.Location;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using MySquare.FourSquare;
using MySquare.Service;
using System.Drawing;
using Tenor.Mobile.Drawing;

namespace MySquare.Controller
{
    class CreateVenueController : BaseController<CreateVenue>
    {
        Service.Google google;

        public CreateVenueController(CreateVenue view)
            : base(view)
        {
            Service.VenueResult += new VenueEventHandler(Service_VenueResult);
            Service.Error += new MySquare.Service.ErrorEventHandler(Service_Error);
            google = new Google();
            google.GeocodeResult += new GeocodeEventHandler(Service_GeocodeResult);
            google.Error += new MySquare.Service.ErrorEventHandler(google_Error);
            View.picMap.Click += new EventHandler(picMap_Click);
            View.picMap.Resize += new EventHandler(picMap_Resize);
        }

        void Service_Error(object serder, MySquare.Service.ErrorEventArgs e)
        {
            WaitThread.Set();
        }




        WorldPosition pos;
        public override void Activate()
        {
            UI.Main form = (Main)View.Parent;
            form.ChangePlacesName("Create Place");
            
            form.Reset();
            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.BringToFront();
            View.Visible = true;

            LeftSoftButtonText = "&Create";
            RightSoftButtonText = "&Back";
            LeftSoftButtonEnabled = true;
            RightSoftButtonEnabled = true;

            View.txtAddress.Text = string.Empty;
            View.txtCity.Text = string.Empty;
            View.txtCross.Text = string.Empty;
            View.txtName.Text = string.Empty;
            View.txtPhone.Text = string.Empty;
            View.txtState.Text = string.Empty;
            View.txtZip.Text = string.Empty;
            View.FixType = null;
            View.picMap.Tag = null;

#if DEBUG
            if (Environment.OSVersion.Platform != PlatformID.WinCE)
            {
                DownloadMapPosition();
                return;
            }
#endif
            if (pos != null)
                pos.Dispose();
            pos = new WorldPosition(true, true);
            pos.LocationChanged += new EventHandler(pos_LocationChanged);
            pos.Error += new Tenor.Mobile.Location.ErrorEventHandler(pos_Error);
        }




        public override void Deactivate()
        {
            if (pos != null)
                pos.LocationChanged -= new EventHandler(pos_LocationChanged);
            pos = null;
            View.Visible = false;
        }

        void google_Error(object serder, MySquare.Service.ErrorEventArgs e)
        {
            if (Log.RegisterLog(e.Exception))
            {
                ShowError("Cannot connect with Google service.");
                pos.Dispose();
                pos = null;
            }
        }


        void pos_Error(object sender, Tenor.Mobile.Location.ErrorEventArgs e)
        {
            if (Log.RegisterLog(e.Error))
            {
                ShowError("Cannot get position from network.");
                pos.Dispose();
                pos = null;
            }

        }

        void pos_LocationChanged(object sender, EventArgs e)
        {
            if (pos != null)
            {
                pos.PollingInterval = 30000;
                DownloadMapPosition();
            }
        }

        Size oldSize = Size.Empty;
        void picMap_Resize(object sender, EventArgs e)
        {
            Size current = ((Control)sender).Size;
            if (pos != null && pos.Latitude.HasValue && pos.Longitude.HasValue && !Size.Equals(current, oldSize))
                DownloadMapPosition();
            oldSize = current;
        }


        public override void OnLeftSoftButtonClick()
        {
            DoCreate();
        }

        public override void OnRightSoftButtonClick()
        {
            OpenController(View.Parent as IView);
        }


        #region Maps

        Thread t;
        void DownloadMapPosition()
        {
            PictureBox box = this.View.picMap;

            Size size = new Size();
            box.Invoke(new ThreadStart(delegate()
            {
                size = box.Size;
            }));

            if (t != null)
                t.Abort();
            t = new Thread(new ThreadStart(delegate()
            {
                CultureInfo culture = CultureInfo.GetCultureInfo("en-us");
                double latitude, longitude;
#if DEBUG
                if (Environment.OSVersion.Platform != PlatformID.WinCE)
                {
                    latitude = -22.856025;
                    longitude = -43.375182;
                }
                else
                {
                    latitude = pos.Latitude.Value;
                    longitude = pos.Longitude.Value;
                }
#else
            latitude = pos.Latitude.Value;
            longitude = pos.Longitude.Value;
#endif
                View.latitudeCenter = latitude;
                View.longitudeCenter = longitude;
                View.latitudeSelected = null;
                View.longitudeSelected = null;
                View.selectedPoint = Point.Empty;

                string googleMapsUrl = string.Format(BaseController.googleMapsUrl,
                    size.Width, size.Height,
                    latitude.ToString(culture),
                    longitude.ToString(culture));
                byte[] buffer = Service.DownloadImageSync(googleMapsUrl, false);

                this.View.Invoke(new ThreadStart(delegate()
                {
                    View.FixType = pos != null && pos.FixType == FixType.Network ? "Low precision" : "High precision";
                    box.Image = null;
                    if (box.Tag != null && box.Tag is IDisposable)
                        ((IDisposable)box.Tag).Dispose();
                    box.Tag = buffer;
                    box.Invalidate();

                }));
            }));
            t.Start();
        }


        void picMap_Click(object sender, EventArgs e)
        {
            double? latSel = View.latitudeSelected;
            double? lngSel = View.longitudeSelected;
            if (latSel.HasValue && lngSel.HasValue)
                google.GetGeocoding(latSel.Value, lngSel.Value);
            else
                google.GetGeocoding(pos.Latitude.Value, pos.Longitude.Value);
        }

        void Service_GeocodeResult(object serder, GeocodeEventArgs e)
        {
            View.Invoke(new ThreadStart(delegate()
            {
                if (View.Visible)
                {
                    string number = string.Empty;
                    foreach (Geocode geocode in e.Geocodes)
                    {
                        if (Array.IndexOf(geocode.Types, "street_address") > -1 ||
                            Array.IndexOf(geocode.Types, "route") > -1)
                        {
                            foreach (AddressComponent addr in geocode.AddressComponents)
                            {
                                if (Array.IndexOf(addr.Types, "route") > -1)
                                    View.txtAddress.Text = addr.LongName;
                                if (Array.IndexOf(addr.Types, "street_number") > -1)
                                    number = addr.LongName;
                                if (Array.IndexOf(addr.Types, "locality") > -1)
                                    View.txtCity.Text = addr.LongName;
                                if (Array.IndexOf(addr.Types, "administrative_area_level_1") > -1)
                                    View.txtState.Text = addr.ShortName;
                                if (Array.IndexOf(addr.Types, "postal_code") > -1)
                                    View.txtZip.Text = addr.ShortName;
                            }
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(View.txtAddress.Text) && !string.IsNullOrEmpty(number))
                    {
                        int i = number.IndexOf("-");
                        if (i > -1)
                            number = number.Substring(0, i);
                        View.txtAddress.Text += ", " + number;
                    }
                }
            }));
        }
        #endregion

        #region CreateVenue
        Venue createdVenue = null;
        private void DoCreate()
        {
            if (string.IsNullOrEmpty(View.txtName.Text))
            {
                MessageBox.Show("Type in the name of this place.", "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
            Cursor.Current = Cursors.WaitCursor;


            double lat = 0, lng = 0;
            double? latSel = View.latitudeSelected;
            double? longSel = View.longitudeSelected;
            if (latSel.HasValue && longSel.HasValue)
            {
                lat = latSel.Value;
                lng = longSel.Value;
            }
            else if (pos != null && pos.Latitude.HasValue)
            {
                lat = pos.Latitude.Value;
                lng = pos.Longitude.Value;
            }

            createdVenue = null;
            Service.AddVenue(
                View.txtName.Text, View.txtAddress.Text, View.txtCross.Text,
                View.txtCity.Text, View.txtState.Text, View.txtZip.Text,
                View.txtPhone.Text, lat, lng, null);
            WaitThread.Reset();
            WaitThread.WaitOne();
            Cursor.Current = Cursors.Default;

            if (createdVenue != null)
            {
                MessageBox.Show(string.Format("{0} was created.", View.txtName.Text), "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                (OpenController(((Main)View.Parent).venueDetails1) as VenueDetailsController).OpenVenue(createdVenue);
                createdVenue = null;
            }
            else
                MessageBox.Show(string.Format("{0} was not created.", View.txtName.Text), "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

        }


        void Service_VenueResult(object serder, VenueEventArgs e)
        {
            createdVenue = e.Venue;
            WaitThread.Set();
        }
        #endregion

        public override void Dispose()
        {
            if (google != null)
                google.Dispose();
            if (pos != null)
                pos.Dispose();

            if (t != null)
                t.Abort();

            base.Dispose();
        }
    }
}
