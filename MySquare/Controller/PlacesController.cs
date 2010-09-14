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


            RightSoftButtonEnabled = true;
            RightSoftButtonText = "&Search";

            form.header.Tabs[0].Selected = true;
            if (View.list1.listBox.Count == 0)
                SearchPlaces();
            else
                ShowList();
        }

        private void BuildMenus()
        {
            LeftSoftButtonEnabled = true;
            LeftSoftButtonText = "&Refresh";


            MenuItem mnu = new MenuItem()
            {
                Text = "&Places",
                Checked = doPlaces,
            };
            mnu.Click += new EventHandler(ViewPlaces_Click);
            AddLeftSubMenu(mnu);


            mnu = new MenuItem()
            {
                Text = "&Tips",
                Checked = !doPlaces,
            };
            mnu.Click += new EventHandler(ViewTips_Click);
            AddLeftSubMenu(mnu);
        }



        public override void Deactivate()
        {
            View.Visible = false;
        }


        public override void OnLeftSoftButtonClick()
        {
            CloseSearch();
            BuildMenus();
        }

        void CloseSearch()
        {
            if (View.list1.txtSearch.Visible)
            {
                View.list1.txtSearch.Visible = false;
                ((UI.Main)View.Parent).inputPanel.Enabled = false;
            }

        }

        void ViewPlaces_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            item.Checked = true;
            item.Parent.MenuItems[1].Checked = false;

            CloseSearch();
            SearchPlaces();

        }


        void ViewTips_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            item.Checked = true;
            item.Parent.MenuItems[0].Checked = false;
            
            CloseSearch();
            SearchTips();
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


        bool doPlaces = true;
        string text;
        private void SearchPlaces(string text)
        {
            doPlaces = true;
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
            doPlaces = false;
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
                if (doPlaces)
                    Service.SearchNearby(text, Program.Location.WorldPoint.Latitude,
                                               Program.Location.WorldPoint.Longitude);
                else
                    Service.GetTipsNearby(Program.Location.WorldPoint.Latitude,
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
            if (View.InvokeRequired)
                View.Invoke(new ThreadStart(delegate()
                {
                    LoadVenues(e.Groups);
                }));
            else
                LoadVenues(e.Groups);

        }



        void Service_TipsResult(object serder, TipsEventArgs e)
        {
            if (View.InvokeRequired)
                View.Invoke(new ThreadStart(delegate()
                {
                    LoadVenues(e.Tips);
                }));
            else
                LoadVenues(e.Tips);
        }

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
            ShowList();


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
                                View.list1.ImageList[url] = buffer;
                                View.list1.listBox.Invoke(new ThreadStart(delegate()
                                {
                                    View.list1.listBox.Invalidate();
                                }));
                            }
                        }

                    }
            }));
            t.Start();
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
            ShowList();


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
                            View.list1.ImageList[url] = buffer;
                            View.list1.listBox.Invoke(new ThreadStart(delegate()
                            {
                                View.list1.listBox.Invalidate();
                            }));
                        }
                    }

                }
            }));
            t.Start();
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
            View.list1.BringToFront();
            View.list1.Dock = DockStyle.Fill;
            View.list1.Visible = true;
            Cursor.Current = Cursors.Default;
            Cursor.Show();
        }
    
    }
}
