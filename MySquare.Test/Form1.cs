using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySquare.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            pictureBox1.MapCenter = new RisingMobility.Mobile.Location.WorldPoint()
            {
                Latitude = -22.909204,
                Longitude = -43.180563
            };

        }
    }
}
