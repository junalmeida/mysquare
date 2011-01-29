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
            try
            {
                if (View.InvokeRequired)
                    View.Invoke(new System.Threading.ThreadStart(delegate()
                    {
                        ShowErrorForm(text);
                    }));
                else
                {
                    try
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
                    catch { }
                }
            }
            catch (ObjectDisposedException) { }
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
                try
                {
                    View.mnuLeft.Enabled = value;
                }
                catch (ThreadAbortException)
                { }
                catch (ObjectDisposedException)
                { }
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
                try
                {
                    View.mnuLeft.Text = value;
                    View.mnuLeft.MenuItems.Clear();
                }
                catch (ThreadAbortException)
                { }
                catch (ObjectDisposedException)
                { }
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
                try
                {
                    View.mnuRight.Enabled = value;
                    View.mnuRight.MenuItems.Clear();
                }
                catch (ThreadAbortException)
                { }
                catch (ObjectDisposedException)
                { }
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
                try
                {
                    View.mnuRight.Text = value;
                }
                catch (ThreadAbortException)
                { }
                catch (ObjectDisposedException)
                { }
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
            CurrentController = lastController;
            BaseController.Controllers[lastController].Activate();
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

        RisingMobilityService adMob;
        System.Threading.Timer timer;

        private void StartAdTimer()
        {
            if (timer == null)
                timer = new System.Threading.Timer(new TimerCallback(this.GetAdSense), null,
           7000, 7000 * 5);
        }

        bool updateChecked = false;
        private void GetAdSense(object state)
        {
            if (adMob == null)
            {
                //initialize
                adMob = new RisingMobilityService();
                adMob.AdArrived += new AdEventHandler(adMob_AdArrived);
                adMob.VersionArrived += new VersionInfoEventHandler(service_VersionArrived);
            }
            if (Configuration.ShowAds)
            {

                //todo: check if mainform is activated
                if (Program.Location.WorldPoint.IsEmpty)
                    adMob.GetAd(null, null, lastTags);
                else
                    adMob.GetAd(Program.Location.WorldPoint.Latitude, Program.Location.WorldPoint.Longitude, lastTags);
            }
            if (!updateChecked && !Configuration.IsAlpha)
            {
                updateChecked = true;
                adMob.GetVersionInfo();
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

        void service_VersionArrived(object sender, VersionInfoEventArgs e)
        {
            try
            {
                View.Invoke(new ThreadStart(delegate()
                    {
                        MoreActionsController.LoadVersion(e, false);
                    }));
            }
            catch (ObjectDisposedException) { }
        }

        #endregion

    }
}
