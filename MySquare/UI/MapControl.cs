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
using System.Diagnostics;

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
            Offset = Point.Empty;
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
        int xCenter, yCenter;
        public WorldPoint MapCenter
        {
            get { return mapCenter; }
            set
            {
                mapCenter = value;
                ClearTiles();

                MapTile tile = GetMapAtPoint(
                    new Point(this.Width / 2, this.Height / 2));
                xCenter = tile.X;
                yCenter = tile.Y;

                Invalidate();
            }
        }
           

        private static int TileSize = 100;
        private List<MapTile> tiles = null;
        private MapTile GetMapAtPoint(Point point)
        {
            Point currentPoint =
                new Point(point.X - Offset.X, point.Y - Offset.Y);

            int x = Convert.ToInt32(Math.Floor((double)currentPoint.X / (double)TileSize));
            int y = Convert.ToInt32(Math.Floor((double)currentPoint.Y / (double)TileSize));
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
                for (int y = 0; y <= (this.Height + TileSize); y += TileSize)
                    for (int x = 0; x <= (this.Width + TileSize); x += TileSize)
                    {
                        MapTile tile = GetMapAtPoint(new Point(x, y));
                        if (tile.Bitmap != null)
                        {
                            Point position = tile.GetPosition();

                            m_backBuffer.DrawImage(tile.Bitmap,
                                position.X,
                                position.Y);
                        }
                    }

                e.Graphics.DrawImage(m_backBufferBitmap, 0, 0);
            }
            else
            {
                base.OnPaint(e);
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

        GMap.NET.Projections.LKS94Projection proj = new GMap.NET.Projections.LKS94Projection();

        const string googleMapsUrl =
            "http://maps.google.com/maps/api/staticmap?zoom={2}&sensor=false&mobile=true&format=jpeg&size={0}x{1}&center={3}";



#region Moving tiles
        private Point Offset
        { get; set; }

        Point startingPoint;
        Point startingOffset;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startingPoint = new Point(e.X, e.Y);
                startingOffset = Offset;
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
            Offset = new Point(
                startingOffset.X + ((startingPoint.X - e.X) * -1),
                startingOffset.Y + ((startingPoint.Y - e.Y) * -1)
            );
            this.Invalidate();
        }

#endregion
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
                    WorldPoint point = control.MapCenter;


                    Point p = control.proj.FromLatLngToPixel(point.Latitude, point.Longitude, control.Zoom);
                    Debug.WriteLine(p.ToString());
                    p = control.proj.FromPixelToTileXY(p);
                    Debug.WriteLine(p.ToString());

                    string url = string.Format(
                        googleMapsUrl,
                        TileSize, TileSize + 22, control.Zoom,
                        point
                    );


                    HttpWebResponse response = null;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.UserAgent = UserAgent;
                    try
                    {
                        response = (HttpWebResponse)request.GetResponse();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {

                            Bitmap bmp = new Bitmap(TileSize, TileSize);
                            using (Graphics gx = Graphics.FromImage(bmp))
                            {
                                gx.DrawImage(new Bitmap(response.GetResponseStream()), 0, 0);
#if DEBUG
                                gx.DrawString(string.Format("{0},{1}", X, Y), 
                                    control.Font, new SolidBrush(Color.Black), 0, 0);
#endif
                            }
                            Bitmap = bmp;
                        }
                    }
                    catch
                    {
                        if (response != null)
                            response.Close();

                    }
                    finally
                    {
                        if (response != null)
                            response.Close();
                    }

                    downloading = false;

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

            internal Point GetPosition()
            {
                return new Point(
                    this.X * TileSize + control.Offset.X,
                    this.Y * TileSize + control.Offset.Y
                    );
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

namespace GMap.NET.Projections
{
    using System;

    /// <summary>
    /// GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4326\"]]
    /// PROJCS[\"Lietuvos Koordinoei Sistema 1994\",GEOGCS[\"LKS94 (ETRS89)\",DATUM[\"Lithuania_1994_ETRS89\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],TOWGS84[0,0,0,0,0,0,0],AUTHORITY[\"EPSG\",\"6126\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9108\"]],AUTHORITY[\"EPSG\",\"4126\"]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"latitude_of_origin\",0],PARAMETER[\"central_meridian\",24],PARAMETER[\"scale_factor\",0.9998],PARAMETER[\"false_easting\",500000],PARAMETER[\"false_northing\",0],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AUTHORITY[\"EPSG\",\"2600\"]]
    /// </summary>
    public class LKS94Projection : PureProjection
    {
        const double MinLatitude = 53.33;
        const double MaxLatitude = 56.55;
        const double MinLongitude = 20.22;
        const double MaxLongitude = 27.11;
        const double orignX = 5122000;
        const double orignY = 10000100;

        Size tileSize = new Size(256, 256);
        public override Size TileSize
        {
            get
            {
                return tileSize;
            }
        }

        public override double Axis
        {
            get
            {
                return 6378137;
            }
        }

        public override double Flattening
        {
            get
            {
                return (1.0 / 298.257222101);
            }
        }

        public override Point FromLatLngToPixel(double lat, double lng, int zoom)
        {
            Point ret = Point.Empty;

            lat = Clip(lat, MinLatitude, MaxLatitude);
            lng = Clip(lng, MinLongitude, MaxLongitude);

            double[] lks = new double[] { lng, lat };
            lks = DTM10(lks);
            lks = MTD10(lks);
            lks = DTM00(lks);

            double res = GetTileMatrixResolution(zoom);

            ret.X = (int)Math.Floor((lks[0] + orignX) / res);
            ret.Y = (int)Math.Floor((orignY - lks[1]) / res);

            return ret;
        }

        //public override PointLatLng FromPixelToLatLng(int x, int y, int zoom)
        //{
        //    PointLatLng ret = PointLatLng.Empty;

        //    double res = GetTileMatrixResolution(zoom);

        //    double[] lks = new double[] { (x * res) - orignX, -(y * res) + orignY };
        //    lks = MTD11(lks);
        //    lks = DTM10(lks);
        //    lks = MTD10(lks);

        //    ret.Lat = Clip(lks[1], MinLatitude, MaxLatitude);
        //    ret.Lng = Clip(lks[0], MinLongitude, MaxLongitude);

        //    return ret;
        //}

        double[] DTM10(double[] lonlat)
        {
            double es;              // Eccentricity squared : (a^2 - b^2)/a^2
            double semiMajor = 6378137.0;		// major axis
            double semiMinor = 6356752.3142451793;		// minor axis
            double ab;				// Semi_major / semi_minor
            double ba;				// Semi_minor / semi_major
            double ses;             // Second eccentricity squared : (a^2 - b^2)/b^2

            es = 1.0 - (semiMinor * semiMinor) / (semiMajor * semiMajor); //e^2
            ses = (Math.Pow(semiMajor, 2) - Math.Pow(semiMinor, 2)) / Math.Pow(semiMinor, 2);
            ba = semiMinor / semiMajor;
            ab = semiMajor / semiMinor;

            // ...

            double lon = DegreesToRadians(lonlat[0]);
            double lat = DegreesToRadians(lonlat[1]);
            double h = lonlat.Length < 3 ? 0 : lonlat[2].Equals(Double.NaN) ? 0 : lonlat[2];
            double v = semiMajor / Math.Sqrt(1 - es * Math.Pow(Math.Sin(lat), 2));
            double x = (v + h) * Math.Cos(lat) * Math.Cos(lon);
            double y = (v + h) * Math.Cos(lat) * Math.Sin(lon);
            double z = ((1 - es) * v + h) * Math.Sin(lat);
            return new double[] { x, y, z, };
        }

        double[] MTD10(double[] pnt)
        {
            const double COS_67P5 = 0.38268343236508977;    // cosine of 67.5 degrees 
            const double AD_C = 1.0026000;                  // Toms region 1 constant 

            double es;                             // Eccentricity squared : (a^2 - b^2)/a^2
            double semiMajor = 6378137.0;		    // major axis
            double semiMinor = 6356752.3141403561;	// minor axis
            double ab;				// Semi_major / semi_minor
            double ba;				// Semi_minor / semi_major
            double ses;            // Second eccentricity squared : (a^2 - b^2)/b^2

            es = 1.0 - (semiMinor * semiMinor) / (semiMajor * semiMajor); //e^2
            ses = (Math.Pow(semiMajor, 2) - Math.Pow(semiMinor, 2)) / Math.Pow(semiMinor, 2);
            ba = semiMinor / semiMajor;
            ab = semiMajor / semiMinor;

            // ...

            bool AtPole = false; // is location in polar region
            double Z = pnt.Length < 3 ? 0 : pnt[2].Equals(Double.NaN) ? 0 : pnt[2];

            double lon = 0;
            double lat = 0;
            double Height = 0;
            if (pnt[0] != 0.0)
            {
                lon = Math.Atan2(pnt[1], pnt[0]);
            }
            else
            {
                if (pnt[1] > 0)
                {
                    lon = Math.PI / 2;
                }
                else
                    if (pnt[1] < 0)
                    {
                        lon = -Math.PI * 0.5;
                    }
                    else
                    {
                        AtPole = true;
                        lon = 0.0;
                        if (Z > 0.0) // north pole
                        {
                            lat = Math.PI * 0.5;
                        }
                        else
                            if (Z < 0.0) // south pole
                            {
                                lat = -Math.PI * 0.5;
                            }
                            else // center of earth
                            {
                                return new double[] { RadiansToDegrees(lon), RadiansToDegrees(Math.PI * 0.5), -semiMinor, };
                            }
                    }
            }
            double W2 = pnt[0] * pnt[0] + pnt[1] * pnt[1]; // Square of distance from Z axis
            double W = Math.Sqrt(W2); // distance from Z axis
            double T0 = Z * AD_C; // initial estimate of vertical component
            double S0 = Math.Sqrt(T0 * T0 + W2); // initial estimate of horizontal component
            double Sin_B0 = T0 / S0; // sin(B0), B0 is estimate of Bowring aux variable
            double Cos_B0 = W / S0; // cos(B0)
            double Sin3_B0 = Math.Pow(Sin_B0, 3);
            double T1 = Z + semiMinor * ses * Sin3_B0; // corrected estimate of vertical component
            double Sum = W - semiMajor * es * Cos_B0 * Cos_B0 * Cos_B0; // numerator of cos(phi1)
            double S1 = Math.Sqrt(T1 * T1 + Sum * Sum); // corrected estimate of horizontal component
            double Sin_p1 = T1 / S1; // sin(phi1), phi1 is estimated latitude
            double Cos_p1 = Sum / S1; // cos(phi1)
            double Rn = semiMajor / Math.Sqrt(1.0 - es * Sin_p1 * Sin_p1); // Earth radius at location
            if (Cos_p1 >= COS_67P5)
            {
                Height = W / Cos_p1 - Rn;
            }
            else
                if (Cos_p1 <= -COS_67P5)
                {
                    Height = W / -Cos_p1 - Rn;
                }
                else
                {
                    Height = Z / Sin_p1 + Rn * (es - 1.0);
                }

            if (!AtPole)
            {
                lat = Math.Atan(Sin_p1 / Cos_p1);
            }
            return new double[] { RadiansToDegrees(lon), RadiansToDegrees(lat), Height, };
        }

        double[] DTM00(double[] lonlat)
        {
            double scaleFactor = 0.9998;	                // scale factor				
            double centralMeridian = 0.41887902047863912;	// Center longitude (projection center) */
            double latOrigin = 0.0;	                // center latitude			
            double falseNorthing = 0.0;	            // y offset in meters			
            double falseEasting = 500000.0;	        // x offset in meters			
            double semiMajor = 6378137.0;		        // major axis
            double semiMinor = 6356752.3141403561;		// minor axis
            double metersPerUnit = 1.0;

            double e0, e1, e2, e3;	// eccentricity constants		
            double e, es, esp;		// eccentricity constants		
            double ml0;		    // small value m			

            es = 1.0 - Math.Pow(semiMinor / semiMajor, 2);
            e = Math.Sqrt(es);
            e0 = e0fn(es);
            e1 = e1fn(es);
            e2 = e2fn(es);
            e3 = e3fn(es);
            ml0 = semiMajor * mlfn(e0, e1, e2, e3, latOrigin);
            esp = es / (1.0 - es);

            // ...		

            double lon = DegreesToRadians(lonlat[0]);
            double lat = DegreesToRadians(lonlat[1]);

            double delta_lon = 0.0;  // Delta longitude (Given longitude - center)
            double sin_phi, cos_phi; // sin and cos value				
            double al, als;		  // temporary values				
            double c, t, tq;	      // temporary values				
            double con, n, ml;	      // cone constant, small m			

            delta_lon = AdjustLongitude(lon - centralMeridian);
            SinCos(lat, out sin_phi, out cos_phi);

            al = cos_phi * delta_lon;
            als = Math.Pow(al, 2);
            c = esp * Math.Pow(cos_phi, 2);
            tq = Math.Tan(lat);
            t = Math.Pow(tq, 2);
            con = 1.0 - es * Math.Pow(sin_phi, 2);
            n = semiMajor / Math.Sqrt(con);
            ml = semiMajor * mlfn(e0, e1, e2, e3, lat);

            double x = scaleFactor * n * al * (1.0 + als / 6.0 * (1.0 - t + c + als / 20.0 *
                (5.0 - 18.0 * t + Math.Pow(t, 2) + 72.0 * c - 58.0 * esp))) + falseEasting;

            double y = scaleFactor * (ml - ml0 + n * tq * (als * (0.5 + als / 24.0 *
                (5.0 - t + 9.0 * c + 4.0 * Math.Pow(c, 2) + als / 30.0 * (61.0 - 58.0 * t
                + Math.Pow(t, 2) + 600.0 * c - 330.0 * esp))))) + falseNorthing;

            if (lonlat.Length < 3)
                return new double[] { x / metersPerUnit, y / metersPerUnit };
            else
                return new double[] { x / metersPerUnit, y / metersPerUnit, lonlat[2] };
        }

        double[] DTM01(double[] lonlat)
        {
            double es;                             // Eccentricity squared : (a^2 - b^2)/a^2
            double semiMajor = 6378137.0;		    // major axis
            double semiMinor = 6356752.3141403561;	// minor axis
            double ab;				                // Semi_major / semi_minor
            double ba;				                // Semi_minor / semi_major
            double ses;                            // Second eccentricity squared : (a^2 - b^2)/b^2

            es = 1.0 - (semiMinor * semiMinor) / (semiMajor * semiMajor);
            ses = (Math.Pow(semiMajor, 2) - Math.Pow(semiMinor, 2)) / Math.Pow(semiMinor, 2);
            ba = semiMinor / semiMajor;
            ab = semiMajor / semiMinor;

            // ...

            double lon = DegreesToRadians(lonlat[0]);
            double lat = DegreesToRadians(lonlat[1]);
            double h = lonlat.Length < 3 ? 0 : lonlat[2].Equals(Double.NaN) ? 0 : lonlat[2];
            double v = semiMajor / Math.Sqrt(1 - es * Math.Pow(Math.Sin(lat), 2));
            double x = (v + h) * Math.Cos(lat) * Math.Cos(lon);
            double y = (v + h) * Math.Cos(lat) * Math.Sin(lon);
            double z = ((1 - es) * v + h) * Math.Sin(lat);
            return new double[] { x, y, z, };
        }

        double[] MTD01(double[] pnt)
        {
            const double COS_67P5 = 0.38268343236508977; // cosine of 67.5 degrees 
            const double AD_C = 1.0026000;               // Toms region 1 constant 

            double es;                             // Eccentricity squared : (a^2 - b^2)/a^2
            double semiMajor = 6378137.0;		    // major axis
            double semiMinor = 6356752.3142451793;	// minor axis
            double ab;		                        // Semi_major / semi_minor
            double ba;				                // Semi_minor / semi_major
            double ses;                            // Second eccentricity squared : (a^2 - b^2)/b^2

            es = 1.0 - (semiMinor * semiMinor) / (semiMajor * semiMajor);
            ses = (Math.Pow(semiMajor, 2) - Math.Pow(semiMinor, 2)) / Math.Pow(semiMinor, 2);
            ba = semiMinor / semiMajor;
            ab = semiMajor / semiMinor;

            // ...

            bool At_Pole = false; // is location in polar region
            double Z = pnt.Length < 3 ? 0 : pnt[2].Equals(Double.NaN) ? 0 : pnt[2];

            double lon = 0;
            double lat = 0;
            double Height = 0;
            if (pnt[0] != 0.0)
            {
                lon = Math.Atan2(pnt[1], pnt[0]);
            }
            else
            {
                if (pnt[1] > 0)
                {
                    lon = Math.PI / 2;
                }
                else
                    if (pnt[1] < 0)
                    {
                        lon = -Math.PI * 0.5;
                    }
                    else
                    {
                        At_Pole = true;
                        lon = 0.0;
                        if (Z > 0.0) // north pole
                        {
                            lat = Math.PI * 0.5;
                        }
                        else
                            if (Z < 0.0) // south pole
                            {
                                lat = -Math.PI * 0.5;
                            }
                            else // center of earth
                            {
                                return new double[] { RadiansToDegrees(lon), RadiansToDegrees(Math.PI * 0.5), -semiMinor, };
                            }
                    }
            }

            double W2 = pnt[0] * pnt[0] + pnt[1] * pnt[1]; // Square of distance from Z axis
            double W = Math.Sqrt(W2);                      // distance from Z axis
            double T0 = Z * AD_C;                // initial estimate of vertical component
            double S0 = Math.Sqrt(T0 * T0 + W2); //initial estimate of horizontal component
            double Sin_B0 = T0 / S0;             // sin(B0), B0 is estimate of Bowring aux variable
            double Cos_B0 = W / S0;              // cos(B0)
            double Sin3_B0 = Math.Pow(Sin_B0, 3);
            double T1 = Z + semiMinor * ses * Sin3_B0; //corrected estimate of vertical component
            double Sum = W - semiMajor * es * Cos_B0 * Cos_B0 * Cos_B0; // numerator of cos(phi1)
            double S1 = Math.Sqrt(T1 * T1 + Sum * Sum); // corrected estimate of horizontal component
            double Sin_p1 = T1 / S1;  // sin(phi1), phi1 is estimated latitude
            double Cos_p1 = Sum / S1; // cos(phi1)
            double Rn = semiMajor / Math.Sqrt(1.0 - es * Sin_p1 * Sin_p1); // Earth radius at location

            if (Cos_p1 >= COS_67P5)
            {
                Height = W / Cos_p1 - Rn;
            }
            else
                if (Cos_p1 <= -COS_67P5)
                {
                    Height = W / -Cos_p1 - Rn;
                }
                else
                {
                    Height = Z / Sin_p1 + Rn * (es - 1.0);
                }

            if (!At_Pole)
            {
                lat = Math.Atan(Sin_p1 / Cos_p1);
            }
            return new double[] { RadiansToDegrees(lon), RadiansToDegrees(lat), Height, };
        }

        double[] MTD11(double[] p)
        {
            double scaleFactor = 0.9998;	                // scale factor				
            double centralMeridian = 0.41887902047863912;	// Center longitude (projection center) 
            double latOrigin = 0.0;	                   // center latitude			
            double falseNorthing = 0.0;	        // y offset in meters			
            double falseEasting = 500000.0;	    // x offset in meters			
            double semiMajor = 6378137.0;		    // major axis
            double semiMinor = 6356752.3141403561;	// minor axis
            double metersPerUnit = 1.0;

            double e0, e1, e2, e3;	// eccentricity constants		
            double e, es, esp;		// eccentricity constants		
            double ml0;		    // small value m

            es = 1.0 - Math.Pow(semiMinor / semiMajor, 2);
            e = Math.Sqrt(es);
            e0 = e0fn(es);
            e1 = e1fn(es);
            e2 = e2fn(es);
            e3 = e3fn(es);
            ml0 = semiMajor * mlfn(e0, e1, e2, e3, latOrigin);
            esp = es / (1.0 - es);

            // ...

            double con, phi;
            double delta_phi;
            long i;
            double sin_phi, cos_phi, tan_phi;
            double c, cs, t, ts, n, r, d, ds;
            long max_iter = 6;

            double x = p[0] * metersPerUnit - falseEasting;
            double y = p[1] * metersPerUnit - falseNorthing;

            con = (ml0 + y / scaleFactor) / semiMajor;
            phi = con;
            for (i = 0; ; i++)
            {
                delta_phi = ((con + e1 * Math.Sin(2.0 * phi) - e2 * Math.Sin(4.0 * phi) + e3 * Math.Sin(6.0 * phi)) / e0) - phi;
                phi += delta_phi;

                if (Math.Abs(delta_phi) <= EPSLoN)
                    break;

                if (i >= max_iter)
                    throw new ArgumentException("Latitude failed to converge");
            }

            if (Math.Abs(phi) < HALF_PI)
            {
                SinCos(phi, out sin_phi, out cos_phi);
                tan_phi = Math.Tan(phi);
                c = esp * Math.Pow(cos_phi, 2);
                cs = Math.Pow(c, 2);
                t = Math.Pow(tan_phi, 2);
                ts = Math.Pow(t, 2);
                con = 1.0 - es * Math.Pow(sin_phi, 2);
                n = semiMajor / Math.Sqrt(con);
                r = n * (1.0 - es) / con;
                d = x / (n * scaleFactor);
                ds = Math.Pow(d, 2);

                double lat = phi - (n * tan_phi * ds / r) * (0.5 - ds / 24.0 * (5.0 + 3.0 * t +
                    10.0 * c - 4.0 * cs - 9.0 * esp - ds / 30.0 * (61.0 + 90.0 * t +
                    298.0 * c + 45.0 * ts - 252.0 * esp - 3.0 * cs)));

                double lon = AdjustLongitude(centralMeridian + (d * (1.0 - ds / 6.0 * (1.0 + 2.0 * t +
                    c - ds / 20.0 * (5.0 - 2.0 * c + 28.0 * t - 3.0 * cs + 8.0 * esp +
                    24.0 * ts))) / cos_phi));

                if (p.Length < 3)
                    return new double[] { RadiansToDegrees(lon), RadiansToDegrees(lat) };
                else
                    return new double[] { RadiansToDegrees(lon), RadiansToDegrees(lat), p[2] };
            }
            else
            {
                if (p.Length < 3)
                    return new double[] { RadiansToDegrees(HALF_PI * Sign(y)), RadiansToDegrees(centralMeridian) };
                else
                    return new double[] { RadiansToDegrees(HALF_PI * Sign(y)), RadiansToDegrees(centralMeridian), p[2] };
            }
        }

        /// <summary>
        /// Clips a number to the specified minimum and maximum values.
        /// </summary>
        /// <param name="n">The number to clip.</param>
        /// <param name="minValue">Minimum allowable value.</param>
        /// <param name="maxValue">Maximum allowable value.</param>
        /// <returns>The clipped value.</returns>
        double Clip(double n, double minValue, double maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }

        #region -- levels info --
        //dojo.io.script.jsonp_dojoIoScript1._jsonpCallback({"serviceDescription":"",
        //      "mapName":"map","description":"","copyrightText":"",
        //      "layers":[{"id":0,"name":"lietuva_MAPSLT_DB_GB200_01_P_APSK",
        //      "parentLayerId":-1,"defaultVisibility":true,"subLayerIds":null}],
        //      "spatialReference":{"wkid":2600},"singleFusedMapCache":true,
        //      "tileInfo":{"rows":256,"cols":256,"dpi":96,"format":"PNG8","compressionQuality":0,
        //      "origin":{"x":-5122000,"y":10000100},"spatialReference":{"wkid":2600},
        //      "lods":[
        //{"level":0,"resolution":1587.50317500635,"scale":6000000},
        //{"level":1,"resolution":793.751587503175,"scale":3000000},
        //{"level":2,"resolution":529.167725002117,"scale":2000000},
        //{"level":3,"resolution":264.583862501058,"scale":1000000},
        //{"level":4,"resolution":132.291931250529,"scale":500000},
        //{"level":5,"resolution":52.9167725002117,"scale":200000},
        //{"level":6,"resolution":26.4583862501058,"scale":100000},
        //{"level":7,"resolution":13.2291931250529,"scale":50000},
        //{"level":8,"resolution":6.61459656252646,"scale":25000},
        //{"level":9,"resolution":2.64583862501058,"scale":10000},
        //{"level":10,"resolution":1.32291931250529,"scale":5000},
        //{"level":11,"resolution":0.529167725002117,"scale":2000}]},

        //"initialExtent":{"xmin":-42686.481789127,"ymin":5746881.05416859,
        //                 "xmax":1029393.00578913,"ymax":6484161.30783141,
        //"spatialReference":{"wkid":2600}},
        //"fullExtent":{"xmin":-42686.481789127,"ymin":5746881.05416859,
        //              "xmax":1029393.00578913,"ymax":6484161.30783141,
        //"spatialReference":{"wkid":2600}},"units":"esriMeters",

        //"supportedImageFormatTypes":"PNG24,PNG,JPG,DIB,TIFF,EMF,PS,PDF,GIF,SVG,SVGZ,AI",
        //"documentInfo":{"Title":"mapslt_minimal","Author":"gstanevicius","Comments":"","Subject":"","Category":"","Keywords":""}}); 
        #endregion

        public double GetTileMatrixResolution(int zoom)
        {
            double ret = 0;

            switch (zoom)
            {
                #region -- sizes --
                case 0:
                    {
                        ret = 1587.50317500635;
                    }
                    break;

                case 1:
                    {
                        ret = 793.751587503175;
                    }
                    break;

                case 2:
                    {
                        ret = 529.167725002117;
                    }
                    break;

                case 3:
                    {
                        ret = 264.583862501058;
                    }
                    break;

                case 4:
                    {
                        ret = 132.291931250529;
                    }
                    break;

                case 5:
                    {
                        ret = 52.9167725002117;
                    }
                    break;

                case 6:
                    {
                        ret = 26.4583862501058;
                    }
                    break;

                case 7:
                    {
                        ret = 13.2291931250529;
                    }
                    break;

                case 8:
                    {
                        ret = 6.61459656252646;
                    }
                    break;

                case 9:
                    {
                        ret = 2.64583862501058;
                    }
                    break;

                case 10:
                    {
                        ret = 1.32291931250529;
                    }
                    break;

                case 11:
                    {
                        ret = 0.529167725002117;
                    }
                    break;
                #endregion
            }

            return ret;
        }

        public override double GetGroundResolution(int zoom, double latitude)
        {
            return GetTileMatrixResolution(zoom);
        }

        public override Size GetTileMatrixMinXY(int zoom)
        {
            Size ret = Size.Empty;

            switch (zoom)
            {
                #region -- sizes --
                case 0:
                    {
                        ret = new Size(12, 8);
                    }
                    break;

                case 1:
                    {
                        ret = new Size(24, 17);
                    }
                    break;

                case 2:
                    {
                        ret = new Size(37, 25);
                    }
                    break;

                case 3:
                    {
                        ret = new Size(74, 51);
                    }
                    break;

                case 4:
                    {
                        ret = new Size(149, 103);
                    }
                    break;

                case 5:
                    {
                        ret = new Size(374, 259);
                    }
                    break;

                case 6:
                    {
                        ret = new Size(749, 519);
                    }
                    break;

                case 7:
                    {
                        ret = new Size(1594, 1100);
                    }
                    break;

                case 8:
                    {
                        ret = new Size(3188, 2201);
                    }
                    break;

                case 9:
                    {
                        ret = new Size(7971, 5502);
                    }
                    break;

                case 10:
                    {
                        ret = new Size(15943, 11005);
                    }
                    break;

                case 11:
                    {
                        ret = new Size(39858, 27514);
                    }
                    break;
                #endregion
            }

            return ret;
        }

        public override Size GetTileMatrixMaxXY(int zoom)
        {
            Size ret = Size.Empty;

            switch (zoom)
            {
                #region -- sizes --
                case 0:
                    {
                        ret = new Size(14, 10);
                    }
                    break;

                case 1:
                    {
                        ret = new Size(30, 20);
                    }
                    break;

                case 2:
                    {
                        ret = new Size(45, 31);
                    }
                    break;

                case 3:
                    {
                        ret = new Size(90, 62);
                    }
                    break;

                case 4:
                    {
                        ret = new Size(181, 125);
                    }
                    break;

                case 5:
                    {
                        ret = new Size(454, 311);
                    }
                    break;

                case 6:
                    {
                        ret = new Size(903, 623);
                    }
                    break;

                case 7:
                    {
                        ret = new Size(1718, 1193);
                    }
                    break;

                case 8:
                    {
                        ret = new Size(3437, 2386);
                    }
                    break;

                case 9:
                    {
                        ret = new Size(8594, 5966);
                    }
                    break;

                case 10:
                    {
                        ret = new Size(17189, 11932);
                    }
                    break;

                case 11:
                    {
                        ret = new Size(42972, 29831);
                    }
                    break;
                #endregion
            }

            return ret;
        }
    }
}



namespace GMap.NET
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// defines projection
    /// </summary>
    public abstract class PureProjection
    {
        /// <summary>
        /// size of tile
        /// </summary>
        public abstract Size TileSize
        {
            get;
        }

        /// <summary>
        /// Semi-major axis of ellipsoid, in meters
        /// </summary>
        public abstract double Axis
        {
            get;
        }

        /// <summary>
        /// Flattening of ellipsoid
        /// </summary>
        public abstract double Flattening
        {
            get;
        }

        /// <summary>
        /// get pixel coordinates from lat/lng
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public abstract Point FromLatLngToPixel(double lat, double lng, int zoom);

        ///// <summary>
        ///// gets lat/lng coordinates from pixel coordinates
        ///// </summary>
        ///// <param name="x"></param>
        ///// <param name="y"></param>
        ///// <param name="zoom"></param>
        ///// <returns></returns>
        //public abstract PointLatLng FromPixelToLatLng(int x, int y, int zoom);

        ///// <summary>
        ///// get pixel coordinates from lat/lng
        ///// </summary>
        ///// <param name="p"></param>
        ///// <param name="zoom"></param>
        ///// <returns></returns>
        //public Point FromLatLngToPixel(PointLatLng p, int zoom)
        //{
        //    return FromLatLngToPixel(p.Lat, p.Lng, zoom);
        //}

        ///// <summary>
        ///// gets lat/lng coordinates from pixel coordinates
        ///// </summary>
        ///// <param name="p"></param>
        ///// <param name="zoom"></param>
        ///// <returns></returns>
        //public PointLatLng FromPixelToLatLng(Point p, int zoom)
        //{
        //    return FromPixelToLatLng(p.X, p.Y, zoom);
        //}

        /// <summary>
        /// gets tile coorddinate from pixel coordinates
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual Point FromPixelToTileXY(Point p)
        {
            return new Point((int)(p.X / TileSize.Width), (int)(p.Y / TileSize.Height));
        }

        /// <summary>
        /// gets pixel coordinate from tile coordinate
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual Point FromTileXYToPixel(Point p)
        {
            return new Point((p.X * TileSize.Width), (p.Y * TileSize.Height));
        }

        /// <summary>
        /// min. tile in tiles at custom zoom level
        /// </summary>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public abstract Size GetTileMatrixMinXY(int zoom);

        /// <summary>
        /// max. tile in tiles at custom zoom level
        /// </summary>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public abstract Size GetTileMatrixMaxXY(int zoom);

        /// <summary>
        /// gets matrix size in tiles
        /// </summary>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public virtual Size GetTileMatrixSizeXY(int zoom)
        {
            Size sMin = GetTileMatrixMinXY(zoom);
            Size sMax = GetTileMatrixMaxXY(zoom);

            return new Size(sMax.Width - sMin.Width + 1, sMax.Height - sMin.Height + 1);
        }

        /// <summary>
        /// tile matrix size in pixels at custom zoom level
        /// </summary>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public int GetTileMatrixItemCount(int zoom)
        {
            Size s = GetTileMatrixSizeXY(zoom);
            return (s.Width * s.Height);
        }

        /// <summary>
        /// gets matrix size in pixels
        /// </summary>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public virtual Size GetTileMatrixSizePixel(int zoom)
        {
            Size s = GetTileMatrixSizeXY(zoom);
            return new Size(s.Width * TileSize.Width, s.Height * TileSize.Height);
        }

        ///// <summary>
        ///// gets all tiles in rect at specific zoom
        ///// </summary>
        //public List<Point> GetAreaTileList(RectLatLng rect, int zoom, int padding)
        //{
        //    List<Point> ret = new List<Point>();

        //    Point topLeft = FromPixelToTileXY(FromLatLngToPixel(rect.LocationTopLeft, zoom));
        //    Point rightBottom = FromPixelToTileXY(FromLatLngToPixel(rect.LocationRightBottom, zoom));

        //    for (int x = (topLeft.X - padding); x <= (rightBottom.X + padding); x++)
        //    {
        //        for (int y = (topLeft.Y - padding); y <= (rightBottom.Y + padding); y++)
        //        {
        //            Point p = new Point(x, y);
        //            if (!ret.Contains(p) && p.X >= 0 && p.Y >= 0)
        //            {
        //                ret.Add(p);
        //            }
        //        }
        //    }
        //    ret.TrimExcess();

        //    return ret;
        //}

        /// <summary>
        /// The ground resolution indicates the distance (in meters) on the ground that’s represented by a single pixel in the map.
        /// For example, at a ground resolution of 10 meters/pixel, each pixel represents a ground distance of 10 meters.
        /// </summary>
        /// <param name="zoom"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public virtual double GetGroundResolution(int zoom, double latitude)
        {
            return (Math.Cos(latitude * (Math.PI / 180)) * 2 * Math.PI * Axis) / GetTileMatrixSizePixel(zoom).Width;
        }

        #region -- math functions --

        /// <summary>
        /// PI
        /// </summary>
        protected const double PI = Math.PI;

        /// <summary>
        /// Half of PI
        /// </summary>
        protected const double HALF_PI = (PI * 0.5);

        /// <summary>
        /// PI * 2
        /// </summary>
        protected const double TWO_PI = (PI * 2.0);

        /// <summary>
        /// EPSLoN
        /// </summary>
        protected const double EPSLoN = 1.0e-10;

        /// <summary>
        /// MAX_VAL
        /// </summary>
        protected const double MAX_VAL = 4;

        /// <summary>
        /// MAXLONG
        /// </summary>
        protected const double MAXLONG = 2147483647;

        /// <summary>
        /// DBLLONG
        /// </summary>
        protected const double DBLLONG = 4.61168601e18;

        const double R2D = 180 / Math.PI;
        const double D2R = Math.PI / 180;

        public double DegreesToRadians(double deg)
        {
            return (D2R * deg);
        }

        public double RadiansToDegrees(double rad)
        {
            return (R2D * rad);
        }

        ///<summary>
        /// return the sign of an argument 
        ///</summary>
        protected static double Sign(double x)
        {
            if (x < 0.0)
                return (-1);
            else
                return (1);
        }

        protected static double AdjustLongitude(double x)
        {
            long count = 0;
            while (true)
            {
                if (Math.Abs(x) <= PI)
                    break;
                else
                    if (((long)Math.Abs(x / Math.PI)) < 2)
                        x = x - (Sign(x) * TWO_PI);
                    else
                        if (((long)Math.Abs(x / TWO_PI)) < MAXLONG)
                        {
                            x = x - (((long)(x / TWO_PI)) * TWO_PI);
                        }
                        else
                            if (((long)Math.Abs(x / (MAXLONG * TWO_PI))) < MAXLONG)
                            {
                                x = x - (((long)(x / (MAXLONG * TWO_PI))) * (TWO_PI * MAXLONG));
                            }
                            else
                                if (((long)Math.Abs(x / (DBLLONG * TWO_PI))) < MAXLONG)
                                {
                                    x = x - (((long)(x / (DBLLONG * TWO_PI))) * (TWO_PI * DBLLONG));
                                }
                                else
                                    x = x - (Sign(x) * TWO_PI);
                count++;
                if (count > MAX_VAL)
                    break;
            }
            return (x);
        }

        /// <summary>
        /// calculates the sine and cosine
        /// </summary>
        protected static void SinCos(double val, out double sin, out double cos)
        {
            sin = Math.Sin(val);
            cos = Math.Cos(val);
        }

        /// <summary>
        /// computes the constants e0, e1, e2, and e3 which are used
        /// in a series for calculating the distance along a meridian.
        /// </summary>
        /// <param name="x">represents the eccentricity squared</param>
        /// <returns></returns>
        protected static double e0fn(double x)
        {
            return (1.0 - 0.25 * x * (1.0 + x / 16.0 * (3.0 + 1.25 * x)));
        }

        protected static double e1fn(double x)
        {
            return (0.375 * x * (1.0 + 0.25 * x * (1.0 + 0.46875 * x)));
        }

        protected static double e2fn(double x)
        {
            return (0.05859375 * x * x * (1.0 + 0.75 * x));
        }

        protected static double e3fn(double x)
        {
            return (x * x * x * (35.0 / 3072.0));
        }

        /// <summary>
        /// computes the value of M which is the distance along a meridian
        /// from the Equator to latitude phi.
        /// </summary>
        protected static double mlfn(double e0, double e1, double e2, double e3, double phi)
        {
            return (e0 * phi - e1 * Math.Sin(2.0 * phi) + e2 * Math.Sin(4.0 * phi) - e3 * Math.Sin(6.0 * phi));
        }

        /// <summary>
        /// calculates UTM zone number
        /// </summary>
        /// <param name="lon">Longitude in degrees</param>
        /// <returns></returns>
        protected static long GetUTMzone(double lon)
        {
            return ((long)(((lon + 180.0) / 6.0) + 1.0));
        }

        #endregion

        /// <summary>
        /// Conversion from cartesian earth-sentered coordinates to geodetic coordinates in the given datum
        /// </summary>
        /// <param name="Lat"></param>
        /// <param name="Lon"></param>
        /// <param name="Height">Height above ellipsoid [m]</param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        public void FromGeodeticToCartesian(double Lat, double Lng, double Height, out double X, out double Y, out double Z)
        {
            Lat = (Math.PI / 180) * Lat;
            Lng = (Math.PI / 180) * Lng;

            double B = Axis * (1.0 - Flattening);
            double ee = 1.0 - (B / Axis) * (B / Axis);
            double N = (Axis / Math.Sqrt(1.0 - ee * Math.Sin(Lat) * Math.Sin(Lat)));

            X = (N + Height) * Math.Cos(Lat) * Math.Cos(Lng);
            Y = (N + Height) * Math.Cos(Lat) * Math.Sin(Lng);
            Z = (N * (B / Axis) * (B / Axis) + Height) * Math.Sin(Lat);
        }

        /// <summary>
        /// Conversion from cartesian earth-sentered coordinates to geodetic coordinates in the given datum
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <param name="Lat"></param>
        /// <param name="Lon"></param>
        public void FromCartesianTGeodetic(double X, double Y, double Z, out double Lat, out double Lng)
        {
            double E = Flattening * (2.0 - Flattening);
            Lng = Math.Atan2(Y, X);

            double P = Math.Sqrt(X * X + Y * Y);
            double Theta = Math.Atan2(Z, (P * (1.0 - Flattening)));
            double st = Math.Sin(Theta);
            double ct = Math.Cos(Theta);
            Lat = Math.Atan2(Z + E / (1.0 - Flattening) * Axis * st * st * st, P - E * Axis * ct * ct * ct);

            Lat /= (Math.PI / 180);
            Lng /= (Math.PI / 180);
        }
    }
}
