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
            tabStrip.Tabs.Add("My Friends");
            tabStrip.Tabs.Add("All");
            tabStrip.SelectedIndex = 0;
            tabStrip_SelectedIndexChanged(null, new EventArgs());
            font = new Font(Font.Name, 7, FontStyle.Regular);
            fontBold = new Font(Font.Name, 7, FontStyle.Bold);

            lstFriends.BackColor = Tenor.Mobile.Drawing.Strings.ToColor("#C5CCD4");
            lstAll.BackColor = lstFriends.BackColor;
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


        void tabStrip_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (tabStrip.SelectedIndex == 0)
            {
                lstFriends.Visible = true;
                lstAll.Visible = false;
            }
            else
            {
                lstFriends.Visible = false;
                lstAll.Visible = true;
            }
        }


        Font font;
        Font fontBold;
        StringFormat format = new StringFormat()
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near
        };
        SolidBrush brush = new SolidBrush(Color.Black);
        SolidBrush selectedBrush = new SolidBrush(Color.PaleGoldenrod);
        Pen borderPen = new Pen(Color.White);

        

        void lstAll_DrawItem(object sender, Tenor.Mobile.UI.DrawItemEventArgs e)
        {
            var listBox = (Tenor.Mobile.UI.KListControl)sender;
            LeaderboardUser user = (LeaderboardUser)e.Item.Value;



            int padding = 5 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;
            int imageSize = Convert.ToInt32(listBox.DefaultItemHeight - padding);

            RectangleF rect = new RectangleF
                (e.Bounds.X + (padding * 2),
                 e.Bounds.Y,
                 e.Bounds.Width - (padding * 2),
                 e.Bounds.Height);
            //if (user.Self)
            //{
            //    Tenor.Mobile.Drawing.RoundedRectangle.Fill(e.Graphics,
            //        borderPen, selectedBrush, e.Bounds, new Size(
            //            8 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width,
            //            8 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height));
            //}

            string text = (e.Item.YIndex + 1).ToString() + ". " + user.User.ToString();
            e.Graphics.DrawString(
               text, fontBold, brush, rect);

            SizeF size = e.Graphics.MeasureString(text, fontBold);
            rect.X += size.Width + padding;
            e.Graphics.DrawString(
                user.Points, font, brush, rect, format);

            rect = new RectangleF
                (e.Bounds.X + (padding * 2),
                 e.Bounds.Y + size.Height + (padding / 2),
                 e.Bounds.Width - (padding * 4),
                 10 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height);

            Color color = Tenor.Mobile.Drawing.Strings.ToColor("#DCE0E4");

            Size ell = new Size(
                8*Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width,
                8*Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height);
            RoundedRectangle.Fill(e.Graphics, new Pen(color), new SolidBrush(color), Rectangle.Round(rect), ell);
            //e.Graphics.FillRectangle(new SolidBrush(Tenor.Mobile.Drawing.Strings.ToColor("#DCE0E4")), Rectangle.Round(rect));

            color = Color.Blue;
            if (user.Self)
                color = Color.Red;

            rect.Width =
                rect.Width * user.Percentage / 100;

            RoundedRectangle.Fill(e.Graphics, new Pen(color), new SolidBrush(color), Rectangle.Round(rect), ell);
            //e.Graphics.FillRectangle(new SolidBrush(color), Rectangle.Round(rect));

            //if (!user.Self)
            //{
            //    Rectangle rect2 = new Rectangle(
            //                   e.Bounds.X, e.Bounds.Bottom - 2, e.Bounds.Width / 3, 1);

            //    e.Graphics.DrawSeparator(rect2, Color.Gray);
            //    Tenor.Mobile.Drawing.GradientFill.Fill(e.Graphics, rect2, this.BackColor, Color.Gray, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
            //    rect2.X += rect2.Width;
            //    Tenor.Mobile.Drawing.GradientFill.Fill(e.Graphics, rect2, Color.Gray, Color.Gray, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
            //    rect2.X += rect2.Width;
            //    Tenor.Mobile.Drawing.GradientFill.Fill(e.Graphics, rect2, Color.Gray, this.BackColor, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
            //}
        }


    }
}
