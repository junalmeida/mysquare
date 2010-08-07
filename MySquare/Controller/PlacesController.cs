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
using System.IO;

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
            if (View.list1.txtSearch.Visible)
            {
                LeftSoftButtonText = "&Refresh";
                View.list1.txtSearch.Visible = false;
                ((UI.Main)View.Parent).inputPanel.Enabled = false;
            }
            else
            {
                Search();
            }
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
                LeftSoftButtonText = "&Cancel";
            }
        }

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
#if TESTING

            //Service.SearchNearby(text, -22.856025, -43.375182);
            //Service.SearchNearby(text, -22.908683, -43.17782);
            Service.SearchNearby(text, 37.535364,-77.509575);
            return;
#endif
            if (Program.Location.FixType == FixType.Gps)
            {
                position_LocationChanged(null, null);
            }
            else
            {
                Program.Location.PollHit += new EventHandler(position_LocationChanged);
                Program.Location.Error += new Tenor.Mobile.Location.ErrorEventHandler(position_Error);

                Program.Location.UseNetwork = true;
                Program.Location.UseGps = true;
                Program.Location.Poll();
            }
        }


        void position_Error(object sender, Tenor.Mobile.Location.ErrorEventArgs e)
        {
            Program.Location.PollHit -= new EventHandler(position_LocationChanged);
            Program.Location.Error -= new Tenor.Mobile.Location.ErrorEventHandler(position_Error);

            ShowError("Could not get your location, try again later.");
            Log.RegisterLog("lbs", e.Error);
        }

        void position_LocationChanged(object sender, EventArgs e)
        {
            Program.Location.PollHit -= new EventHandler(position_LocationChanged);
            Program.Location.Error -= new Tenor.Mobile.Location.ErrorEventHandler(position_Error);

            if (!Program.Location.WorldPoint.IsEmpty)
            {
                Service.SearchNearby(text, Program.Location.WorldPoint.Latitude, 
                                           Program.Location.WorldPoint.Longitude);
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


        void LoadVenues(Group[] groups)
        {
            View.list1.imageList = new Dictionary<string, byte[]>();
            View.list1.imageListBuffer = new Dictionary<string, Tenor.Mobile.Drawing.AlphaImage>();

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
            View.list1.listBox.AddItem("Create a new place", null);
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
                        if (!View.list1.imageList.ContainsKey(url))
                        {
                            byte[] buffer = Service.DownloadImageSync
                                (string.IsNullOrEmpty(url) ? "http://foursquare.com/img/categories/none.png" : url);

                            if (buffer != null)
                            {
                                View.list1.imageList[url] = buffer;
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
