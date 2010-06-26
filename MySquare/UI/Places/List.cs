using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySquare.Properties;
using MySquare.FourSquare;
using System.Threading;

namespace MySquare.UI.Places
{
    public partial class List : UserControl
    {
        public List()
        {
            InitializeComponent();

            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);
            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(lblError);

            Program.Position.LocationChanged += new EventHandler(Position_LocationChanged);
            Program.Service.SearchArrives += new MySquare.FourSquare.SearchEventHandler(Service_SearchArrives);
            Program.Service.Error += new ErrorEventHandler(Service_Error);
        }


        void Service_Error(object serder, ErrorEventArgs e)
        {
            this.Invoke(new System.Threading.ThreadStart(delegate()
            {
                ShowError("Cannot connect to foursquare.");
            }));
        }



        internal void Refresh()
        {
            Application.DoEvents();
            ShowLoading();
            Program.Position.PollCell();
        }

        void Position_LocationChanged(object sender, EventArgs e)
        {
            if (Program.Position.Latitude.HasValue && Program.Position.Longitude.HasValue)
            {
                Program.Service.SearchNearby(null, Program.Position.Latitude.Value, Program.Position.Longitude.Value);
            }
            else
            {
                ShowError("Cannot check your location, try again.");
            }
        }


        void Service_SearchArrives(object serder, MySquare.FourSquare.SearchEventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(new ThreadStart(delegate()
                {
                    LoadVenues(e.Venues);
                }));
            else
                LoadVenues(e.Venues);
        }



        private void ShowLoading()
        {
            listBox.Visible = false;
            lblError.Visible = false;
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
        }

        private void ShowError(string text)
        {
            listBox.Visible = false;
            lblError.Text = text;
            lblError.Visible = true;
            Cursor.Current = Cursors.Default;
            Cursor.Show();
        }

        private void ShowList()
        {
            listBox.Visible = true;
            lblError.Visible = false;
            Cursor.Current = Cursors.Default;
            Cursor.Show();
        }




        void LoadVenues(Venue[] venues)
        {
            listBox.Clear();
            foreach (Venue venue in venues)
            {
                listBox.AddItem(venue.Name, venue);
            }
            ShowList();
        }

        Brush brush = new SolidBrush(Tenor.Mobile.UI.Skin.Current.TextForeColor);
        Brush brushS = new SolidBrush(Tenor.Mobile.UI.Skin.Current.TextHighLight);
        StringFormat format = new StringFormat()
        {
             Alignment = StringAlignment.Near,
             LineAlignment = StringAlignment.Near
        };

        private void listBox_DrawItem(object sender, Tenor.Mobile.UI.DrawItemEventArgs e)
        {
            Venue venue = (Venue)e.Item.Value;
            Brush textBrush = (e.Item.Selected? brushS  : brush);
            RectangleF rect = new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            e.Graphics.DrawString(venue.Name, this.Font, textBrush, rect, format);
        }
    }
}
