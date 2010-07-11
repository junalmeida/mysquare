using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MySquare.Properties;
using Tenor.Mobile.Diagnostics;

namespace MySquare.UI
{
    internal partial class Help : UserControl, IView
    {
        public Help()
        {
            InitializeComponent();

            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(lblHelpText);
            lblHelpText.ForeColor = Tenor.Mobile.UI.Skin.Current.TextForeColor;
            lblHelpText.Text = string.Format(lblHelpText.Text, this.GetType().Assembly.GetName().Version);
        }


        Bitmap logo;

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (logo == null)
            {
                Size size = Resources.FoursquareLogo.Size;
                if (size.Width > pictureBox.Width)
                {
                    int percentage = 100 * pictureBox.Width / size.Width;
                    int height = percentage * size.Height / 100;
                    size = new Size(pictureBox.Width, height);
                    logo = new Bitmap(size.Width, size.Height);
                    using (Graphics g = Graphics.FromImage(logo))
                    {
                        g.DrawImage(Resources.FoursquareLogo, new Rectangle(0, 0, size.Width, size.Height), new Rectangle(0, 0, Resources.FoursquareLogo.Width, Resources.FoursquareLogo.Height), GraphicsUnit.Pixel);
                    }
                }
                else
                    logo = Resources.FoursquareLogo;

            }
            e.Graphics.DrawImage(logo, 0, 0);
        }



        #region Scroll Control
        int originalMouse;
        int originalScroll;
        private void Help_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originalMouse = e.Y;
                originalScroll = this.AutoScrollPosition.Y;
            }
        }

        private void Help_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.AutoScrollPosition = new Point(0, (originalMouse - e.Y) - originalScroll);
            }
        }
        #endregion

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("http://foursquare.com/legal/terms", string.Empty));
        }

    }
}
