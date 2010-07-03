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

namespace MySquare.Controller
{
    class CreateVenueController : BaseController
    {
        private CreateVenue View
        {
            get { return (CreateVenue)base.view; }
        }


        public CreateVenueController(CreateVenue view)
            : base ((IView)view)
        {
            Service.ImageResult+=new ImageResultEventHandler(Service_ImageResult);
            Service.GeocodeResult += new GeocodeEventHandler(Service_GeocodeResult);
        }


        WorldPosition pos;
        protected override void Activate()
        {
            Places parent = View.Parent as Places;

            UI.Main form = parent.Parent as UI.Main;
            form.ChangePlacesName("Create Place");
            
            parent.Reset();
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


            pos = new WorldPosition(true, true);
            pos.LocationChanged += new EventHandler(pos_LocationChanged);
            pos.Error += new Tenor.Mobile.Location.ErrorEventHandler(pos_Error);

            
        }

        protected override void Deactivate()
        {
            pos.LocationChanged -= new EventHandler(pos_LocationChanged);
            pos = null;
        }
        void pos_Error(object sender, Tenor.Mobile.Location.ErrorEventArgs e)
        {
            pos = null;
            ShowError("Cannot get position from network.");
            Service.RegisterLog(e.Error);
        }

        void pos_LocationChanged(object sender, EventArgs e)
        {
            Service.GetGeocoding(pos.Latitude.Value, pos.Longitude.Value);
            DownloadMapPosition();
        }

        protected override void OnLeftSoftButtonClick()
        {
            
        }

        protected override void OnRightSoftButtonClick()
        {
            OpenController(View.Parent as IView);
        }


        #region Maps

        void DownloadMapPosition()
        {

            PictureBox box = this.View.picMap;
            System.Drawing.Size size = new System.Drawing.Size();
            box.Invoke(new ThreadStart(delegate()
            {
                size = box.Size;
            }));

            CultureInfo culture = CultureInfo.GetCultureInfo("en-us");
            string googleMapsUrl = string.Format(BaseController.googleMapsUrl,
                size.Width, size.Height,
                pos.Latitude.Value.ToString(culture),
                pos.Longitude.Value.ToString(culture));

            Service.DownloadImage(googleMapsUrl);

        }


        void Service_ImageResult(object serder, ImageEventArgs e)
        {
            View.Invoke(new ThreadStart(delegate()
            {
                if (View.Visible && e.Url.StartsWith("http://maps.google.com"))
                {
                    PictureBox box = this.View.picMap;
                    var image = new System.Drawing.Bitmap(new System.IO.MemoryStream(e.Image));

                    box.Image = image;
                }
            }));
        }

        #endregion

        void Service_GeocodeResult(object serder, GeocodeEventArgs e)
        {
            View.Invoke(new ThreadStart(delegate()
            {
                if (View.Visible)
                {
                    foreach (Geocode geocode in e.Geocodes)
                    {
                        if (Array.IndexOf(geocode.Types, "street_address") > -1)
                        {
                            foreach (AddressComponent addr in geocode.AddressComponents)
                            {
                                if (Array.IndexOf(addr.Types, "route") > -1)
                                    View.txtAddress.Text = addr.LongName;
                                if (Array.IndexOf(addr.Types, "locality") > -1)
                                    View.txtCity.Text = addr.LongName;
                                if (Array.IndexOf(addr.Types, "administrative_area_level_1") > -1)
                                    View.txtState.Text = addr.ShortName;
                                if (Array.IndexOf(addr.Types, "postal_code") > -1)
                                    View.txtZip.Text = addr.ShortName;
                            }
                        }
                    }
                }
            }));
        }
    }
}
