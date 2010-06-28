using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

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
            contextMenu.Show(lblPhone, new Point(0, lblPhone.Height));
        }

        private void mnuDial_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(lblPhone.Text);
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                Microsoft.WindowsMobile.Telephony.Phone p = new Microsoft.WindowsMobile.Telephony.Phone();
                p.Talk(lblPhone.Text, true);
            }

        }

        private void Img_Paint(object sender, PaintEventArgs e)
        {
            Control control = ((Control)sender);
            byte[] image = control.Tag as byte[];
            if (image != null)
            {
                using (MemoryStream mem = new MemoryStream(image))
                {
                    Tenor.Mobile.Drawing.AlphaImage.DrawImage(e.Graphics, mem,
                        new Rectangle(0, 0, control.Width, control.Height)
                        );
                }
                image = null;
            }
        }
    }
}
