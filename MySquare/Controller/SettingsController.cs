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

            if (!string.IsNullOrEmpty(MySquare.Service.Configuration.Token))
                View.lnkOAuth.Text = "Re-auth";
            else
                View.lnkOAuth.Text = "Authenticate";

            View.lblLogin.Text = MySquare.Service.Configuration.Login;
            //View.txtPassword.Text = string.Empty;
            View.pnlPremium.Enabled = MySquare.Service.Configuration.IsPremium;
            View.chkShowAds.Checked = MySquare.Service.Configuration.ShowAds;
            View.chkUseGps.Checked = MySquare.Service.Configuration.UseGps;
            View.chkNotifications.Checked = MySquare.Service.Configuration.PingInterval > 0;
            View.cboAutoUpdate.Checked = MySquare.Service.Configuration.AutoUpdate;
            View.cboDoubleTap.Checked = MySquare.Service.Configuration.DoubleTap;

            View.cboMapType.Items.Clear();
            View.cboMapType.Items.Add(MySquare.Service.MapType.Roadmap);
            View.cboMapType.Items.Add(MySquare.Service.MapType.Satellite);
            View.cboMapType.Items.Add(MySquare.Service.MapType.Hybrid);
            View.cboMapType.Items.Add(MySquare.Service.MapType.Terrain);
            View.cboMapType.SelectedIndex = View.cboMapType.Items.IndexOf(MySquare.Service.Configuration.MapType);
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
            //MySquare.Service.Configuration.Login = View.txtEmail.Text;
            //if (!string.IsNullOrEmpty(View.txtPassword.Text))
                //MySquare.Service.Configuration.Password = View.txtPassword.Text;

            MySquare.Service.Configuration.ShowAds = View.chkShowAds.Checked;
            MySquare.Service.Configuration.UseGps = View.chkUseGps.Checked;
            MySquare.Service.Configuration.DoubleTap = View.cboDoubleTap.Checked;
            MySquare.Service.Configuration.AutoUpdate = View.cboAutoUpdate.Checked;
            MySquare.Service.Configuration.PingInterval =
            View.chkNotifications.Checked ?
            MySquare.Service.Configuration.DefaultPingInterval : 0;

            MySquare.Service.Configuration.MapType = (MySquare.Service.MapType)View.cboMapType.Items[View.cboMapType.SelectedIndex];

            MySquare.Service.Configuration.CheckNotifications();
            MessageBox.Show("Settings saved.", "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
        }

    }
}
