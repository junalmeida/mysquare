using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MySquare.UI.Places.Details
{
    public partial class VenueInfo : UserControl
    {
        public VenueInfo()
        {
            InitializeComponent();
        }

        internal void Activate()
        {
            Dock = DockStyle.Fill;
            BringToFront();
            Visible = true;
        }

        private void lblPhone_Click(object sender, EventArgs e)
        {
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                Microsoft.WindowsMobile.Telephony.Phone p = new Microsoft.WindowsMobile.Telephony.Phone();
                p.Talk(lblPhone.Text, true);
            }
        }
    }
}
