using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.UI;
using System.Windows.Forms;

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
            LeftSoftButtonText = string.Empty;
            RightSoftButtonEnabled = true;
            RightSoftButtonText = "&Exit";

        }

        public override void Deactivate()
        {
            View.Visible = false;
        }

        public override void OnRightSoftButtonClick()
        {
            Application.Exit();
        }

    }
}
