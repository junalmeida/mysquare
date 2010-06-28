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
        UI.Main Form
        {
            get { return (UI.Main)base.view; }
        }
        #region Controllers

        internal MainController(UI.Main view)
            : base((MySquare.UI.IView)view)
        {
            this.Form.mnuLeft.Click += new EventHandler(mnuLeft_Click);
            this.Form.mnuRight.Click += new EventHandler(mnuRight_Click);
            Service.Error += new ErrorEventHandler(Service_Error);
        }

        #endregion

        #region Error Control

        protected override void ShowError(string text)
        {
            if (Form.InvokeRequired)
                Form.Invoke(new System.Threading.ThreadStart(delegate()
                {
                    ShowErrorForm(text);
                }));
            else
                ShowErrorForm(text);
        }

        private void ShowErrorForm(string text)
        {
            Form.settings1.Visible = false;
            Form.places1.Visible = false;

            Form.lblError.Text = text;
            Form.lblError.Visible = true;

            
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
                return Form.mnuLeft.Enabled;
            }
            set
            {
                Form.mnuLeft.Enabled = value;
            }
        }

        protected override string LeftSoftButtonText
        {
            get
            {
                return Form.mnuLeft.Text;
            }
            set
            {
                Form.mnuLeft.Text = value;
            }
        }

        protected override bool RightSoftButtonEnabled
        {
            get
            {
                return Form.mnuRight.Enabled;
            }
            set
            {
                Form.mnuRight.Enabled = value;
            }
        }

        protected override string RightSoftButtonText
        {
            get
            {
                return Form.mnuRight.Text;
            }
            set
            {
                Form.mnuRight.Text = value;
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
            Form.settings1.Visible = false;
            Form.places1.Visible = false;
            BaseController.OpenController(Form.places1);
        }

        protected override void OnRightSoftButtonClick()
        {
            BaseController.OpenController(this.Form.places1);
        }
    }
}
