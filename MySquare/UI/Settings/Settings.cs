using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MySquare.UI.Settings
{
    internal partial class Settings : UserControl, IView
    {
        public Settings()
        {
            InitializeComponent();
            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawSeparator(new Rectangle(0, 0, pnlPremium.Width, 5), pnlPremium.BackColor);
        }




        #region Scroll Control
        int originalMouse;
        int originalScroll;
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Control control = sender as Control;
                int top = control == this ? 0 : control.Top;
                originalMouse = e.Y + top;
                originalScroll = this.AutoScrollPosition.Y;
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Control control = sender as Control;
                int top = control == this ? 0 : control.Top;
                this.AutoScrollPosition = new Point(0, (originalMouse - (e.Y + top)) - originalScroll);
            }
        }
        #endregion

        private void lnkOAuth_Click(object sender, EventArgs e)
        {
            this.AutoScrollPosition = new Point(0,0);
            this.AutoScroll = false;
            webBrowser.Location = new Point(0, 0);
            webBrowser.Size = new Size(this.Size.Width, this.Size.Height);

            webBrowser.Url = new Uri("http://risingmobility.com/mysquare/oauth.ashx");
            webBrowser.Show();
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            
        }

    }

}
