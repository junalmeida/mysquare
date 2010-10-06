using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using RisingMobility.Mobile.Location;
using System.Threading;
using MySquare.FourSquare;
using System.Windows.Forms;
using System.Drawing;
using MySquare.Service;
using System.IO;
using System.Diagnostics;

namespace MySquare.Controller
{
    class PlacesController : BaseController<UI.Places.Places>
    {

        public PlacesController(UI.Places.Places view)
            : base(view)
        {
            Service.SearchArrives += new MySquare.FourSquare.SearchEventHandler(Service_SearchArrives);
            Service.TipsResult += new TipsEventHandler(Service_TipsResult);
            Service.Error += new MySquare.Service.ErrorEventHandler(MainController.Service_Error);
            View.list1.Search += new EventHandler(list1_Search);
        }




        public override void Activate()
        {

            UI.Main form = View.Parent as UI.Main;

            form.ChangePlacesName(null);

            form.Reset();
            View.BringToFront();
            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.Visible = true;


            BuildMenus();

            if (form.lastTab <= 1)
                form.header.Tabs[form.lastTab].Selected = true;

            if ((ViewTips && tips == null) || (!ViewTips && venues == null))
                Refresh();
            else
                ShowList();

        }

        bool ViewTips
        {
            get
            {
                bool tips = false;
                try
                {
                    View.Invoke(new ThreadStart(delegate()
                    {
                        tips = (View.Parent as UI.Main).header.SelectedIndex == 1;
                    }));
                }
                catch (ObjectDisposedException) { }
                return tips;
            }
        }

        private void BuildMenus()
        {
            LeftSoftButtonEnabled = true;
            LeftSoftButtonText = "&Refresh";

            if (ViewTips)
            {
                RightSoftButtonEnabled = false;
                RightSoftButtonText = string.Empty;
            }
            else
            {
                RightSoftButtonEnabled = true;
                RightSoftButtonText = "&Search";
            }
        }



        public override void Deactivate()
        {
            View.Visible = false;
        }


        public override void OnLeftSoftButtonClick()
        {
            CloseSearch();
            if (LeftSoftButtonText == "&Refresh")
                Refresh();
            BuildMenus();
        }

        private void Refresh()
        {
            CloseSearch();
            if (ViewTips)
                SearchTips();
            else
                SearchPlaces();
        }

        void CloseSearch()
        {
            if (View.list1.txtSearch.Visible)
            {
                View.list1.txtSearch.Visible = false;
                ((UI.Main)View.Parent).inputPanel.Enabled = false;
            }
        }


        public override void OnRightSoftButtonClick()
        {
            SearchVenue();
        }

        void list1_Search(object sender, EventArgs e)
        {
            SearchVenue();
        }


        private void SearchVenue()
        {
            if (View.list1.txtSearch.Visible)
            {
                SearchPlaces(View.list1.txtSearch.Text);
            }
            else
            {
                View.list1.txtSearch.Visible = true;
                View.list1.txtSearch.Focus();
                LeftSoftButtonText = "&Cancel";
            }
        }

        private void SearchPlaces()
        {
            SearchPlaces(null);
        }


        string text;
        private void SearchPlaces(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                View.list1.txtSearch.Text = null;
                View.list1.txtSearch.Visible = false;
            }
            this.text = text;

            RequestLocation();
        }

        private void SearchTips()
        {
            RequestLocation();
        }

        private void RequestLocation()
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            View.list1.Visible = false;
#if TESTING

            //Service.SearchNearby(text, -22.856025, -43.375182);
            //Service.SearchNearby(text, -22.908683, -43.17782);
            Service.SearchNearby(text, 37.535364,-77.509575);
            return;
#endif

            Program.Location.Stop();
            Program.Location.PollHit += new EventHandler(position_LocationChanged);
            Program.Location.Error += new RisingMobility.Mobile.Location.ErrorEventHandler(position_Error);

            Program.Location.UseNetwork = true;
            Program.Location.UseGps = Configuration.UseGps;
            Program.Location.Poll();
        }



        void position_Error(object sender, RisingMobility.Mobile.Location.ErrorEventArgs e)
        {
            Program.Location.PollHit -= new EventHandler(position_LocationChanged);
            Program.Location.Error -= new RisingMobility.Mobile.Location.ErrorEventHandler(position_Error);

            ShowError("Could not get your location, try again later.");
            Log.RegisterLog("lbs", e.Error);
        }

        void position_LocationChanged(object sender, EventArgs e)
        {
            Program.Location.PollHit -= new EventHandler(position_LocationChanged);
            Program.Location.Error -= new RisingMobility.Mobile.Location.ErrorEventHandler(position_Error);

            if (!Program.Location.WorldPoint.IsEmpty)
            {
                View.list1.Address = null;
                if (ViewTips)
                    Service.GetTipsNearby(Program.Location.WorldPoint.Latitude,
                                           Program.Location.WorldPoint.Longitude);
                else
                    Service.SearchNearby(text, Program.Location.WorldPoint.Latitude,
                                               Program.Location.WorldPoint.Longitude);
                var geo = Program.Location.GetGeoLocation();
                if (geo != null)
                    View.list1.Address = geo;
            }
            else
            {
                ShowError("Could not get your location, try again later.");
                Log.RegisterLog("lbs", new Exception("Unknown error from location service."));
            }
        }

        void Service_SearchArrives(object serder, MySquare.FourSquare.SearchEventArgs e)
        {
            try
            {
                venues = e.Groups;
                if (View.InvokeRequired)
                    View.Invoke(new ThreadStart(ShowList));
                else
                    ShowList(true);
            }
            catch (ObjectDisposedException)
            {
            }
        }



        void Service_TipsResult(object serder, TipsEventArgs e)
        {
            try
            {
                tips = e.Tips;
                if (View.InvokeRequired)
                    View.Invoke(new ThreadStart(ShowList));
                else
                    ShowList(true);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        Tip[] tips = null;
        Group[] venues = null;

        void LoadVenues(Group[] groups)
        {
            View.list1.ImageList = new Dictionary<string, byte[]>();

            View.list1.listBox.Clear();

            foreach (Group g in groups)
            {
                if (!string.IsNullOrEmpty(g.Type))
                    View.list1.listBox.AddItem(g.Type, null, View.list1.listBox.DefaultItemHeight / 2);
                foreach (Venue venue in g.Venues)
                {
                    View.list1.listBox.AddItem(venue.Name, venue);
                }
            }
            AddCreatePlace();


            Thread t = new Thread(new ThreadStart(delegate()
            {
                foreach (Group g in groups)
                    foreach (Venue venue in g.Venues)
                    {
                        //TODO: revise the null category image.
                        string url = string.Empty;
                        if (venue.PrimaryCategory != null)
                            url = venue.PrimaryCategory.IconUrl;
                        if (!View.list1.ImageList.ContainsKey(url))
                        {
                            byte[] buffer = Service.DownloadImageSync
                                (string.IsNullOrEmpty(url) ? "http://foursquare.com/img/categories/none.png" : url);

                            if (buffer != null)
                            {
                                try
                                {
                                    View.list1.ImageList[url] = buffer;
                                    View.list1.listBox.Invoke(new ThreadStart(delegate()
                                    {
                                        View.list1.listBox.Invalidate();
                                    }));
                                }
                                catch (ObjectDisposedException)
                                {
                                    return;
                                }
                            }
                        }

                    }
            }));
            t.StartThread();
        }

        private void LoadVenues(Tip[] tips)
        {
            View.list1.ImageList = new Dictionary<string, byte[]>();

            View.list1.listBox.Clear();


            View.list1.listBox.AddItem("Nearby", null, View.list1.listBox.DefaultItemHeight / 2);
            foreach (Tip tip in tips)
            {
                View.list1.listBox.AddItem(tip.Venue.Name, tip, View.list1.Measure(tip));
            }

            AddCreatePlace();

            Thread t = new Thread(new ThreadStart(delegate()
            {
                foreach (Tip tip in tips)
                {
                    Venue venue = tip.Venue;
                    //TODO: revise the null category image.
                    string url = string.Empty;
                    if (venue.PrimaryCategory != null)
                        url = venue.PrimaryCategory.IconUrl;
                    if (!View.list1.ImageList.ContainsKey(url))
                    {
                        byte[] buffer = Service.DownloadImageSync
                            (string.IsNullOrEmpty(url) ? "http://foursquare.com/img/categories/none.png" : url);

                        if (buffer != null)
                        {
                            try
                            {
                                View.list1.ImageList[url] = buffer;
                                View.list1.listBox.Invoke(new ThreadStart(delegate()
                                {
                                    View.list1.listBox.Invalidate();
                                }));
                            }
                            catch (ObjectDisposedException)
                            {
                                return;
                            }
                        }
                    }

                }
            }));
            t.StartThread();
        }

        private void AddCreatePlace()
        {
            string newItemText = "Create a new place";
            if (View.list1.listBox.Count > 0)
                newItemText += "\r\nIf you cannot find a venue, tap here.";
            else
                newItemText += "\r\nNo place found. Tap here to create.";


            View.list1.listBox.AddItem(newItemText, null);
        }


        private void ShowList()
        {
            ShowList(false);
        }
        private void ShowList(bool force)
        {
            try
            {
                View.list1.BringToFront();
                View.list1.Dock = DockStyle.Fill;

                if (ViewTips && tips != null &&
                   (force || tips.Length == 0 || View.list1.listBox.Count == 0 || (View.list1.listBox.Count > 1 && View.list1.listBox[1].Value != tips[0])))
                    LoadVenues(tips);
                else if
                   (!ViewTips && venues != null &&
                   (force || (venues.Length == 0 || venues[0].Venues.Length == 0) || View.list1.listBox.Count == 0 || (View.list1.listBox.Count > 1 && View.list1.listBox[1].Value != venues[0].Venues[0])))
                    LoadVenues(venues);

                View.list1.Visible = true;

                Cursor.Current = Cursors.Default;
                Cursor.Show();
            }
            catch (ObjectDisposedException)
            { }
        }
    
    }
}
