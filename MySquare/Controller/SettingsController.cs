using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MySquare.Controller
{
    class SettingsController : BaseController
    {
        UI.Settings.Settings view;
        public SettingsController(UI.Settings.Settings view)
        {
            this.view = view;
        }

        protected override void Activate()
        {
            UI.Main form = view.Parent as UI.Main;
            form.places1.Visible = false;
            view.Dock = DockStyle.Fill;
            view.BringToFront();
            view.Visible = true;

            LeftSoftButtonEnabled = true;
            LeftSoftButtonText = "&Save";
            RightSoftButtonEnabled = true;
            RightSoftButtonText = "&Exit";

            Load();
        }

        private void Load()
        {
            view.txtEmail.Text = Service.Login;
            view.txtPassword.Text = string.Empty;
        }

        protected override void OnLeftSoftButtonClick()
        {
            Save();
        }

        protected override void OnRightSoftButtonClick()
        {
            Application.Exit();
        }

        private void Save()
        {
            Service.Login = view.txtEmail.Text;
            if (!string.IsNullOrEmpty(view.txtPassword.Text))
                Service.Password = view.txtPassword.Text;

            MessageBox.Show("Settings saved.", "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
        }



    }
}
