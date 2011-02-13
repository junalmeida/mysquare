using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySquare.Test
{
    public partial class OAuth : Form
    {
        public OAuth()
        {
            InitializeComponent();
        }

        private void startOAuth_Click(object sender, EventArgs e)
        {
            webBrowser.Navigate("http://risingmobility.com/mysquare/oauth.ashx");
        }

        public string Token = string.Empty;
        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.ToString().StartsWith("http://risingmobility.com/mysquare/oauth.ashx?code"))
            {
                Token = webBrowser.Document.Body.InnerText;
                this.Close();
            }
        }

    }
}
