using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySquare.Properties;

namespace MySquare.UI
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);
            header.Tabs.Add(new Tenor.Mobile.UI.HeaderTab("Places", Resources.PinMap));

            header.Tabs.Add(new Tenor.Mobile.UI.HeaderTab("Settings", Resources.Settings));

            Program.Service.Error += new MySquare.FourSquare.ErrorEventHandler(Service_Error);

            header_SelectedTabChanged(null, null);
        }

        void Service_Error(object serder, MySquare.FourSquare.ErrorEventArgs e)
        {
            Program.WaitThread.Set();
            this.Invoke(new System.Threading.ThreadStart(delegate()
            {
                string text = null;

                if (e.Exception is UnauthorizedAccessException)
                    text = "Invalid credentials, change your settings and try again.";
                else
                    text = "Cannot connect to foursquare, try again.";

                ShowError(text);
            }));
        }


        internal void ShowError(string text)
        {
            settings1.Visible = false;
            places1.Visible = false;

            lblError.Text = text;
            lblError.Visible = true;

            ResetMenus();
            mnuRight.Text = "&Back";
            mnuLeft.Text = "&Refresh";
            mnuLeft.Enabled = false;

            Cursor.Current = Cursors.Default;
            Cursor.Show();
        }

        private void header_SelectedTabChanged(object sender, EventArgs e)
        {
            lblError.Visible = false;
            switch (header.Tabs[header.SelectedIndex].Text)
            {
                case "Places":
                    settings1.Visible = false;
                    ResetMenus();
                    ((IPanel)places1).ActivateControl(mnuLeft, mnuRight);
                    break;
                case "Settings":
                    places1.Visible = false;
                    ResetMenus();
                    ((IPanel)settings1).ActivateControl(mnuLeft, mnuRight);
                    break;
            }

        }

        private void ResetMenus()
        {
            mnuLeft.MenuItems.Clear();
            mnuRight.MenuItems.Clear();
        }

        private void mnuRight_Click(object sender, EventArgs e)
        {
            if (lblError.Visible)
                header_SelectedTabChanged(null, null);
        }
    }
}