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
    internal partial class VenueInfo : UserControl
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
                contextMenu.Show(lblPhone, new Point(0, lblPhone.Height / 2));
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
            if (control.Tag != null)
            {
                try
                {
                    if (control.Tag is byte[])
                        control.Tag = new Tenor.Mobile.Drawing.AlphaImage((byte[])control.Tag);
                    Tenor.Mobile.Drawing.AlphaImage image = (Tenor.Mobile.Drawing.AlphaImage)control.Tag;

                    Rectangle rect = new Rectangle(0, 0, control.Width, control.Height);


                    image.Draw(e.Graphics, rect);
                }
                catch { control.Tag = null; }

            }
        }

        #region Scroll Control
        int originalMouse;
        int originalScroll;
        private void VenueInfo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originalMouse = e.Y;
                originalScroll = this.AutoScrollPosition.Y;
            }
        }

        private void VenueInfo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.AutoScrollPosition = new Point(0, (originalMouse - e.Y) - originalScroll);
            }
        }
        #endregion

    }
}
