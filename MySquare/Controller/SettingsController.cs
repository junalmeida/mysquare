using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MySquare.Controller
{
    class SettingsController : BaseController<UI.Settings.Settings>
    {
        public SettingsController(UI.Settings.Settings view)
            : base(view)
        {
        }

        public override void Activate()
        {
            UI.Main form = View.Parent as UI.Main;
            form.places1.Visible = false;
            View.Dock = DockStyle.Fill;
            View.BringToFront();
            View.Visible = true;

            LeftSoftButtonEnabled = true;
            LeftSoftButtonText = "&Save";
            RightSoftButtonEnabled = true;
            RightSoftButtonText = "&Exit";

            Load();
        }

        public override void Deactivate()
        {
            View.Visible = false;
        }

        private void Load()
        {
            View.txtEmail.Text = MySquare.Service.Configuration.Login;
            View.txtPassword.Text = string.Empty;
            View.pnlPremium.Enabled = MySquare.Service.Configuration.IsPremium;
            View.chkShowAds.Checked = MySquare.Service.Configuration.ShowAds;
            View.chkUseGps.Checked = MySquare.Service.Configuration.UseGps;
            View.chkNotifications.Checked = MySquare.Service.Configuration.PingInterval > 0;
        }

        public override void OnLeftSoftButtonClick()
        {
            Save();
        }

        public override void OnRightSoftButtonClick()
        {
            Program.Terminate();
        }

        private void Save()
        {
            MySquare.Service.Configuration.Login = View.txtEmail.Text;
            if (!string.IsNullOrEmpty(View.txtPassword.Text))
                MySquare.Service.Configuration.Password = View.txtPassword.Text;

            MySquare.Service.Configuration.ShowAds = View.chkShowAds.Checked;
            MySquare.Service.Configuration.UseGps = View.chkUseGps.Checked;
            MySquare.Service.Configuration.PingInterval = 
                View.chkNotifications.Checked ?
                MySquare.Service.Configuration.DefaultPingInterval : 0;

            NotificationsController.Check();
            MessageBox.Show("Settings saved.", "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
        }

    }
}
