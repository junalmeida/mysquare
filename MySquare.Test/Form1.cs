﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RisingMobility.Mobile.Location;

namespace MySquare.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Zoom = 16;
            pictureBox1.CenterMark = imageList1.Images[0];
            pictureBox1.MapCenter = new RisingMobility.Mobile.Location.Coordinate()
            {
                Latitude = -22.909204,
                Longitude = -43.180563
            };
            numericUpDown1.Value = pictureBox1.Zoom;
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Geolocation geo = Geolocation.Get(pictureBox1.SelectedPoint);
            if (geo != null)
                label1.Text = geo.ToString();
            else
                label1.Text = " - ";
            Clipboard.SetText(pictureBox1.SelectedPoint.ToString());


        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            pictureBox1.Zoom = Convert.ToInt32(numericUpDown1.Value);
        }
    }
}
