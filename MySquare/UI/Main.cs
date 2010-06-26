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

            header_SelectedTabChanged(null, null);
        }

        private void header_SelectedTabChanged(object sender, EventArgs e)
        {

            switch (header.SelectedIndex)
            {
                case 0:
                    places1.BringToFront();
                    places1.Dock = DockStyle.Fill;
                    places1.Visible = true;

                    ((IPanel)places1).ActivateControl(mnuLeft, mnuRight);
                    break;
            }

        }
    }
}