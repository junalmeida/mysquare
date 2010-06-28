using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MySquare.Controller
{
    class SettingsController : BaseController
    {
        UI.Settings.Settings View
        {
            get
            {
                return (UI.Settings.Settings)base.view;
            }
        }
        public SettingsController(UI.Settings.Settings view)
            : base((MySquare.UI.IView)view)
        {
        }

        protected override void Activate()
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

        private void Load()
        {
            View.txtEmail.Text = Service.Login;
            View.txtPassword.Text = string.Empty;
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
            Service.Login = View.txtEmail.Text;
            if (!string.IsNullOrEmpty(View.txtPassword.Text))
                Service.Password = View.txtPassword.Text;

            MessageBox.Show("Settings saved.", "MySquare", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
        }



    }
}
