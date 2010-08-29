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
        #region Events
        public event EventHandler SelectedCoordinateChanged;
        protected virtual void OnSelectedCoordinateChanged(EventArgs e)
        {
            if (SelectedCoordinateChanged != null)
                SelectedCoordinateChanged(this, e);
        }
        #endregion

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
            CenterMap();

        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            CreateBackBuffer();
            CenterMap();

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

        #region Map Properties
        private Image centerMark;
        public Image CenterMark
        {
            get { return centerMark; }
            set
            {
                centerMark = value;
                if (this.InvokeRequired)
                {
                    this.Invoke(new ThreadStart(this.ClearTiles));
                    this.Invoke(new ThreadStart(this.Invalidate));
                }
                else
                {
                    ClearTiles();
                    Invalidate();
                }
            }
        }


        int zoom;
        public int Zoom
        {
            get { return zoom; }
            set
            {
                if (value > 22)
                    value = 22;
                if (value < 1)
                    value = 1;
                zoom = value;
                if (this.InvokeRequired)
                    this.Invoke(new ThreadStart(this.ClearTiles));
                else
                    ClearTiles();
            }
        }

        private void ClearTiles()
        {
            if (tiles != null && tiles.Count > 0)
            {
                for (int i = tiles.Count - 1; i >= 0; i--)
                {
                    if (tiles[i].Bitmap != null)
                        tiles[i].Bitmap.Dispose();
                    tiles.RemoveAt(i);
                }
            }
            else
                tiles = new List<MapTile>();
            Invalidate();
        }

        Coordinate mapCenter;
        public Coordinate MapCenter
        {
            get { return mapCenter; }
            set
            {
                mapCenter = value;


                if (this.InvokeRequired)
                    this.Invoke(new ThreadStart(this.CenterMap));
                else
                    CenterMap();
            }
        }

        private void CenterMap()
        {
            Point mapCenterWorld = proj.FromLatLngToPixel(mapCenter.Latitude, mapCenter.Longitude, Zoom);
            Point tile = proj.FromPixelToTileXY(mapCenterWorld);
            tile = proj.FromTileXYToPixel(tile);

            Offset = new Point(
                mapCenterWorld.X - (mapCenterWorld.X - (this.Width / 2) + (mapCenterWorld.X - tile.X)),
                mapCenterWorld.Y - (mapCenterWorld.Y - (this.Height / 2) + (mapCenterWorld.Y - tile.Y))
                );

            Invalidate();
        }


        Coordinate selectedCoordinate;
        public Coordinate SelectedPoint
        {
            get { return selectedCoordinate; }
        }

        MapType mapType;
        public MapType MapType
        {
            get { return mapType; }
            set
            {
                mapType = value;
                if (this.InvokeRequired)
                    this.Invoke(new ThreadStart(this.ClearTiles));
                else
                    ClearTiles();
            }
        }
        #endregion

        GMap.NET.Projections.MercatorProjection proj = new GMap.NET.Projections.MercatorProjection();
        private int TileSize
        {
            get
            {
                return proj.TileSize.Width;   
            }
        }

        #region Paint
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
                DrawMarkers(e);
                Point p = new Point(0, m_backBufferBitmap.Height - Resources.PoweredByGoogle.Height);

                base.OnPaint(new PaintEventArgs(m_backBuffer, e.ClipRectangle));
                m_backBuffer.DrawImage(Resources.PoweredByGoogle, p.X, p.Y);

                e.Graphics.DrawImage(m_backBufferBitmap, 0, 0);
            }
            else
            {
                base.OnPaint(e);
            }
        }

        private void DrawMarkers(PaintEventArgs e)
        {
            if (CenterMark != null)
            {
                Point center = proj.FromLatLngToPixel(MapCenter.Latitude, MapCenter.Longitude, Zoom);
                Point tile = proj.FromPixelToTileXY(center);
                tile = proj.FromTileXYToPixel(tile);

                Point selectedPixel = new Point(
                    Offset.X + (center.X - tile.X),
                    Offset.Y + (center.Y - tile.Y)
                    );
                if (e.ClipRectangle.Contains(selectedPixel))
                {
                    m_backBuffer.DrawImage(
                        CenterMark,
                        selectedPixel.X - (CenterMark.Width / 2),
                        selectedPixel.Y - CenterMark.Height);
                }
            }
        }

        #endregion

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
                if (Offset.Equals(startingOffset) || selectedCoordinate.IsEmpty)
                {
                    Point center = proj.FromLatLngToPixel(MapCenter.Latitude, MapCenter.Longitude, Zoom);
                    Point centerTile = 
                         proj.FromTileXYToPixel(
                            proj.FromPixelToTileXY(center));

                    Point selectedPixel = new Point(
                        centerTile.X - Offset.X + e.X,
                        centerTile.Y - Offset.Y + e.Y
                        );

                    
                    Coordinate newCoord = proj.FromPixelToLatLng(selectedPixel.X, selectedPixel.Y, Zoom);
                    if (!newCoord.Equals(selectedCoordinate))
                    {
                        selectedCoordinate = newCoord;
                        OnSelectedCoordinateChanged(new EventArgs());
                    }
                }
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

        #region Map Tile

        private class MapTile : IDisposable
        {
            MapControl control;
            //int zoom;
            public MapTile(MapControl control, int x, int y)//int zoom)
            {
                this.control = control;
                X = x;
                Y = y;
            }

            bool downloading = false;

            private void Download()
            {
                lock (this)
                {
                    if (downloading)
                        return;
                    else
                        downloading = true;
                }
                Thread t = new Thread(delegate()
                {
                    Coordinate point = control.MapCenter;
                    Point p = control.proj.FromLatLngToPixel(point.Latitude, point.Longitude, control.Zoom);
                    p = control.proj.FromPixelToTileXY(p);
                    p.X += X;
                    p.Y += Y;

                    RisingMobility.Mobile.Location.MapType map = RisingMobility.Mobile.Location.MapType.GoogleMap;
                    if (control.MapType == MapType.Satellite)
                        map = RisingMobility.Mobile.Location.MapType.GoogleSatellite;
                    else if (control.MapType == MapType.Terrain)
                        map = RisingMobility.Mobile.Location.MapType.GoogleTerrain;
                    else if (control.MapType == MapType.Hybrid)
                        map = RisingMobility.Mobile.Location.MapType.GoogleLabels;

                    Bitmap = Imagery.Download(map, p, control.TileSize, control.Zoom);

                    Thread.Sleep(3000);
                    downloading = false;


                });
                lock (this)
                {
                    t.Start();
                }
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
                if (bitmap != null)
                    bitmap.Dispose();
            }

            internal Point GetPosition()
            {
                return new Point(
                    this.X * control.TileSize + control.Offset.X,
                    this.Y * control.TileSize + control.Offset.Y
                    );
            }

            public override string ToString()
            {

                return string.Format("X={0},Y={1}", X, Y);
            }
        }
        #endregion

        public void BeginInit()
        {
        }

        public void EndInit()
        {
        }

    }

    enum MapType
    {
        RoadMap,
        Satellite,
        Terrain,
        Hybrid
    }
}
