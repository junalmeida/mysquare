using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.FourSquare;
using System.Threading;
using System.Windows.Forms;

namespace MySquare.Controller
{
    class MainController : BaseController
    {
        UI.Main form;
        #region Controllers

        internal MainController(UI.Main form)
            : base()
        {
            this.form = form;
            this.view = form;
            this.form.mnuLeft.Click += new EventHandler(mnuLeft_Click);
            this.form.mnuRight.Click += new EventHandler(mnuRight_Click);
            Service.Error += new ErrorEventHandler(Service_Error);
        }

        #endregion

        #region Error Control

        protected override void ShowError(string text)
        {
            if (form.InvokeRequired)
                form.Invoke(new System.Threading.ThreadStart(delegate()
                {
                    ShowErrorForm(text);
                }));
            else
                ShowErrorForm(text);
        }

        private void ShowErrorForm(string text)
        {
            form.settings1.Visible = false;
            form.places1.Visible = false;

            form.lblError.Text = text;
            form.lblError.Visible = true;

            
            RightSoftButtonText = "&Back";
            LeftSoftButtonText = "&Refresh";
            LeftSoftButtonEnabled = false;

            Cursor.Current = Cursors.Default;
            Cursor.Show();
        }

        void Service_Error(object serder, ErrorEventArgs e)
        {
            WaitThread.Set();

            string text = null;

            if (e.Exception is UnauthorizedAccessException)
                text = "Invalid credentials, change your settings and try again.";
            else
                text = "Cannot connect to foursquare, try again.";

            ShowError(text);
        }

        #endregion

        #region Menu Control

        protected override bool LeftSoftButtonEnabled
        {
            get
            {
                return form.mnuLeft.Enabled;
            }
            set
            {
                form.mnuLeft.Enabled = value;
            }
        }

        protected override string LeftSoftButtonText
        {
            get
            {
                return form.mnuLeft.Text;
            }
            set
            {
                form.mnuLeft.Text = value;
            }
        }

        protected override bool RightSoftButtonEnabled
        {
            get
            {
                return form.mnuRight.Enabled;
            }
            set
            {
                form.mnuRight.Enabled = value;
            }
        }

        protected override string RightSoftButtonText
        {
            get
            {
                return form.mnuRight.Text;
            }
            set
            {
                form.mnuRight.Text = value;
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


        protected override void Activate()
        {
            form.settings1.Visible = false;
            form.places1.Visible = false;
            BaseController.OpenController(form.places1);
        }

    }
}
