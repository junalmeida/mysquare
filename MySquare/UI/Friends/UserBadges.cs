using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MySquare.FourSquare;
using MySquare.Service;
using Tenor.Mobile.Drawing;
using System.IO;

namespace MySquare.UI.Friends
{
    public partial class UserBadges : UserControl
    {
        public UserBadges()
        {
            InitializeComponent();
            font = new Font(Font.Name, 8, FontStyle.Regular);
            fontBold = new Font(Font.Name, 9, FontStyle.Bold);
        }


        internal Dictionary<string, byte[]> imageList;
        Dictionary<string, AlphaImage> imageListA;
        Badge[] badges;

        internal Badge[] Badges
        {
            get { return badges; }
            set
            {
                badges = value;
                imageList = new Dictionary<string, byte[]>();
                Program.ClearImageList(imageListA);
                imageListA = new Dictionary<string, AlphaImage>();
                listBox.Clear();
                if (badges != null)
                    foreach (Badge badge in badges)
                        listBox.AddItem(null, badge, MeasureHeight(badge));
            }
        }

        
        StringFormat format = new StringFormat()
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near
        };

        Font font;
        Font fontBold;
        SolidBrush brush = new SolidBrush(Color.Black);



        internal int MeasureHeight(Badge badge)
        {
#if DEBUG
            if (Environment.OSVersion.Platform != PlatformID.WinCE)
                return listBox.DefaultItemHeight + 10;
#endif
            using (Graphics g = this.listBox.CreateGraphics())
            {
                Rectangle rect = new Rectangle
                        (0,
                         0,
                         listBox.Width - listBox.DefaultItemHeight - (5 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width),
                         listBox.Height);
                Size size1 = Tenor.Mobile.Drawing.Strings.Measure(g, badge.Name, fontBold, rect);
                Size size2 = Tenor.Mobile.Drawing.Strings.Measure(g, badge.Description, font, rect);

                int size = size1.Height * 2 + size2.Height;
                if (size < listBox.DefaultItemHeight)
                    size = listBox.DefaultItemHeight;
                return size;
            }
        }

        private void listBox_DrawItem(object sender, Tenor.Mobile.UI.DrawItemEventArgs e)
        {
            Badge badge = (Badge)e.Item.Value;

            Size factor = Tenor.Mobile.UI.Skin.Current.ScaleFactor;
            if (!string.IsNullOrEmpty(badge.Name))
            {

                int padding = 5 * factor.Width;
                int imageSize = listBox.DefaultItemHeight - padding;

                RectangleF rect = new RectangleF
                    (e.Bounds.X + imageSize + (padding * 2),
                     e.Bounds.Y,
                     e.Bounds.Width - (imageSize) - (padding * 2),
                     e.Bounds.Height);


                if (imageList != null && imageList.ContainsKey(badge.ImageUrl))
                {
                    if (!imageListA.ContainsKey(badge.ImageUrl))
                    {
                        imageListA.Add(badge.ImageUrl, new AlphaImage(new Bitmap(new MemoryStream(imageList[badge.ImageUrl]))));
                        imageList[badge.ImageUrl] = null;
                    }
                    AlphaImage image = imageListA[badge.ImageUrl];

                    Rectangle imgRect =
                        new Rectangle(0 + Convert.ToInt32(padding),
                           Convert.ToInt32(rect.Y + (rect.Height / 2) - (imageSize / 2)), imageSize, imageSize);
                    try
                    {
                        image.Draw(e.Graphics, imgRect);
                    }
                    catch (Exception ex) { Log.RegisterLog(ex); }
                }

                rect.Y += 2 * factor.Height;
                SizeF size = e.Graphics.MeasureString(badge.Name, fontBold);
                e.Graphics.DrawString(
                    badge.Name, fontBold, brush, rect);

                string text = badge.Description;

                rect.Y += size.Height + (3 * factor.Height);
                e.Graphics.DrawString(
                    text, font, brush, rect, format);




                Rectangle rect2 = new Rectangle(
                               e.Bounds.X, e.Bounds.Bottom - 2, e.Bounds.Width / 3, 1);
                Tenor.Mobile.Drawing.GradientFill.Fill(e.Graphics, rect2, this.BackColor, Color.Gray, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
                rect2.X += rect2.Width;
                Tenor.Mobile.Drawing.GradientFill.Fill(e.Graphics, rect2, Color.Gray, Color.Gray, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
                rect2.X += rect2.Width;
                Tenor.Mobile.Drawing.GradientFill.Fill(e.Graphics, rect2, Color.Gray, this.BackColor, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);

            }
        }
    }
}
