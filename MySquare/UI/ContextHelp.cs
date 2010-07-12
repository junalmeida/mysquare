using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MySquare.UI
{
    class ContextHelp : Control
    {


        public ContextHelp()
        {
            Visible = false;

        }

        Image bg;
        
        public void ShowHelp(Point target, string text)
        {
            Cursor.Current = Cursors.Default;
            Application.DoEvents();
            bg = Tenor.Mobile.Drawing.GraphicsEx.CopyFromScreen(Parent.CreateGraphics(), new Rectangle(0, 0, Parent.Width, Parent.Height));

            this.BringToFront();
            this.Location = new Point(0, 0);
            this.Size = new Size(Parent.Width, Parent.Height);

            this.Target = target;
            this.Text = text;
            this.Visible = true;
        }

        protected override void OnClick(EventArgs e)
        {
            this.Visible = false;
            base.OnClick(e);
        }


        public Point Target
        { get; set; }

        SolidBrush shadowBrush = new SolidBrush(Color.DarkGray);
        SolidBrush brush = new SolidBrush(Color.WhiteSmoke);
        Pen pen = new Pen(Color.LightGray);
        SolidBrush textBrush = new SolidBrush(Color.Black);
        StringFormat format = new StringFormat()
        {
            Alignment = StringAlignment.Near,
            LineAlignment = StringAlignment.Near,
            FormatFlags = StringFormatFlags.NoClip
        };

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (bg != null)
            {
                e.Graphics.DrawImage(bg, 0, 0);

                int arrowSize = 10 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height;
                int arrowSizeW = 6 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;
                Point[] arrow = new Point[] {
                    new Point(Target.X, Target.Y + arrowSize),
                    Target,
                    new Point(Target.X + arrowSizeW, Target.Y + arrowSize)
                };

                Size size = new Size(
                    130 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width,
                    50 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height);

                size = Tenor.Mobile.Drawing.Strings.Measure(e.Graphics, Text, Font, new Rectangle(0, 0, size.Width, size.Height));
                size.Width += 10 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;
                size.Height += 10 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height;

                Point p = new Point(0, arrow[2].Y);
                p.X = Target.X - size.Width + (arrowSize * 2);
                if (p.X + size.Width > this.Width)
                    p.X = this.Width - size.Width - arrowSize;
                if (p.X < 0)
                    p.X = arrowSizeW;
                p.Y -= 1 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height;

                RectangleF rectF = new RectangleF(p.X, p.Y, size.Width, size.Height);
                Rectangle rect = Rectangle.Round(rectF);


                Rectangle shadowRect = rect;
                shadowRect.Offset(2 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width, 2 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height);
                e.Graphics.FillRectangle(shadowBrush, shadowRect);

                e.Graphics.FillRectangle(brush, rect);
                e.Graphics.DrawRectangle(pen, rect);

                rectF.X += 4 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;
                rectF.Y += 4 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height;
                rectF.Width -= 8 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Width;
                rectF.Height -= 8 * Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height;

                e.Graphics.DrawString(Text, Font, textBrush, rectF, format);

                e.Graphics.FillPolygon(brush, arrow);
                e.Graphics.DrawLines(pen, arrow);
            }
        }
    }
}
