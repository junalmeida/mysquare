using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.FourSquare;
using System.Threading;
using System.Windows.Forms;
using MySquare.UI;
using MySquare.Service;

namespace MySquare.Controller
{
    class MainController : BaseController<UI.Main>
    {
        #region Controllers

        public MainController(UI.Main view)
            : base(view)
        {
            this.View.mnuLeft.Click += new EventHandler(mnuLeft_Click);
            this.View.mnuRight.Click += new EventHandler(mnuRight_Click);
            Service.Error += new ErrorEventHandler(Service_Error);
        }

        #endregion

        #region Error Control

        internal void ShowErrorForm(string text)
        {
            if (View.InvokeRequired)
                View.Invoke(new System.Threading.ThreadStart(delegate()
                {
                    ShowErrorForm(text);
                }));
            else
            {
                View.Reset();

                View.lblError.Text = text;
                View.lblError.Visible = true;


                RightSoftButtonText = "&Back";
                RightSoftButtonEnabled = true;
                LeftSoftButtonText = string.Empty;
                LeftSoftButtonEnabled = false;

                Cursor.Current = Cursors.Default;
                Cursor.Show();
            }
        }

        internal void Service_Error(object serder, ErrorEventArgs e)
        {
            if (WaitThread != null)
                WaitThread.Set();
            ShowError(e.Exception);
        }

        #endregion

        #region Menu Control

        protected override bool LeftSoftButtonEnabled
        {
            get
            {
                return View.mnuLeft.Enabled;
            }
            set
            {
                View.mnuLeft.Enabled = value;
            }
        }

        protected override string LeftSoftButtonText
        {
            get
            {
                return View.mnuLeft.Text;
            }
            set
            {
                View.mnuLeft.Text = value;
                View.mnuLeft.MenuItems.Clear();
            }
        }
        protected override void AddLeftSubMenu(MenuItem item)
        {
            View.mnuLeft.MenuItems.Add(item);
        }

        protected override bool RightSoftButtonEnabled
        {
            get
            {
                return View.mnuRight.Enabled;
            }
            set
            {
                View.mnuRight.Enabled = value;
                View.mnuRight.MenuItems.Clear();
            }
        }

        protected override string RightSoftButtonText
        {
            get
            {
                return View.mnuRight.Text;
            }
            set
            {
                View.mnuRight.Text = value;
            }
        }




        void mnuRight_Click(object sender, EventArgs e)
        {
            HitRightButton();
        }

        void mnuLeft_Click(object sender, EventArgs e)
        {
            HitLeftButton();
        }
        #endregion


        public override void Activate()
        {
            View.Reset();
            BaseController.OpenController(View.places1);
            StartAdTimer();
            
        }

        public override void OnRightSoftButtonClick()
        {
            View.lblError.Visible = false;
            if (View.header.SelectedIndex == 0)
                BaseController.OpenController(View.places1);
            else if (View.header.SelectedIndex == 1)
                BaseController.OpenController(View.friends1);
            else if (View.header.SelectedIndex == 2)
                BaseController.OpenController(View.settings1);
            else if (View.header.SelectedIndex == 3)
                BaseController.OpenController(View.help1);
        }

        public override void Dispose()
        {
            View.Close();
            View.Dispose();
            if (timer != null)
                timer.Dispose();
            base.Dispose();
        }

        #region AdSense

        RisingMobility adMob;
        System.Threading.Timer timer;

        private void StartAdTimer()
        {
            if (timer == null)
                timer = new System.Threading.Timer(new TimerCallback(this.GetAdSense), null,
           7000, 7000 * 5);
        }

        private void GetAdSense(object state)
        {
            if (Configuration.ShowAds)
            {
                if (adMob == null)
                {
                    //initialize
                    adMob = new RisingMobility();
                    adMob.AdArrived += new AdEventHandler(adMob_AdArrived);
                }
                //todo: check if mainform is activated
                if (Program.Location.WorldPoint.IsEmpty)
                    adMob.GetAd(null, null, lastTags);
                else
                    adMob.GetAd(Program.Location.WorldPoint.Latitude, Program.Location.WorldPoint.Longitude, lastTags);
            }
        }

        void adMob_AdArrived(object sender, AdEventArgs e)
        {
            try
            {
                timer.Change(60000 * 4, 60000 * 4);
                View.Invoke(new ThreadStart(delegate()
                {
                    View.picAd.Tag = e;
                    View.picAd.Invalidate();
                }));
            }
            catch (ObjectDisposedException) { }
        }


        #endregion
    }
}
