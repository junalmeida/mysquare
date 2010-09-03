using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.UI;
using System.Windows.Forms;
using MySquare.Service;
using System.Threading;

namespace MySquare.Controller
{
    class HelpController : BaseController<MySquare.UI.Help>
    {
        public HelpController(MySquare.UI.Help view)
            : base(view)
        {
        }


        public override void Activate()
        {

            (View.Parent as Main).Reset();
            View.BringToFront();
            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.Visible = true;

            LeftSoftButtonEnabled = false;
            LeftSoftButtonText = "";
            RightSoftButtonEnabled = true;
            RightSoftButtonText = "&Back";

        }

        public override void Deactivate()
        {
            View.Visible = false;
        }




        public override void OnRightSoftButtonClick()
        {
            OpenController((View.Parent as Main).moreActions1);
        }

    }
}
