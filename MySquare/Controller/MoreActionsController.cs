using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.UI.More;
using MySquare.UI;
using MySquare.Service;
using System.Windows.Forms;
using System.Threading;

namespace MySquare.Controller
{
    class MoreActionsController : BaseController<MoreActions>
    {
        RisingMobilityService service;
        public MoreActionsController(MoreActions view)
            : base(view)
        {
            service = new RisingMobilityService();
            service.Error += new ErrorEventHandler(service_Error);
            service.VersionArrived += new VersionInfoEventHandler(service_VersionArrived);

            View.listBox.SelectedItemChanged += new System.EventHandler(this.listBox_SelectedItemChanged);
            View.listBox.SelectedItemClicked += new System.EventHandler(this.listBox_SelectedItemClicked);
        }

        public override void Activate()
        {
            (View.Parent as Main).Reset();
            View.BringToFront();
            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.Visible = true;


            LeftSoftButtonEnabled = false;
            LeftSoftButtonText = "";
            RightSoftButtonEnabled = true;
            RightSoftButtonText = "&Exit";


        }

        public override void OnRightSoftButtonClick()
        {
            Program.Terminate();
        }

        public override void Deactivate()
        {
            View.Visible = false;
        }

        private void listBox_SelectedItemChanged(object sender, EventArgs e)
        {
            if (!Configuration.DoubleTap)
                listBox_SelectedItemClicked(sender, e);
        }

        private void listBox_SelectedItemClicked(object sender, EventArgs e)
        {
            switch (View.listBox.SelectedItem.YIndex)
            {
                case 0:
                    //shout
                    BaseController.OpenController((View.Parent as Main).shout1);
                    break;
                case 1:
                    //Leaderboard
                    BaseController.OpenController((View.Parent as Main).leaderboard1);
                    break;
                case 2:
                    //Updates
                    CheckUpdates();
                    break;
                case 3:
                    //About:
                    BaseController.OpenController((View.Parent as Main).help1);
                    break;
            }
        }

        private void CheckUpdates()
        {
            Cursor.Current = Cursors.WaitCursor;
            service.GetVersionInfo();
        }



        #region Check for updates
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
            LoadVersion(this.version, true);
        }

        internal static void LoadVersion(VersionInfoEventArgs version, bool showAllMessages)
        {
            Cursor.Current = Cursors.Default;

            if (Configuration.GetVersion() != version.Version)
            {
                if (MessageBox.Show("There is a new version available.\r\n\r\nDo you want to upgrade now?", "MySquare", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(
                            new System.Diagnostics.ProcessStartInfo("http://www.risingmobility.com/mysquare/update.axd/mysquare.cab", string.Empty));
                        Application.Exit();
                    }
                    catch { }
                }
            }
            else if (showAllMessages)
            {
                MessageBox.Show("You have the latest version.", "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }


        void service_Error(object serder, ErrorEventArgs e)
        {
            ShowError("Unable to check for updates. Try again later.");
        }


        #endregion

    }
}
