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
            header_SelectedTabChanged(null, null);
        }

        private void header_SelectedTabChanged(object sender, EventArgs e)
        {

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
    }
}