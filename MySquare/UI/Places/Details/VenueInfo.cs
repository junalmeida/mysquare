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
            if (!string.IsNullOrEmpty(lblPhone.Text))
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
                Rectangle rect = new Rectangle(0, 0, control.Width, control.Height);
                using (MemoryStream mem = new MemoryStream(image))
                {
#if DEBUG
                    if (Environment.OSVersion.Platform == PlatformID.WinCE)
                    {
#endif

                        Tenor.Mobile.Drawing.AlphaImage.DrawImage(e.Graphics, mem,
                            rect
                            );

                        image = null;
#if DEBUG
                    }
                    else
                    {
                        using (Bitmap img = new Bitmap(mem))
                        {
                            e.Graphics.DrawImage(img, rect, new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
                        }
                    }
#endif
                }
            }
        }
    }
}
