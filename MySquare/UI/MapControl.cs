using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using RisingMobility.Mobile.Location;
using MySquare.Properties;
using System.Threading;
using System.Net;
using System.IO;

namespace RisingMobility.Mobile.UI
{
    class MapControl : Control,System.ComponentModel.ISupportInitialize
    {

        public MapControl()
        {
            Zoom = 16;
            if (Tenor.Mobile.UI.Skin.Current.ScaleFactor.Height < 2)
                Zoom = 15;
        }

        #region BackBuffer


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CreateBackBuffer();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            CreateBackBuffer();
        }


        Bitmap m_backBufferBitmap;
        Graphics m_backBuffer;
        /// <summary>
        /// Cleans up the background paint buffer.
        /// </summary>
        private void CleanupBackBuffer()
        {
            if (m_backBufferBitmap != null)
            {
                m_backBufferBitmap.Dispose();
                m_backBufferBitmap = null;
                m_backBuffer.Dispose();
                m_backBuffer = null;
            }
        }
        /// <summary>
        /// Creates the background paint buffer.
        /// </summary>
        private void CreateBackBuffer()
        {
            CleanupBackBuffer();
            if (this.Width > 0 && this.Height > 0)
            {
                m_backBufferBitmap = new Bitmap(this.Width, this.Height);
                m_backBuffer = Graphics.FromImage(m_backBufferBitmap);
            }

            if (matrix != null)
            {
                for (int i = 0; i < matrix.GetUpperBound(0); i++)
                    for (int j = 0; j < matrix.GetUpperBound(1); j++)
                        if (matrix[i, j] != null)
                            matrix[i, j].Dispose();
            }
            DownloadTiles();
        }
        #endregion


        int zoom;
        public int Zoom
        { get { return zoom; } set { zoom = value; Invalidate(); } }

        WorldPoint mapCenter;

        public WorldPoint MapCenter
        { get { return mapCenter; } set { mapCenter = value; Invalidate(); } }
           

        private static int TileSize = 100;
        private MapTile[,] matrix;
            
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do nothing

        }

        TextureBrush bgBrush = new TextureBrush(Resources.MapBg);
        protected override void OnPaint(PaintEventArgs e)
        {
            if (m_backBuffer != null)
            {
                m_backBuffer.FillRectangle(bgBrush, 0, 0, m_backBufferBitmap.Width, m_backBufferBitmap.Height);
                base.OnPaint(new PaintEventArgs(m_backBuffer, e.ClipRectangle));

                if (matrix != null)
                    for (int i = 0; i < matrix.GetUpperBound(0); i++)
                        for (int j = 0; j < matrix.GetUpperBound(1); j++)
                        {
                            if (matrix[i, j].Bitmap != null)
                                m_backBuffer.DrawImage(matrix[i, j].Bitmap, i * TileSize, j * TileSize);

                        }

                e.Graphics.DrawImage(m_backBufferBitmap, 0, 0);
            }
            else
            {
                base.OnPaint(e);
            }
        }

        private void DownloadTiles()
        {
            int xCount = Convert.ToInt32(Math.Floor(this.Width / TileSize)) + 1;
            int yCount = Convert.ToInt32(Math.Floor(this.Height / TileSize)) + 1;
            matrix = new MapTile[xCount, yCount];
            for (int i = 0; i < matrix.GetUpperBound(0); i++)
                for (int j = 0; j < matrix.GetUpperBound(1); j++)
                {
                    matrix[i, j] = new MapTile(MapCenter, Zoom);
                }
        }


        private WorldPoint ToWorldPoint(Point selectedPoint)
        {

            // Retirer les pixels du centre de l'image 
            // à partir le lati longi de la localisation courante 
            double sinLatitudeCenter = Math.Sin(MapCenter.Latitude * Math.PI / 180);
            double pixelXCenter = ((MapCenter.Longitude + 180) / 360) * 256 * Math.Pow(2, zoom);
            double pixelYCenter = (0.5 - Math.Log((1 + sinLatitudeCenter) / (1 - sinLatitudeCenter)) / (4 * Math.PI)) * 256 * Math.Pow(2, zoom);

            //Calculer les coordonnées pixel du coin haut gauche de la carte
            double topLeftPixelX = pixelXCenter - (this.Width / 2);
            double topLeftPixelY = pixelYCenter - (this.Height / 2);
            Point topLeftCorner = new Point((int)topLeftPixelX, (int)topLeftPixelY);

            // Le deplacement à partir du coin haut gauche
            int x = topLeftCorner.X + selectedPoint.X;
            int y = topLeftCorner.Y + selectedPoint.Y;

            // Convertion au coordonnées Geo
            double longitudeSelected = (((double)x * 360) / (256 * Math.Pow(2, zoom))) - 180;
            double efactor = Math.Exp((0.5 - (double)y / 256 / Math.Pow(2, zoom)) * 4 * Math.PI);
            double latitudeSelected = Math.Asin((efactor - 1) / (efactor + 1)) * 180 / Math.PI;

            return new WorldPoint()
            {
                Latitude = latitudeSelected,
                Longitude = longitudeSelected
            };
        }


        const string googleMapsUrl = "http://maps.google.com/maps/api/staticmap?zoom={2}&sensor=false&mobile=true&format=jpeg&size={0}x{1}&center={3}";
        const string markers = "&markers=color:blue|{2},{3}";
        private class MapTile : IDisposable
        {
            WorldPoint relative;
            int zoom;
            public MapTile(WorldPoint relative, int zoom)
            {
                this.relative = relative;
                this.zoom = zoom;
                Download();
            }

            bool downloading = false;
            const string EmptyTileText = "We are sorry, but we don't\nhave imagery at this zoom\nlevel for this region.";

            private void Download()
            {
                if (downloading)
                    return;
                downloading = true;
                Thread t = new Thread(delegate()
                {
                    string url = string.Format(googleMapsUrl,
    TileSize, TileSize, zoom,
    relative.Latitude.ToString());


                    HttpWebResponse response = null;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    try
                    {
                        response = (HttpWebResponse)request.GetResponse();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Bitmap = new Bitmap(response.GetResponseStream());
                        }
                    }
                    catch
                    {
                        //Bitmap = new Bitmap(TileSize, TileSize);


                    }
                    finally
                    {
                        if (response != null)
                            response.Close();
                        downloading = false;
                    }
                });
                t.Start();
            }

            public Bitmap Bitmap { get; private set; }

            public void Dispose()
            {
                if (Bitmap != null)
                    Bitmap.Dispose();
            }

        }





        #region ISupportInitialize Members

        public void BeginInit()
        {
        }

        public void EndInit()
        {
        }

        #endregion
    }
}
