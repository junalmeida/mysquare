using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.UI;
using System.Windows.Forms;
using MySquare.FourSquare;
using System.Threading;

namespace MySquare.Controller
{
    class LeaderboardController : BaseController<MySquare.UI.More.Leaderboard>
    {
        public LeaderboardController(MySquare.UI.More.Leaderboard view)
            : base(view)
        {
            Service.Error += new MySquare.Service.ErrorEventHandler(Service_Error);
            Service.LeaderboardResult += new MySquare.FourSquare.LeaderboardEventHandler(Service_LeaderboardResult);
            View.tabStrip.SelectedIndexChanged += new EventHandler(tabStrip_SelectedIndexChanged);

        }



        void Service_Error(object serder, MySquare.Service.ErrorEventArgs e)
        {
            ShowError(e.Exception);
        }

        public override void Activate()
        {
            (View.Parent as Main).Reset();
            View.BringToFront();
            View.Dock = System.Windows.Forms.DockStyle.Fill;
            View.Visible = true;


            LeftSoftButtonEnabled = true;
            LeftSoftButtonText = "&Refresh";
            RightSoftButtonEnabled = true;
            RightSoftButtonText = "&Back";

            tabStrip_SelectedIndexChanged(null, null);
        }

        public override void OnLeftSoftButtonClick()
        {
            Refresh();
        }



        public override void OnRightSoftButtonClick()
        {
            OpenController((View.Parent as Main).moreActions1);
        }


        void tabStrip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (View.tabStrip.SelectedIndex == 0)
                scope = MySquare.Service.Scope.Friends;
            else
                scope = MySquare.Service.Scope.All;


            if (
                (scope == MySquare.Service.Scope.Friends && View.lstFriends.Count == 0) ||
                (scope == MySquare.Service.Scope.All && View.lstAll.Count == 0)
               )
                Refresh();
        }
        MySquare.Service.Scope scope;

        private void Refresh()
        {
            Cursor.Current = Cursors.WaitCursor;

            Service.GetLeaderBoard(Program.Location.WorldPoint.Latitude, Program.Location.WorldPoint.Longitude, scope);
        }

        LeaderboardUser[] users;
        string refreshTime;
        string allText;
        void Service_LeaderboardResult(object serder, MySquare.FourSquare.LeaderboardEventArgs e)
        {
            users = e.Users;
            refreshTime = e.RefreshTime;
            allText = e.AllText;
            if (View.InvokeRequired)
                View.Invoke(new ThreadStart(LoadFriends));
            else
                LoadFriends();
        }


        void LoadFriends()
        {
            var listC = View.lstAll;
            if (scope == MySquare.Service.Scope.Friends)
                listC = View.lstFriends;

            listC.Clear();
            foreach (var u in users)
            {
                listC.AddItem(null, u);
            }

            View.lblRefreshTime.Text = refreshTime;
            if (!string.IsNullOrEmpty(allText))
                View.tabStrip.Tabs[1] = allText;
            Cursor.Current = Cursors.Default;
        }

    }
}
