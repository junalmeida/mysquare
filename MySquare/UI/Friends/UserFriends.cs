using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MySquare.FourSquare;
using MySquare.Service;

namespace MySquare.UI.Friends
{
    partial class UserFriends : UserControl
    {
        public UserFriends()
        {
            InitializeComponent();
            font = new Font(Font.Name, 8, FontStyle.Regular);
            fontBold = new Font(Font.Name, 9, FontStyle.Bold);
        }

        private void listBox_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        internal Dictionary<string, Bitmap> imageList = new Dictionary<string, Bitmap>();
        StringFormat format = new StringFormat()
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near
        }; 
        SolidBrush selectedBrush = new SolidBrush(Color.PaleGoldenrod);
        Pen borderPen = new Pen(Color.White);
        Font font;
        Font fontBold;
        SolidBrush brush = new SolidBrush(Color.Black);
        private void listBox_DrawItem(object sender, Tenor.Mobile.UI.DrawItemEventArgs e)
        {
            User user = (User)e.Item.Value;

            Size factor = Tenor.Mobile.UI.Skin.Current.ScaleFactor;
            if (!string.IsNullOrEmpty(user.ToString()))
            {
                int padding = 5 * factor.Width;
                int imageSize = listBox.DefaultItemHeight - padding;

                RectangleF rect = new RectangleF
                    (e.Bounds.X + imageSize + (padding * 2),
                     e.Bounds.Y,
                     e.Bounds.Width - (imageSize) - (padding * 2),
                     e.Bounds.Height);

                if (e.Item.Selected)
                {
                    Tenor.Mobile.Drawing.RoundedRectangle.Fill(e.Graphics,
                        borderPen, selectedBrush, e.Bounds, new Size(8 * factor.Width, 8 * factor.Height));
                }

                if (imageList != null && imageList.ContainsKey(user.ImageUrl))
                {
                    Bitmap image = imageList[user.ImageUrl];

                    Rectangle imgRect =
                        new Rectangle(0 + Convert.ToInt32(padding),
                           Convert.ToInt32(rect.Y + (rect.Height / 2) - (imageSize / 2)), imageSize, imageSize);
                    try
                    {
                        e.Graphics.DrawImage(image, imgRect, new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                        //image.Draw(e.Graphics, imgRect);
                    }
                    catch (Exception ex) { Log.RegisterLog("gdi", ex); }
                }

                rect.Y += 2 * factor.Height;
                SizeF size = e.Graphics.MeasureString(user.ToString(), fontBold);
                e.Graphics.DrawString(
                    user.ToString(), fontBold, brush, rect);

                if (user.CheckIn != null)
                {
                    string text = string.Empty;
                    if (user.CheckIn.Venue != null)
                        text = "@ " + user.CheckIn.Venue.Name;
                    else
                        text = user.CheckIn.Shout;

                    rect.Y += size.Height + (3 * factor.Height);
                    e.Graphics.DrawString(
                        text, font, brush, rect, format);
                }


                if (!e.Item.Selected)
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

        private void listBox_SelectedItemClicked(object sender, EventArgs e)
        {
            User user = listBox.SelectedItem.Value as User;
            (MySquare.Controller.BaseController.OpenController(this.Parent as UserDetail) as MySquare.Controller.UserController).LoadUser(user);
        }


    }
}
