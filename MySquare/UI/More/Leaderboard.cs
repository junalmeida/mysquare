using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MySquare.Service;
using Tenor.Mobile.Drawing;
using MySquare.Properties;

namespace MySquare.UI.More
{
    partial class Leaderboard : UserControl, IView
    {
        public Leaderboard()
        {
            InitializeComponent();
            tabStrip.Tabs.Add("My Friends");
            tabStrip.Tabs.Add("All");
            
        }
        AlphaImage img;
        private void picAvatar_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (img == null)
                    img = new AlphaImage(Resources.Leaderboard);
                img.Draw(e.Graphics, new Rectangle(0, 0, picAvatar.Width, picAvatar.Height));
            }
            catch (Exception ex)
            {
                Log.RegisterLog("gdi", ex);
            }
        }
    }
}
