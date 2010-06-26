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
            imgLoading.Image = Resources.Loader;

            Tenor.Mobile.UI.Skin.Current.ApplyColorsToControl(this);

            Program.Service.SearchArrives += new MySquare.FourSquare.SearchEventHandler(Service_SearchArrives);
            Program.Service.Error += new ErrorEventHandler(Service_Error);
        }

        void Service_Error(object serder, ErrorEventArgs e)
        {
            this.Invoke(new System.Threading.ThreadStart(delegate()
            {
                listBox.Visible = false;
                imgLoading.Visible = false;
                lblError.Visible = true;
            }));
        }



        internal void Refresh()
        {
            listBox.Visible = false;
            imgLoading.Visible = true;
            lblError.Visible = false;

            Program.Service.SearchNearby(null, -22.878073, -43.223305);
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

        void LoadVenues(Venue[] venues)
        {

            listBox.Visible = true;
            imgLoading.Visible = false;
            lblError.Visible = false;
        }
    }
}
