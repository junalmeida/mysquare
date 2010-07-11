using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Tenor.Mobile.Location;
using System.Threading;
using MySquare.FourSquare;
using System.Windows.Forms;
using System.Drawing;
using MySquare.Service;

namespace MySquare.Controller
{
    class PlacesController : BaseController<UI.Places.Places>
    {

        public PlacesController(UI.Places.Places view)
            : base(view)
        {
            Service.SearchArrives += new MySquare.FourSquare.SearchEventHandler(Service_SearchArrives);
            Service.Error += new MySquare.Service.ErrorEventHandler(MainController.Service_Error);
        }


        public override void Activate()
        {

            UI.Main form = View.Parent as UI.Main;

            form.ChangePlacesName(null);

            form.Reset();
            View.BringToFront();
            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.Visible = true;

            LeftSoftButtonEnabled = true;
            LeftSoftButtonText = "&Refresh";
            RightSoftButtonEnabled = true;
            RightSoftButtonText = "&Search";

            form.header.Tabs[0].Selected = true;
            if (View.list1.listBox.Count == 0)
                Search();
            else
                ShowList();
        }

        public override void Deactivate()
        {
            View.Visible = false;
        }


        public override void OnLeftSoftButtonClick()
        {
            Search();
        }

        public override void OnRightSoftButtonClick()
        {
            SearchVenue();
        }

        private void SearchVenue()
        {
            if (View.list1.txtSearch.Visible)
            {
                Search(View.list1.txtSearch.Text);
            }
            else
            {
                View.list1.txtSearch.Visible = true;
                View.list1.txtSearch.Focus();
            }
        }

        WorldPosition position;
        private void Search()
        {
            Search(null);
        }

        string text;
        private void Search(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                View.list1.txtSearch.Text = null;
                View.list1.txtSearch.Visible = false;
            }
            this.text = text;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            View.list1.Visible = false;
#if DEBUG
            if (Environment.OSVersion.Platform != PlatformID.WinCE)
            {
                Service.SearchNearby(text, -22.856025, -43.375182);
                return;
            }

#endif
            position = new WorldPosition(false, false);
            position.LocationChanged += new EventHandler(position_LocationChanged);
            position.Error += new Tenor.Mobile.Location.ErrorEventHandler(position_Error);

            position.PollCell();

        }

        void position_Error(object sender, Tenor.Mobile.Location.ErrorEventArgs e)
        {
            ShowError("Could not get your location, try again later.");
            Log.RegisterLog(e.Error);
            position = null;
        }

        void position_LocationChanged(object sender, EventArgs e)
        {
            if (position.Latitude.HasValue && position.Longitude.HasValue)
                Service.SearchNearby(text, position.Latitude.Value, position.Longitude.Value);
            else
            {
                ShowError("Could not get your location, try again later.");
                Log.RegisterLog(new Exception("Unknown error from location service."));
            }
            position = null;
        }

        void Service_SearchArrives(object serder, MySquare.FourSquare.SearchEventArgs e)
        {
            if (View.InvokeRequired)
                View.Invoke(new ThreadStart(delegate()
                {
                    LoadVenues(e.Venues);
                }));
            else
                LoadVenues(e.Venues);
        }


        void LoadVenues(Venue[] venues)
        {
            View.list1.imageList = new Dictionary<string, Tenor.Mobile.Drawing.AlphaImage>();

            View.list1.listBox.Clear();
            foreach (Venue venue in venues)
            {
                View.list1.listBox.AddItem(venue.Name, venue);
            }
            View.list1.listBox.AddItem(null, null);
            ShowList();


            Thread t = new Thread(new ThreadStart(delegate()
            {
                foreach (Venue venue in venues)
                {
                    if (venue.PrimaryCategory != null)
                    {
                        string url = venue.PrimaryCategory.IconUrl;
                        if (!string.IsNullOrEmpty(url))
                        {
                            byte[] buffer = Service.DownloadImageSync(url);
                            if (buffer != null)
                            {
                                View.list1.imageList[url] = new Tenor.Mobile.Drawing.AlphaImage(buffer);
                                buffer = null;
                                View.list1.listBox.Invoke(new ThreadStart(delegate()
                                {
                                    View.list1.listBox.Invalidate();
                                }));
                            }
                        }
                    }
                }
            }));
            t.Start();
        }

        private void ShowList()
        {
            View.list1.BringToFront();
            View.list1.Dock = DockStyle.Fill;
            View.list1.Visible = true;
            Cursor.Current = Cursors.Default;
            Cursor.Show();
        }
    }
}
