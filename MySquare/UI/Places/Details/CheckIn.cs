using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Tenor.Mobile.Drawing;
using MySquare.FourSquare;
using MySquare.Properties;
using MySquare.Service;

namespace MySquare.UI.Places.Details
{
    internal partial class CheckIn : UserControl
    {
        public CheckIn()
        {
            InitializeComponent();

            Color bgColor = Tenor.Mobile.Drawing.Strings.ToColor("#C5CCD4");
            BackColor = bgColor;
            txtShout.BackColor = bgColor;
        }

        private void txtShout_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void chkTellFriends_CheckStateChanged(object sender, EventArgs e)
        {
            if (!chkTellFriends.Checked)
            {
                chkTwitter.Enabled = false;
                chkFacebook.Enabled = false;
                chkTwitter.CheckState = CheckState.Unchecked;
                chkFacebook.CheckState = CheckState.Unchecked;
            }
            else
            {
                chkTwitter.Enabled = true;
                chkFacebook.Enabled = true;
                chkTwitter.CheckState = CheckState.Indeterminate;
                chkFacebook.CheckState = CheckState.Indeterminate;
            }
        }

        internal void Activate()
        {
            Dock = DockStyle.Fill;
            BringToFront();
            Visible = true;
        }

        internal string message;
        internal string mayorship;
        internal bool showCrown;
        internal BadgeNotification[] badges;
        internal ScoreNotification[] scoring;
        internal SpecialNotification[] specials;
        internal Dictionary<string, Bitmap> badgeImageList;
        internal Dictionary<string, Bitmap> scoreImageList;




        SolidBrush textBrush = new SolidBrush(Color.Black);
        StringFormat format = new StringFormat()
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near
        };

        Bitmap backBuffer = null;
        Graphics backBufferG = null;

        private void pnlCheckInResult_Resize(object sender, EventArgs e)
        {
            ResetBackBuffer();
        }

        private void ResetBackBuffer()
        {
            if (backBuffer != null)
                backBuffer.Dispose();
            if (backBufferG != null)
                backBufferG.Dispose();

            backBuffer = new Bitmap(pnlCheckInResult.Width, pnlCheckInResult.Height);
            backBufferG = Graphics.FromImage(backBuffer);
            backBufferG.Clear(pnlCheckInResult.BackColor);
        }

        private void pnlCheckInResult_Paint(object sender, PaintEventArgs e)
        {
            if (backBuffer == null)
                ResetBackBuffer();

            e.Graphics.DrawImage(backBuffer, 0, 0);

            Graphics graphics = backBufferG;
            graphics.Clear(pnlCheckInResult.BackColor);

            int padding = 4 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;
            RectangleF rectF =
                new RectangleF(
                        padding,
                        padding,
                        pnlCheckInResult.Width - (padding * 2),
                        pnlCheckInResult.Height - (padding * 2)
                    );

            Rectangle rect = Rectangle.Round(rectF);

            int leftWithPadding = rect.X + padding;
            int widthWithPadding = Convert.ToInt32(rect.Width - (padding * 2.5));

            RoundedRectangle.Fill(graphics, new Pen(Color.Gray), new SolidBrush(Color.White),
                rect, new Size(8 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width, 8 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height));

            Rectangle lastRectangle;

            // -- message 
            lastRectangle = new Rectangle(
                leftWithPadding, rect.Y + padding, widthWithPadding, this.Height);

            Size measure = Tenor.Mobile.Drawing.Strings.Measure(graphics, message, Font, lastRectangle);
            graphics.DrawString(message, Font, textBrush, new RectangleF(lastRectangle.X, lastRectangle.Y, lastRectangle.Width, lastRectangle.Height), format);

            lastRectangle.Height = measure.Height + padding;
            // --

            if (specials != null)
            {
                foreach (var special in specials)
                {
                    if (special.Kind == SpecialKind.nearby)
                        continue; //TODO: Show specials nearby on check ins.

                    int stampSize = 31 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;

                    DrawSeparator(graphics, padding, ref rectF, ref lastRectangle);

                    AlphaImage image;
                    if (special.Kind == SpecialKind.here)
                        image = new AlphaImage(Resources.SpecialHere);
                    else if (special.Kind == SpecialKind.nearby)
                        image = new AlphaImage(Resources.SpecialNearby);
                    else
                        image = new AlphaImage(Resources.SpecialUnlocked);


                    Rectangle textRectangle =
                        new Rectangle(
                            lastRectangle.Left, 
                            lastRectangle.Top, 
                            lastRectangle.Width - stampSize - (padding * 2), this.Height);

                    measure = Tenor.Mobile.Drawing.Strings.Measure(graphics, special.Message, Font, textRectangle);
                    graphics.DrawString(special.Message, Font, textBrush, new RectangleF(textRectangle.X, textRectangle.Y, textRectangle.Width, textRectangle.Height), format);


                    if (measure.Height < stampSize)
                        measure.Height = stampSize;
                    try
                    {
                        image.Draw(graphics, new Rectangle(
                            lastRectangle.Right - stampSize - padding,
                            lastRectangle.Top + ((measure.Height / 2) - (stampSize / 2)), stampSize, stampSize));
                    }
                    catch (Exception ex)
                    {
                        Log.RegisterLog("gdi", ex);
                    }

                    lastRectangle.Height = measure.Height + padding;
                }
            }




            if (!string.IsNullOrEmpty(mayorship))
            {
                int crownSize = 0;
                if (showCrown)
                    crownSize = 31 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;

                DrawSeparator(graphics, padding, ref rectF, ref lastRectangle);


                Rectangle textRectangle = new Rectangle(
                    lastRectangle.Left, lastRectangle.Top, lastRectangle.Width - crownSize - (padding * 2), this.Height);

                measure = Tenor.Mobile.Drawing.Strings.Measure(graphics, mayorship, Font, textRectangle);
                graphics.DrawString(mayorship, Font, textBrush, new RectangleF(textRectangle.X, textRectangle.Y, textRectangle.Width, textRectangle.Height), format);

                if (measure.Height < crownSize)
                    measure.Height = crownSize;

                if (showCrown)
                {
                    try
                    {
                        AlphaImage image = new AlphaImage(Resources.Crown);
                        image.Draw(graphics, new Rectangle(
                            lastRectangle.Right - crownSize - padding,
                            lastRectangle.Top + ((measure.Height / 2) - (crownSize / 2)), crownSize, crownSize));
                    }
                    catch (Exception ex)
                    {
                        Log.RegisterLog("gdi", ex);
                    }
                }
                lastRectangle.Height = measure.Height + padding;
            }


            if (badges != null)
            {
                foreach (var badge in badges)
                {

                    int stampSize = 31 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;

                    DrawSeparator(graphics, padding, ref rectF, ref lastRectangle);


                    Rectangle textRectangle =
                        new Rectangle(
                            lastRectangle.Left,
                            lastRectangle.Top,
                            lastRectangle.Width - stampSize - (padding * 2), this.Height);

                    measure = Tenor.Mobile.Drawing.Strings.Measure(graphics, badge.ToString(), Font, textRectangle);
                    graphics.DrawString(badge.ToString(), Font, textBrush, new RectangleF(textRectangle.X, textRectangle.Y, textRectangle.Width, textRectangle.Height), format);

                    if (measure.Height < stampSize)
                        measure.Height = stampSize;

                    if (badgeImageList != null && badgeImageList.ContainsKey(badge.ImageUrl))
                    {
                        try
                        {
                            AlphaImage image = new AlphaImage(badgeImageList[badge.ImageUrl]);
                            image.Draw(graphics, new Rectangle(
                                lastRectangle.Right - stampSize - padding,
                                lastRectangle.Top + ((measure.Height / 2) - (stampSize / 2)), stampSize, stampSize));
                        }
                        catch (Exception ex)
                        {
                            Log.RegisterLog("gdi", ex);
                        }
                    }

                    lastRectangle.Height = measure.Height + padding;
                }
            }



            if (scoring != null)
            {
                foreach (var scoreCol in scoring)
                    foreach (var score in scoreCol)
                    {

                        int stampSize = 16 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;

                        DrawSeparator(graphics, padding, ref rectF, ref lastRectangle);

                        Rectangle textRectangle =
                            new Rectangle(
                                lastRectangle.Left,
                                lastRectangle.Top,
                                lastRectangle.Width - stampSize - (padding * 2), this.Height);

                        measure = Tenor.Mobile.Drawing.Strings.Measure(graphics, score.ToString(), Font, textRectangle);
                        graphics.DrawString(score.ToString(), Font, textBrush, new RectangleF(textRectangle.X, textRectangle.Y, textRectangle.Width, textRectangle.Height), format);

                        if (measure.Height < stampSize)
                            measure.Height = stampSize;

                        if (scoreImageList != null && scoreImageList.ContainsKey(score.ImageUrl))
                        {
                            try
                            {
                                AlphaImage image = new AlphaImage(scoreImageList[score.ImageUrl]);
                                image.Draw(graphics, new Rectangle(
                                    lastRectangle.Right - stampSize - padding,
                                    lastRectangle.Top + ((measure.Height / 2) - (stampSize / 2)), stampSize, stampSize));
                            }
                            catch (Exception ex)
                            {
                                Log.RegisterLog("gdi", ex);
                            }
                        }

                        lastRectangle.Height = measure.Height + padding;
                    }
            }

            e.Graphics.DrawImage(backBuffer, 0, 0);

            if (pnlCheckInResult.Height != lastRectangle.Bottom + padding)
            {
                pnlCheckInResult.Height = lastRectangle.Bottom + padding;
                pnlCheckInResult.Invalidate();
            }
        }

        private void DrawSeparator(Graphics graphics, int padding, ref RectangleF rectF, ref Rectangle lastRectangle)
        {
            lastRectangle = new Rectangle(
                lastRectangle.Left, lastRectangle.Bottom, lastRectangle.Width, this.Height);

            graphics.DrawLine(new Pen(Color.Gray),
                Convert.ToInt32(rectF.X), lastRectangle.Top, Convert.ToInt32(rectF.Right) - 1, lastRectangle.Top);

            lastRectangle.Y += padding;
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
