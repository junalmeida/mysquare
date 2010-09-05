using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySquare.Properties;

namespace MySquare.UI
{
    partial class MapTest : Form
    {
        public MapTest()
        {
            InitializeComponent();
            mapControl1.CenterMark = Resources.Placemark;
            mapControl1.SelectedMark = Resources.Marker;
            mapControl1.MapCenter =
                new RisingMobility.Mobile.Location.Coordinate()
                {
                    Latitude = -22.856025,
                    Longitude = -43.375182
                };
        }
    }
}