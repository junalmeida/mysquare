using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.UI;
using MySquare.UI.Places.Create;
using MySquare.UI.Places;

namespace MySquare.Controller
{
    class CreateVenueController : BaseController
    {
        private CreateVenue View
        {
            get { return (CreateVenue)base.view; }
        }


        public CreateVenueController(CreateVenue view)
            : base ((IView)view)
        {
        }


        protected override void Activate()
        {
            Places parent = View.Parent as Places;
            parent.Reset();
            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.BringToFront();
            View.Visible = true;

        }
    }
}
