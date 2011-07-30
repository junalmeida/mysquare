using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.UI;
using System.Windows.Forms;
using MySquare.FourSquare;
using System.Threading;
using System.Drawing;
using MySquare.Service;

namespace MySquare.Controller
{
    class LeaderboardController : BaseController<MySquare.UI.More.Leaderboard>
    {
        public LeaderboardController(MySquare.UI.More.Leaderboard view)
            : base(view)
        {
            Service.Error += new MySquare.Service.ErrorEventHandler(Service_Error);
            Service.LeaderboardResult += new MySquare.FourSquare.LeaderboardEventHandler(Service_LeaderboardResult);
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

            Refresh();
        }

        public override void OnLeftSoftButtonClick()
        {
            Refresh();
        }



        public override void OnRightSoftButtonClick()
        {
            OpenController((View.Parent as Main).moreActions1);
        }


        private void Refresh()
        {
            Cursor.Current = Cursors.WaitCursor;

            Service.GetLeaderBoard();
        }        
        


        LeaderboardUser[] users;
        void Service_LeaderboardResult(object serder, MySquare.FourSquare.LeaderboardEventArgs e)
        {
            users = e.Leaderboard;
            try
            {
                if (View.InvokeRequired)
                    View.Invoke(new ThreadStart(LoadFriends));
                else
                    LoadFriends();
            }
            catch (ObjectDisposedException) { }
        }


        void LoadFriends()
        {
            var listC = View.lstAll;

            View.ImageList = new Dictionary<string, byte[]>();
            listC.Clear();
            foreach (var u in users)
            {
                listC.AddItem(null, u);
            }

            Thread t = new Thread(new ThreadStart(delegate()
            {
                int i = 0;
                foreach (var u in users)
                {
                    if (!string.IsNullOrEmpty(u.User.ImageUrl))
                    {
                        try
                        {
                            //TODO: Load friends of friends avatars on demand.
                            //This is a workaround to not let the dataplan leak.
                            if (i > 10)
                                break;
                            if (!Service.IsInCache(u.User.ImageUrl))
                                i++;

                            byte[] image = Service.DownloadImageSync(u.User.ImageUrl);

                            if (!View.ImageList.ContainsKey(u.User.ImageUrl))
                            {
                                View.ImageList.Add(u.User.ImageUrl, image);
                                View.Invoke(new ThreadStart(delegate() { View.lstAll.Invalidate(); }));
                            }
                        }
                        catch (ObjectDisposedException) { return; }
                        catch { }
                    }
                }
            }));
            t.StartThread();

            Cursor.Current = Cursors.Default;
        }

    }
}
