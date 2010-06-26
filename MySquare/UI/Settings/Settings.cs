using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MySquare.UI.Settings
{
    public partial class Settings : UserControl, IPanel
    {
        public Settings()
        {
            InitializeComponent();
            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);
        }

        #region IPanel Members


        MenuItem leftSoft, rightSoft;
        public void ActivateControl(MenuItem leftSoft, MenuItem rightSoft)
        {
            if (this.leftSoft != leftSoft)
            {
                this.leftSoft = leftSoft;
                this.leftSoft.Click += new EventHandler(leftSoft_Click);
            }

            if (this.rightSoft != rightSoft)
            {
                this.rightSoft = rightSoft;
                this.rightSoft.Click += new EventHandler(rightSoft_Click);
            }

            BringToFront();
            Dock = DockStyle.Fill;
            Visible = true;

            leftSoft.Text = "&Save";
            rightSoft.Text = "&Exit";

            Load();
        }


        void leftSoft_Click(object sender, EventArgs e)
        {
            if (Visible)
                Save();
        }

        void rightSoft_Click(object sender, EventArgs e)
        {
            if (Visible)
                Exit();
        }

        #endregion

        private void Load()
        {
            txtEmail.Text = Program.Service.Login;
            txtPassword.Text = string.Empty;
        }

        private void Save()
        {
            Program.Service.Login = txtEmail.Text;
            if (!string.IsNullOrEmpty(txtPassword.Text))
                Program.Service.Password = txtPassword.Text;

            MessageBox.Show("Settings saved.", "MySquare");
        }

        private void Exit()
        {
            Application.Exit();
        }
    }
}
