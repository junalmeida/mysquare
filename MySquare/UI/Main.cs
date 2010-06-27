using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySquare.Properties;
using MySquare.Controller;

namespace MySquare.UI
{
    public partial class Main : Form, IView
    {
        public Main()
        {
            InitializeComponent();

            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);

            header.Tabs.Add(new Tenor.Mobile.UI.HeaderTab("Places", Resources.PinMap));
            header.Tabs.Add(new Tenor.Mobile.UI.HeaderTab("Settings", Resources.Settings));
        }



        private void header_SelectedTabChanged(object sender, EventArgs e)
        {
            lblError.Visible = false;
            switch (header.Tabs[header.SelectedIndex].Text)
            {
                case "Places":
                    Controller.BaseController.OpenController(places1);
                    break;
                case "Settings":
                    Controller.BaseController.OpenController(settings1);
                    break;
            }

        }

        private void Main_Load(object sender, EventArgs e)
        {
            mnuLeft.Text = string.Empty;
            mnuRight.Text = string.Empty;

            Application.DoEvents();
            BaseController.OpenController(this);
        }
    }
}