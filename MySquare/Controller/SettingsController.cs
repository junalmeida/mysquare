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
            View.txtEmail.Text = Service.Login;
            View.txtPassword.Text = string.Empty;
        }

        public override void OnLeftSoftButtonClick()
        {
            Save();
        }

        public override void OnRightSoftButtonClick()
        {
            Application.Exit();
        }

        private void Save()
        {
            Service.Login = View.txtEmail.Text;
            if (!string.IsNullOrEmpty(View.txtPassword.Text))
                Service.Password = View.txtPassword.Text;

            MessageBox.Show("Settings saved.", "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
        }

    }
}
