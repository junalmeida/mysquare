using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.UI;
using MySquare.UI.Places.Create;
using MySquare.UI.Places;
using RisingMobility.Mobile.Location;
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

        public CreateVenueController(CreateVenue view)
            : base(view)
        {
            Service.VenueResult += new VenueEventHandler(Service_VenueResult);
            Service.Error += new MySquare.Service.ErrorEventHandler(Service_Error);
            View.picMap.Click += new EventHandler(picMap_Click);
            View.picMap.Resize += new EventHandler(picMap_Resize);
        }

        void Service_Error(object serder, MySquare.Service.ErrorEventArgs e)
        {
            WaitThread.Set();
        }


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

            Program.KeepGpsOpened = true;

            Program.Location.LocationChanged += new EventHandler(pos_LocationChanged);
            Program.Location.Error += new RisingMobility.Mobile.Location.ErrorEventHandler(pos_Error);

            Program.Location.UseGps = true;
            Program.Location.UseNetwork = true;
            if (!Program.Location.WorldPoint.IsEmpty)
                pos_LocationChanged(null, null);
            else
                Program.Location.Poll();
        }





        public override void Deactivate()
        {
            Program.Location.LocationChanged -= new EventHandler(pos_LocationChanged);
            Program.Location.Error -= new RisingMobility.Mobile.Location.ErrorEventHandler(pos_Error);
            Program.KeepGpsOpened = false;
            try
            {
                if (View.InvokeRequired)
                    View.Invoke(new ThreadStart(delegate()
                    {
                        View.Visible = false;
                    }));
                else
                    View.Visible = false;
            }
            catch (ObjectDisposedException) { }
        }

  


        void pos_Error(object sender, RisingMobility.Mobile.Location.ErrorEventArgs e)
        {
            if (Log.RegisterLog("lbs", e.Error))
            {
                Deactivate();
                ShowError("Cannot get position from network.");
            }

        }



        void pos_LocationChanged(object sender, EventArgs e)
        {

            Program.Location.PollingInterval = 30000;
            Program.Location.UseNetwork = true;
            Program.Location.UseGps = true;


            DownloadMapPosition();
        }


        Size oldSize = Size.Empty;
        void picMap_Resize(object sender, EventArgs e)
        {
            Size current = ((Control)sender).Size;
            if (!Program.Location.WorldPoint.IsEmpty && !Size.Equals(current, oldSize))
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
            int zoom = 16;
            if (Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height < 2)
                zoom = 15;

            View.zoom = zoom;

            PictureBox box = this.View.picMap;

            Size size = new Size();
            try
            {
                box.Invoke(new ThreadStart(delegate()
                {
                    size = box.Size;
                }));
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            if (t != null)
                t.Abort();
            t = new Thread(new ThreadStart(delegate()
            {
                CultureInfo culture = CultureInfo.GetCultureInfo("en-us");
                double latitude = Program.Location.WorldPoint.Latitude;
                double longitude = Program.Location.WorldPoint.Longitude;


                string googleMapsUrl = string.Format(BaseController.googleMapsUrl,
                    size.Width, size.Height,
                    latitude.ToString(culture),
                    longitude.ToString(culture),
                    zoom, Configuration.MapType.ToString().ToLower());
                byte[] buffer = Service.DownloadImageSync(googleMapsUrl, false);
                t = null;
                if (buffer != null && buffer.Length > 0)
                {
                    View.latitudeCenter = latitude;
                    View.longitudeCenter = longitude;
                    View.latitudeSelected = null;
                    View.longitudeSelected = null;
                    View.selectedPoint = Point.Empty;
                    try
                    {
                        this.View.Invoke(new ThreadStart(delegate()
                        {
                            string precision = "";
                            if (Program.Location.FixType == FixType.Gps)
                                precision = "High precision";
                            else if (Program.Location.FixType == FixType.GeoIp)
                                precision = "Low precision";
                            else if (Program.Location.FixType == FixType.GsmNetwork)
                                precision = "Average precision";

                            View.FixType = precision;

                            box.Image = null;
                            if (box.Tag != null && box.Tag is IDisposable)
                                ((IDisposable)box.Tag).Dispose();
                            box.Tag = buffer;
                            box.Invalidate();

                        }));
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                }
            }));
            t.StartThread();
        }


        void picMap_Click(object sender, EventArgs e)
        {
            double? latSel = View.latitudeSelected;
            double? lngSel = View.longitudeSelected;

            Thread t = new Thread(new ThreadStart(delegate()
            {
                try
                {
                    if (latSel.HasValue && lngSel.HasValue)
                    {
                        Geolocation geo = Geolocation.Get(latSel.Value, lngSel.Value);
                        if (geo != null)
                            Service_GeocodeResult(geo);
                    }
                }
                catch (Exception ex)
                {
                    Log.RegisterLog("geolocation", ex);
                }
            }));
            t.StartThread();
        }

        void Service_GeocodeResult(Geolocation geo)
        {
            try
            {
                View.Invoke(new ThreadStart(delegate()
                {
                    if (View.Visible)
                    {
                        string number = string.Empty;

                        View.txtAddress.Text = geo.Route;
                        number = geo.StreetNumber;

                        View.txtCity.Text = geo.City;
                        View.txtState.Text = geo.Province;
                        View.txtZip.Text = geo.ZipCode;

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
            catch (ObjectDisposedException) { }
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
            else if (!Program.Location.WorldPoint.IsEmpty)
            {
                lat = Program.Location.WorldPoint.Latitude;
                lng = Program.Location.WorldPoint.Longitude;
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


        void Service_VenueResult(object sender, VenueEventArgs e)
        {
            createdVenue = e.Venue;
            WaitThread.Set();
        }
        #endregion

        public override void Dispose()
        {
            if (t != null)
                t.Abort();

            base.Dispose();
        }
    }
}
