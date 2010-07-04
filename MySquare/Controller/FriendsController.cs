using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Tenor.Mobile.Location;
using MySquare.FourSquare;
using System.Threading;

namespace MySquare.Controller
{
    class FriendsController : BaseController
    {
        public FriendsController(MySquare.UI.Friends.Friends view)
            : base((MySquare.UI.IView)view)
        {
            Service.CheckInsResult += new MySquare.FourSquare.CheckInsEventHandler(Service_CheckInsResult);
        }



        MySquare.UI.Friends.Friends View
        {
            get
            {
                return (MySquare.UI.Friends.Friends)base.view;
            }
        }


        protected override void Activate()
        {
            UI.Main form = View.Parent as UI.Main;
            form.ChangeFriendsName(null);

            View.BringToFront();
            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.Visible = true;

            LeftSoftButtonEnabled = true;
            LeftSoftButtonText = "&Refresh";
            RightSoftButtonEnabled = true;
            RightSoftButtonText = "&Search";

        }

        protected override void OnLeftSoftButtonClick()
        {
            LoadCheckIns();
        }

        WorldPosition position;
        private void LoadCheckIns()
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            View.listBox.Visible = false;
#if DEBUG
            if (Environment.OSVersion.Platform != PlatformID.WinCE)
            {
                Service.GetFriendsCheckins(text, -22.856025, -43.375182);
                return;
            }
#endif
            position = new WorldPosition(false, false);
            position.LocationChanged += new EventHandler(position_LocationChanged);
            position.Error += new ErrorEventHandler(position_Error);
            position.PollCell();
        }

        void position_Error(object sender, ErrorEventArgs e)
        {
            ShowError("Could not get your location, try again later.");
            Service.RegisterLog(e.Error);
            position = null;
        }

        void position_LocationChanged(object sender, EventArgs e)
        {
            if (position.Latitude.HasValue && position.Longitude.HasValue)
                Service.GetFriendsCheckins(position.Latitude.Value, position.Longitude.Value);
            else
            {
                ShowError("Could not get your location, try again later.");
                Service.RegisterLog(new Exception("Unknown error from location service."));
            }
            position = null;
        }

        void Service_CheckInsResult(object serder, MySquare.FourSquare.CheckInsEventArgs e)
        {
            View.Invoke(new ThreadStart(delegate()
                {
                    LoadCheckIns(e.CheckIns);
                }));
        }

        void LoadCheckIns(CheckIn[] checkIns)
        {
            // View.imageList = new Dictionary<string, Tenor.Mobile.Drawing.AlphaImage>();

            View.listBox.Clear();
            foreach (CheckIn checkin in checkIns)
            {
                View.listBox.AddItem(null, checkin);
            }

            //Thread t = new Thread(new ThreadStart(delegate()
            //{

            //}));
            //t.Start();
        }

    }
}
