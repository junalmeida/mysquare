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
            WaitThread.Set();

            string text = null;

            if (e.Exception is UnauthorizedAccessException)
                text = "Invalid credentials, change your settings and try again.";
            else
            {
                text = "Cannot connect to foursquare, try again.";
                Log.RegisterLog(e.Exception);
            }

            ShowError(text);
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
            }
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
        }

        public override void OnRightSoftButtonClick()
        {
            View.lblError.Visible = false;
            if (View.header.SelectedIndex == 0)
                BaseController.OpenController(View.places1);
            else
                BaseController.OpenController(View.friends1);

        }

        public override void Dispose()
        {
            View.Close();
            View.Dispose();
            base.Dispose();
        }
    }
}
