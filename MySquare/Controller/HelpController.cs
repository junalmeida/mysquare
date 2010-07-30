using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.UI;
using System.Windows.Forms;
using MySquare.Service;
using System.Threading;

namespace MySquare.Controller
{
    class HelpController : BaseController<MySquare.UI.Help>
    {
        RisingMobility service;
        public HelpController(MySquare.UI.Help view)
            : base(view)
        {
            service = new RisingMobility();
            service.Error += new ErrorEventHandler(service_Error);
            service.VersionArrived += new VersionInfoEventHandler(service_VersionArrived);
        }

        VersionInfoEventArgs version;
        void service_VersionArrived(object sender, VersionInfoEventArgs e)
        {
            version = e;
            try
            {
                View.Invoke(new ThreadStart(LoadVersion));
            }
            catch (ObjectDisposedException) { }
            version = null;
        }

        private void LoadVersion()
        {
            if (Configuration.GetVersion() != version.Version)
            {
                MessageBox.Show("There is a new version available.\r\n\r\nDownload it now at http://www.risingmobility.com/mysquare", "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else
            {
                MessageBox.Show("You have the latest version.", "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }


        void service_Error(object serder, ErrorEventArgs e)
        {
            ShowError(e.Exception);
        }

        public override void Activate()
        {
            (View.Parent as Main).Reset();
            View.BringToFront();
            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.Visible = true;

            LeftSoftButtonEnabled = true;
            LeftSoftButtonText = "&Update";
            RightSoftButtonEnabled = true;
            RightSoftButtonText = "&Exit";

        }

        public override void Deactivate()
        {
            View.Visible = false;
        }

        public override void OnLeftSoftButtonClick()
        {
            service.GetVersionInfo();
        }


        public override void OnRightSoftButtonClick()
        {
            Program.Terminate();
        }

    }
}
