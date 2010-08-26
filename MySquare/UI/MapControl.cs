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


        }
        #endregion


        int zoom;
        public int Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                ClearTiles();
                Invalidate();
            }
        }

        private void ClearTiles()
        {
            if (tiles != null && tiles.Count > 0)
            {
                for (int i = tiles.Count; i >= 0; i--)
                {
                    if (tiles[i].Bitmap != null)
                        tiles[i].Bitmap.Dispose();
                    tiles.RemoveAt(i);
                }
            }
            else
                tiles = new List<MapTile>();
        }

        WorldPoint mapCenter;
        public WorldPoint MapCenter
        { get { return mapCenter; } set { mapCenter = value; ClearTiles(); Invalidate(); } }
           

        private static int TileSize = 100;
        private List<MapTile> tiles = null;
        private MapTile GetMapAtPoint(Point point)
        {
            int x = Convert.ToInt32(Math.Floor((double)point.X / (double)TileSize));
            int y = Convert.ToInt32(Math.Floor((double)point.Y / (double)TileSize));
            foreach (MapTile tile in tiles)
            {
                if (tile.X == x && tile.Y == y)
                    return tile;
            }
            var t = new MapTile(this, x, y);
            tiles.Add(t);
            return t;
        }

            
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
                for (int y = 0; y < this.Height; y += TileSize)
                    for (int x = 0; x < this.Width; x += TileSize)
                    {
                        MapTile tile = GetMapAtPoint(new Point(x, y));
                        if (tile.Bitmap != null)
                            m_backBuffer.DrawImage(tile.Bitmap, x + offset.X, y + offset.Y);
                    }

                e.Graphics.DrawImage(m_backBufferBitmap, 0, 0);
            }
            else
            {
                base.OnPaint(e);
            }
        }


        //private static WorldPoint ToWorldPoint(Point selectedPoint)
        //{

        //    // Retirer les pixels du centre de l'image 
        //    // à partir le lati longi de la localisation courante 
        //    double sinLatitudeCenter = Math.Sin(MapCenter.Latitude * Math.PI / 180);
        //    double pixelXCenter = ((MapCenter.Longitude + 180) / 360) * 256 * Math.Pow(2, zoom);
        //    double pixelYCenter = (0.5 - Math.Log((1 + sinLatitudeCenter) / (1 - sinLatitudeCenter)) / (4 * Math.PI)) * 256 * Math.Pow(2, zoom);

        //    //Calculer les coordonnées pixel du coin haut gauche de la carte
        //    double topLeftPixelX = pixelXCenter - (this.Width / 2);
        //    double topLeftPixelY = pixelYCenter - (this.Height / 2);
        //    Point topLeftCorner = new Point((int)topLeftPixelX, (int)topLeftPixelY);

        //    // Le deplacement à partir du coin haut gauche
        //    int x = topLeftCorner.X + selectedPoint.X;
        //    int y = topLeftCorner.Y + selectedPoint.Y;

        //    // Convertion au coordonnées Geo
        //    double longitudeSelected = (((double)x * 360) / (256 * Math.Pow(2, zoom))) - 180;
        //    double efactor = Math.Exp((0.5 - (double)y / 256 / Math.Pow(2, zoom)) * 4 * Math.PI);
        //    double latitudeSelected = Math.Asin((efactor - 1) / (efactor + 1)) * 180 / Math.PI;

        //    return new WorldPoint()
        //    {
        //        Latitude = latitudeSelected,
        //        Longitude = longitudeSelected
        //    };
        //}


        const string googleMapsUrl =
            "http://maps.google.com/maps/api/staticmap?zoom={2}&sensor=false&mobile=true&format=jpeg&size={0}x{1}&center={3}";

        private class MapTile : IDisposable
        {
            MapControl control;
            //int zoom;
            public MapTile(MapControl control, int x, int y)//int zoom)
            {
                this.control = control;
                X = x;
                Y = y;
                //this.zoom = zoom;
                //Download();
            }

            bool downloading = false;
            /// <summary>
            /// Gets or sets the value of the User-agent HTTP header.
            /// </summary>
            const string UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.1.7) Gecko/20091221 Firefox/3.5.7";

            private void Download()
            {
                if (downloading)
                    return;
                downloading = true;
                Thread t = new Thread(delegate()
                {
                    string url = string.Format(
                        googleMapsUrl,
                        TileSize, TileSize + 22, control.Zoom,
                        control.MapCenter.ToString()
                    );
                    int count = 0;
                    while (true)
                    {


                        HttpWebResponse response = null;
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.UserAgent = UserAgent;
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
                            count++;
                            if (count >= 3)
                            {
                                //error logo
                                Bitmap = Resources.Gps;
                            }
                            else
                            {
                                if (response != null)
                                    response.Close();
                                continue;
                            }
                        }
                        finally
                        {
                            if (response != null)
                                response.Close();
                        }

                        downloading = false;
                        break;
                    }
                });
                t.Start();
            }

            public int X { get; private set; }
            public int Y { get; private set; }
            private Bitmap bitmap;
            public Bitmap Bitmap
            {
                get
                {
                    if (bitmap == null)
                        Download();
                    return bitmap;
                }
                private set
                {
                    bitmap = value; control.Invoke(new ThreadStart(control.Invalidate));
                }
            }

            public void Dispose()
            {
                if (Bitmap != null)
                    Bitmap.Dispose();
            }
        }


#region Moving tiles
        Point offset = Point.Empty;
        Point startingPoint;
        Point startingOffset;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startingPoint = new Point(e.X, e.Y);
                startingOffset = offset;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UpdateOffset(e);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UpdateOffset(e);
            }
            base.OnMouseUp(e);
        }

        private void UpdateOffset(MouseEventArgs e)
        {
            offset = new Point(
                startingOffset.X + ((startingPoint.X - e.X) * -1),
                startingOffset.Y + ((startingPoint.Y - e.Y) * -1)
            );
            this.Invalidate();
        }

#endregion

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
