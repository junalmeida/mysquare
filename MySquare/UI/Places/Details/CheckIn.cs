﻿using System;
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

namespace MySquare.UI.Places.Details
{
    internal partial class CheckIn : UserControl
    {
        public CheckIn()
        {
            InitializeComponent();
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
        internal Badge[] badges;
        internal Score[] scoring;
        internal Special[] specials;
        internal Dictionary<string, Image> badgeImageList;
        internal Dictionary<string, Image> scoreImageList;

        SolidBrush textBrush = new SolidBrush(Color.Black);
        StringFormat format = new StringFormat()
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near
        };
        private void pnlCheckInResult_Paint(object sender, PaintEventArgs e)
        {

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

            RoundedRectangle.Fill(e.Graphics, new Pen(Color.Gray), new SolidBrush(Color.White),
                rect, new Size(8 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width, 8 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height));

            Rectangle lastRectangle;

            // -- message 
            lastRectangle = new Rectangle(
                leftWithPadding, rect.Y + padding, widthWithPadding, this.Height);

            Size measure = Tenor.Mobile.Drawing.Strings.Measure(e.Graphics, message, Font, lastRectangle);
            e.Graphics.DrawString(message, Font, textBrush, new RectangleF(lastRectangle.X, lastRectangle.Y, lastRectangle.Width, lastRectangle.Height), format);

            lastRectangle.Height = measure.Height + padding;
            // --

            if (specials != null)
            {
                foreach (var special in specials)
                {

                    int stampSize = 32 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;

                    DrawSeparator(e, padding, ref rectF, ref lastRectangle);

                    AlphaImage image;
                    if (special.Kind == SpecialKind.here)
                        image = new AlphaImage(Resources.SpecialHere);
                    else if (special.Kind == SpecialKind.nearby)
                        image = new AlphaImage(Resources.SpecialNearby);
                    else
                        image = new AlphaImage(Resources.SpecialUnlocked);

                    image.Draw(e.Graphics, new Rectangle(
                        lastRectangle.Right - stampSize - padding, lastRectangle.Top + padding, stampSize, stampSize));

                    Rectangle textRectangle =
                        new Rectangle(
                            lastRectangle.Left, 
                            lastRectangle.Top, 
                            lastRectangle.Width - stampSize - (padding * 2), this.Height);

                    measure = Tenor.Mobile.Drawing.Strings.Measure(e.Graphics, special.Message, Font, textRectangle);
                    e.Graphics.DrawString(special.Message, Font, textBrush, new RectangleF(textRectangle.X, textRectangle.Y, textRectangle.Width, textRectangle.Height), format);


                    lastRectangle.Height = measure.Height + padding;
                }
            }




            if (!string.IsNullOrEmpty(mayorship))
            {
                int crownSize = 32 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;

                DrawSeparator(e, padding, ref rectF, ref lastRectangle);

                AlphaImage image = new AlphaImage(Resources.Crown);
                image.Draw(e.Graphics, new Rectangle(
                    lastRectangle.Right - crownSize - padding, lastRectangle.Top, crownSize, crownSize));

                Rectangle textRectangle = new Rectangle(
                    lastRectangle.Left, lastRectangle.Top, lastRectangle.Width - crownSize - (padding * 2), this.Height);

                measure = Tenor.Mobile.Drawing.Strings.Measure(e.Graphics, mayorship, Font, textRectangle);
                e.Graphics.DrawString(mayorship, Font, textBrush, new RectangleF(textRectangle.X, textRectangle.Y, textRectangle.Width, textRectangle.Height), format);


                lastRectangle.Height = measure.Height + padding;
            }


            if (badges != null)
            {
                foreach (var badge in badges)
                {

                    int stampSize = 32 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;

                    DrawSeparator(e, padding, ref rectF, ref lastRectangle);

                    if (badgeImageList != null && badgeImageList.ContainsKey(badge.ImageUrl))
                    {
                        AlphaImage image = new AlphaImage(badgeImageList[badge.ImageUrl]);
                        image.Draw(e.Graphics, new Rectangle(
                            lastRectangle.Right - stampSize - padding, lastRectangle.Top + padding, stampSize, stampSize));
                    }
                    Rectangle textRectangle =
                        new Rectangle(
                            lastRectangle.Left,
                            lastRectangle.Top,
                            lastRectangle.Width - stampSize - (padding * 2), this.Height);

                    measure = Tenor.Mobile.Drawing.Strings.Measure(e.Graphics, badge.ToString(), Font, textRectangle);
                    e.Graphics.DrawString(badge.ToString(), Font, textBrush, new RectangleF(textRectangle.X, textRectangle.Y, textRectangle.Width, textRectangle.Height), format);


                    lastRectangle.Height = measure.Height + padding;
                }
            }



            if (scoring != null)
            {
                foreach (var score in scoring)
                {

                    int stampSize = 32 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;

                    DrawSeparator(e, padding, ref rectF, ref lastRectangle);

                    if (scoreImageList != null && scoreImageList.ContainsKey(score.ImageUrl))
                    {
                        AlphaImage image = new AlphaImage(scoreImageList[score.ImageUrl]);
                        image.Draw(e.Graphics, new Rectangle(
                            lastRectangle.Right - stampSize - padding, lastRectangle.Top + padding, stampSize, stampSize));
                    }
                    Rectangle textRectangle =
                        new Rectangle(
                            lastRectangle.Left,
                            lastRectangle.Top,
                            lastRectangle.Width - stampSize - (padding * 2), this.Height);

                    measure = Tenor.Mobile.Drawing.Strings.Measure(e.Graphics, score.ToString(), Font, textRectangle);
                    e.Graphics.DrawString(score.ToString(), Font, textBrush, new RectangleF(textRectangle.X, textRectangle.Y, textRectangle.Width, textRectangle.Height), format);


                    lastRectangle.Height = measure.Height + padding;
                }
            }


            if (pnlCheckInResult.Height != lastRectangle.Bottom + padding)
            {
                pnlCheckInResult.Height = lastRectangle.Bottom + padding;
                pnlCheckInResult.Invalidate();
            }

        }

        private void DrawSeparator(PaintEventArgs e, int padding, ref RectangleF rectF, ref Rectangle lastRectangle)
        {
            lastRectangle = new Rectangle(
                lastRectangle.Left, lastRectangle.Bottom, lastRectangle.Width, this.Height);

            e.Graphics.DrawLine(new Pen(Color.Gray),
                Convert.ToInt32(rectF.X), lastRectangle.Top, Convert.ToInt32(rectF.Right) - 1, lastRectangle.Top);

            lastRectangle.Y += padding;
        }
    }
}
