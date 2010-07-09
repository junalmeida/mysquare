﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Tenor.Mobile.Drawing;

namespace MySquare.UI.Friends
{
    public partial class UserDetail : UserControl, IView
    {
        public UserDetail()
        {
            InitializeComponent();
            tabStrip.Tabs.Add("Info");
            tabStrip.Tabs.Add("Friends");
            tabStrip.Tabs.Add("Badges");
        }

        SizeF factor;
        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this.factor = factor;
            base.ScaleControl(factor, specified);
        }


        private void tabStrip1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        Image avatar;
        internal byte[] Avatar
        {
            set
            {
                if (value != null)
                    using (MemoryStream mem = new MemoryStream(value))
                    using (Bitmap bmp = new Bitmap(mem))
                        avatar = Main.CreateRoundedAvatar(bmp, picAvatar.Width, factor);
                else if (avatar != null)
                {
                    avatar.Dispose();
                    avatar = null;
                }
            }
        }

        private void picAvatar_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (avatar != null)
                {
                    AlphaImage alpha = new AlphaImage(avatar);
                    alpha.Draw(e.Graphics,
                                new Rectangle(
                                    0,
                                    0,
                                    picAvatar.Width,
                                    picAvatar.Height), Tenor.Mobile.UI.Skin.Current.SelectedBackColor);
                }

            }
            catch { }
        }
    }
}
