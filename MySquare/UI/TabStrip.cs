using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MySquare.UI
{
    partial class TabStrip : Control
    {
        public TabStrip()
        {
            Tabs = new List<string>();
            InitializeComponent();
            this.BackColor = Color.Black;
        }

        public IList<string> Tabs
        { get; private set; }

        public int SelectedIndex
        { get; set; }

        public event EventHandler SelectedIndexChanged;
        private void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);
        }
        
        Pen selectedBorder = new Pen(Tenor.Mobile.Drawing.Strings.ToColor("#DFE6EE"));
        SolidBrush selectedColor = new SolidBrush(Tenor.Mobile.Drawing.Strings.ToColor("#C5CCD4"));

        Pen unselectedBorder = new Pen(Tenor.Mobile.Drawing.Strings.ToColor("#303030"));
        SolidBrush unselectedColor = new SolidBrush(Tenor.Mobile.Drawing.Strings.ToColor("#202020"));

        SolidBrush selectedBrush = new SolidBrush(Color.Black);
        SolidBrush textBrush = new SolidBrush(Color.WhiteSmoke);

        StringFormat format = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
            FormatFlags = StringFormatFlags.NoWrap
        };

        SizeF corner;
        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            corner = new SizeF(8 * factor.Width, 8 * factor.Height);
            base.ScaleControl(factor, specified);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Tabs.Count == 0)
                return;
            float width = this.Width / Tabs.Count;
            for (int i = 0; i < Tabs.Count; i++)
            {
                RectangleF rect = new RectangleF(i * width, 0, width, this.Height);
                if (Rectangle.Round(rect).IntersectsWith(e.ClipRectangle))
                {
                    Rectangle roundedRect = Rectangle.Round(rect);
                    roundedRect.Height += Convert.ToInt32(corner.Height);
                    if (SelectedIndex == i)
                    {
                        Tenor.Mobile.Drawing.RoundedRectangle.Fill(e.Graphics, selectedBorder, selectedColor, roundedRect, corner.ToSize());
                        e.Graphics.DrawString(Tabs[i], Font, selectedBrush, rect, format);
                    }
                    else
                    {
                        Tenor.Mobile.Drawing.RoundedRectangle.Fill(e.Graphics, unselectedBorder, unselectedColor, roundedRect, corner.ToSize());
                        e.Graphics.DrawString(Tabs[i], Font, textBrush, rect, format);
                        e.Graphics.DrawLine(selectedBorder, Convert.ToInt32(rect.X), Convert.ToInt32(rect.Bottom) - 1, Convert.ToInt32(rect.Right), Convert.ToInt32(rect.Bottom) - 1);
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (Enabled && e.Button == MouseButtons.Left)
            {
                float width = this.Width / Tabs.Count;
                for (int i = 0; i < Tabs.Count; i++)
                {
                    Rectangle rect = Rectangle.Round(new RectangleF(i * width, 0, width, this.Height));
                    if (rect.Contains(new Point(e.X, e.Y)))
                    {
                        if (SelectedIndex != i)
                        {
                            SelectedIndex = i;
                            OnSelectedIndexChanged(new EventArgs());
                            this.Invalidate();
                        }
                    }
                }
            }
        }

    }
}
