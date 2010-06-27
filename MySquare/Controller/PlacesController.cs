using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Tenor.Mobile.Location;
using System.Threading;
using MySquare.FourSquare;
using System.Windows.Forms;

namespace MySquare.Controller
{
    class PlacesController : BaseController
    {
        UI.Places.Places view;
        public PlacesController(UI.Places.Places view)
        {
            this.view = view;
            this.Service.SearchArrives += new MySquare.FourSquare.SearchEventHandler(Service_SearchArrives);
        }


        protected override void Activate()
        {

            UI.Main form = view.Parent as UI.Main;
            form.settings1.Visible = false;
            view.BringToFront();
            view.Dock = System.Windows.Forms.DockStyle.Fill;
            view.Visible = true;

            LeftSoftButtonEnabled = true;
            LeftSoftButtonText = "&Refresh";
            RightSoftButtonEnabled = true;
            RightSoftButtonText = "&Create";

            if (view.list1.listBox.Count == 0)
                Search();
            else
                ShowList();
        }

        protected override void OnLeftSoftButtonClick()
        {
            Search();
        }

        protected override void OnRightSoftButtonClick()
        {
            CreateVenue();
        }

        private void CreateVenue()
        {
            throw new NotImplementedException();
        }

        WorldPosition position;
        private void Search()
        {

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            view.list1.Visible = false;
#if DEBUG
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
#endif
                position = new WorldPosition(false, false);
                position.LocationChanged += new EventHandler(position_LocationChanged);
                position.Error += new EventHandler(position_Error);

                position.PollCell();
#if DEBUG
            }
            else
            {
               
                Service.SearchNearby(null, -22.856025, -43.375182);
            }
#endif

        }

        void position_Error(object sender, EventArgs e)
        {
            ShowError("Could not get your location, try again later.");
            position = null;
        }

        void position_LocationChanged(object sender, EventArgs e)
        {
            if (position.Latitude.HasValue && position.Longitude.HasValue)
            {
                Service.SearchNearby(null, position.Latitude.Value, position.Longitude.Value);
            }
            else
            {
                ShowError("Could not get your location, try again later.");
            }
            position = null;
        }

        void Service_SearchArrives(object serder, MySquare.FourSquare.SearchEventArgs e)
        {
            if (view.InvokeRequired)
                view.Invoke(new ThreadStart(delegate()
                {
                    LoadVenues(e.Venues);
                }));
            else
                LoadVenues(e.Venues);
        }


        void LoadVenues(Venue[] venues)
        {
            view.list1.listBox.Clear();
            foreach (Venue venue in venues)
            {
                view.list1.listBox.AddItem(venue.Name, venue);
            }
            ShowList();
        }

        private void ShowList()
        {
            view.venueDetails1.Visible = false;
            view.list1.BringToFront();
            view.list1.Dock = DockStyle.Fill;
            view.list1.Visible = true;
            Cursor.Current = Cursors.Default;
            Cursor.Show();
        }
    }
}
