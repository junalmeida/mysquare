using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySquare.Properties;
using MySquare.Controller;
using Tenor.Mobile.UI;

namespace MySquare.UI
{
    public partial class Main : Form, IView
    {
        public Main()
        {
            InitializeComponent();

            this.Icon = Resources.mySquare;
            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);

            header.Tabs.Add(tabPlaces = new Tenor.Mobile.UI.HeaderTab("Places", Resources.PinMap));
            header.Tabs.Add(tabSettings = new Tenor.Mobile.UI.HeaderTab("Settings", Resources.Settings));
        }
        HeaderTab tabPlaces;
        HeaderTab tabSettings;

        internal void ChangePlacesName(string text)
        {
            if (string.IsNullOrEmpty(text))
                tabPlaces.Text = "Places";
            else
                tabPlaces.Text = text;
            header.Invalidate();
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



        private void inputPanel_EnabledChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (Control c in this.Controls)
                {
                    if (c.Visible && c is IView)
                    {
                        if (inputPanel.Enabled)
                        {
                            picAd.Height = inputPanel.Bounds.Height;
                            picAd.Visible = true;
                        }
                        else
                        {
                            picAd.Height = 30;
                            picAd.Visible = false;
                        }
                    }
                }
            }
            catch (ObjectDisposedException) { }
        }
    }
}