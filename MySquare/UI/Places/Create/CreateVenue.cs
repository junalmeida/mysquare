using System;
using System.Drawing;
using System.Windows.Forms;
using MySquare.Properties;
using Tenor.Mobile.Drawing;

namespace MySquare.UI.Places.Create
{
    partial class CreateVenue : UserControl, IView
    {
        public CreateVenue()
        {
            InitializeComponent();
            font = new Font(this.Font.Name, 7, FontStyle.Regular);
        }

        SizeF factor;
        Size ellipse;
        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            this.factor = factor;
            ellipse = new SizeF(8 * factor.Width, 8 * factor.Height).ToSize();
            base.ScaleControl(factor, specified);
        }

        const string helpText = "Tap to fill address";
        Font font;
        SolidBrush brush1 = new SolidBrush(Color.White);
        SolidBrush brush2 = new SolidBrush(Color.Black);

        string fixType;

        internal string FixType
        {
            get { return fixType; }
            set { fixType = value; picMap.Invalidate(); }
        }

        Pen penBorder = new Pen(Color.White);
        AlphaImage pin;
        private void picMap_Paint(object sender, PaintEventArgs e)
        {
            if (picMap.Tag != null && picMap.Tag is byte[])
            {
                try
                {
                    picMap.Tag = new AlphaImage(Main.CreateRoundedAvatar((byte[])picMap.Tag, picMap.Size, factor));
                }
                catch
                {
                    GC.Collect();
                }
            }

            Rectangle picMapRect=new Rectangle(0, 0, picMap.Width, picMap.Height);

            if (picMap.Tag != null && picMap.Tag is AlphaImage)
            {

                string text = string.Format("{0}\r\n{1}", fixType, helpText);
                try
                {
                    ((AlphaImage)picMap.Tag).Draw(e.Graphics,
                        picMapRect);

                    e.Graphics.DrawString(
                        text,
                        font, brush1, 4 * factor.Width, 4 * factor.Height);

                    e.Graphics.DrawString(
                        text,
                        font, brush2, 3 * factor.Width, 3 * factor.Height);

                    if (!selectedPoint.IsEmpty)
                    {
                        Pen p = new Pen(Color.Black);
                        int size = Convert.ToInt32(10 * factor.Width);
                        if (pin == null)
                            pin = new AlphaImage(Resources.Pin);
                        Rectangle rect = new Rectangle(selectedPoint.X - Convert.ToInt32(2 * factor.Width), selectedPoint.Y - size, size, size);
                        pin.Draw(e.Graphics, rect);
                    }
                }
                catch (Exception ex) { MySquare.Service.Log.RegisterLog("gdi", ex); }
            }
            else if (!ellipse.IsEmpty)
            {
                TextureBrush brush = new TextureBrush(Resources.MapBg);
                Tenor.Mobile.Drawing.RoundedRectangle.Fill(e.Graphics, new Pen(Color.Gray), brush, picMapRect, ellipse);
            }
        }

        ~CreateVenue()
        {
            if (picMap.Tag != null && picMap.Tag is IDisposable)
                ((IDisposable)picMap.Tag).Dispose();
            else
                picMap.Tag = null;
        }

        DateTime lastResize = DateTime.MinValue;
        private void CreateVenue_Resize(object sender, EventArgs e)
        {
            if ((DateTime.Now - lastResize).TotalSeconds > 2)
            {
                foreach (Control c in panel1.Controls)
                {
                    if (c.Focused)
                    {
                        this.AutoScrollPosition = new Point(0, c.Top - (c.Height * 2));
                        break;
                    }
                }
                lastResize = DateTime.Now;
            }
        }

        #region Scroll Control
        int originalMouse;
        int originalScroll;
        private void CreateVenue_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originalMouse = e.Y;
                originalScroll = this.AutoScrollPosition.Y;
            }
        }

        private void CreateVenue_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.AutoScrollPosition = new Point(0, (originalMouse - e.Y) - originalScroll); 
            }
        }
        #endregion


        internal double? latitudeCenter;
        internal double? longitudeCenter;
        internal Point selectedPoint = Point.Empty;
        internal double? latitudeSelected;
        internal double? longitudeSelected;
        internal int zoom;
        private void picMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && latitudeCenter.HasValue && longitudeCenter.HasValue)
            {
                selectedPoint = new Point(e.X, e.Y);

                // Retirer les pixels du centre de l'image 
                // à partir le lati longi de la localisation courante 
                double sinLatitudeCenter = Math.Sin(latitudeCenter.Value * Math.PI / 180);
                double pixelXCenter = ((longitudeCenter.Value + 180) / 360) * 256 * Math.Pow(2, zoom);
                double pixelYCenter = (0.5 - Math.Log((1 + sinLatitudeCenter) / (1 - sinLatitudeCenter)) / (4 * Math.PI)) * 256 * Math.Pow(2, zoom);

                //Calculer les coordonnées pixel du coin haut gauche de la carte
                double topLeftPixelX = pixelXCenter - (picMap.Width / 2);
                double topLeftPixelY = pixelYCenter - (picMap.Height / 2);
                Point topLeftCorner = new Point((int)topLeftPixelX, (int)topLeftPixelY);

                // Le deplacement à partir du coin haut gauche
                int x = topLeftCorner.X + selectedPoint.X;
                int y = topLeftCorner.Y + selectedPoint.Y;

                // Convertion au coordonnées Geo
                longitudeSelected = (((double)x * 360) / (256 * Math.Pow(2, zoom))) - 180;
                double efactor = Math.Exp((0.5 - (double)y / 256 / Math.Pow(2, zoom)) * 4 * Math.PI);
                latitudeSelected = Math.Asin((efactor - 1) / (efactor + 1)) * 180 / Math.PI;

                picMap.Invalidate();
            }

        }
    }
}
