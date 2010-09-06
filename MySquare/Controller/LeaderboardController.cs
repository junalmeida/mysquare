using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.UI;

namespace MySquare.Controller
{
    class LeaderboardController : BaseController<MySquare.UI.More.Leaderboard>
    {
        public LeaderboardController(MySquare.UI.More.Leaderboard view)
            : base(view)
        {
            Service.Error += new MySquare.Service.ErrorEventHandler(Service_Error);
        }

        void Service_Error(object serder, MySquare.Service.ErrorEventArgs e)
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

        public override void OnRightSoftButtonClick()
        {
            OpenController((View.Parent as Main).moreActions1);
        }

    }
}
