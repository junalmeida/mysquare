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
using MySquare.FourSquare;

namespace MySquare.UI.More
{
    partial class Leaderboard : UserControl, IView
    {
        public Leaderboard()
        {
            InitializeComponent();
            //tabStrip.Tabs.Add("My Friends");
            //tabStrip.Tabs.Add("All");
            //tabStrip.SelectedIndex = 0;
            font = new Font(Font.Name, 7, FontStyle.Regular);
            medFont = new Font(Font.Name, 10, FontStyle.Bold);
            bigFont = new Font(Font.Name, 12, FontStyle.Bold);
            fontBold = new Font(Font.Name, 7, FontStyle.Bold);

            lstAll.BackColor = Tenor.Mobile.Drawing.Strings.ToColor("#C5CCD4");
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

        Font font;
        Font bigFont;
        Font medFont;
        Font fontBold;
        StringFormat format = new StringFormat()
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near
        };
        SolidBrush brush = new SolidBrush(Color.Black);
        SolidBrush selectedBrush = new SolidBrush(Color.PaleGoldenrod);
        Pen borderPen = new Pen(Color.White);

        Dictionary<string, byte[]> imageList;
        Dictionary<string, AlphaImage> imageListBuffer;
        internal Dictionary<string, byte[]> ImageList
        {
            get { return imageList; }
            set
            {
                lstAll.Clear();
                imageList = value;
                imageListBuffer.ClearImageList();
                imageListBuffer = new Dictionary<string, AlphaImage>();
            }
        }

        void lstAll_DrawItem(object sender, Tenor.Mobile.UI.DrawItemEventArgs e)
        {
            var listBox = (Tenor.Mobile.UI.KListControl)sender;
            LeaderboardUser lboard = (LeaderboardUser)e.Item.Value;

            User user = lboard.User;

            string rankText = "#" + lboard.Rank.ToString();
            SizeF rankSize = e.Graphics.MeasureString(rankText, medFont);
            RectangleF rankRect = new RectangleF(
                3 * factorF.Width,
                e.Bounds.Y + (e.Bounds.Height / 2 - rankSize.Height / 2),
                rankSize.Width,
                rankSize.Height
            );

            Size factor = Tenor.Mobile.UI.Skin.Current.ScaleFactor;
            if (!string.IsNullOrEmpty(user.ToString()))
            {
                int padding = 5 * factor.Width;
                int imageSize = listBox.DefaultItemHeight - padding;

                RectangleF rect = new RectangleF
                    (imageSize + (padding * 2) + rankRect.Right,
                     e.Bounds.Y,
                     e.Bounds.Width - (imageSize) - (padding * 2),
                     e.Bounds.Height);

                if (user.FriendStatus != null && user.FriendStatus.Value == FriendStatus.self)
                {
                    Tenor.Mobile.Drawing.RoundedRectangle.Fill(e.Graphics,
                        borderPen, selectedBrush, e.Bounds, new Size(8 * factor.Width, 8 * factor.Height));
                }
                e.Graphics.DrawString(rankText, medFont, brush, rankRect);

                if (imageList != null && imageList.ContainsKey(user.ImageUrl))
                {
                    if (!imageListBuffer.ContainsKey(user.ImageUrl))
                    {
                        imageListBuffer.Add(user.ImageUrl, new AlphaImage(Main.CreateRoundedAvatar(imageList[user.ImageUrl], imageSize, factorF)));
                    }
                    AlphaImage image = imageListBuffer[user.ImageUrl];

                    Rectangle imgRect =
                        new Rectangle(Convert.ToInt32(rankRect.Right + padding),
                           Convert.ToInt32(rect.Y + (rect.Height / 2) - (imageSize / 2)), imageSize, imageSize);
                    try
                    {
                        //e.Graphics.DrawImage(image, imgRect, new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                        image.Draw(e.Graphics, imgRect);
                    }
                    catch (Exception ex) { Log.RegisterLog("gdi", ex); }
                }

                rect.Y += 2 * factor.Height;
                SizeF size = e.Graphics.MeasureString(user.ToString(), fontBold);
                e.Graphics.DrawString(
                    user.ToString(), fontBold, brush, rect);

                if (lboard.Scores != null)
                {
                    string text = "7-day high score: ";
                    text += lboard.Scores.Max.ToString();


                    rect.Y += size.Height + (3 * factor.Height);
                    e.Graphics.DrawString(
                        text, font, brush, rect, format);


                    var scoreText = lboard.Scores.Recent.ToString();
                    SizeF sizeScore = e.Graphics.MeasureString(scoreText, bigFont);
                    rect = new RectangleF(
                        e.Bounds.Width - sizeScore.Width - (10 * factorF.Width),
                        e.Bounds.Y + (e.Bounds.Height /2 - sizeScore.Height/2),
                        sizeScore.Width,sizeScore.Height
                        );

                    e.Graphics.DrawString(
                        scoreText, bigFont, brush, rect, format);

                }


                if (user.FriendStatus != null && user.FriendStatus.Value != FriendStatus.self)
                {
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

        SizeF factorF;
        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            factorF = factor;
            base.ScaleControl(factor, specified);
        }

    }
}
