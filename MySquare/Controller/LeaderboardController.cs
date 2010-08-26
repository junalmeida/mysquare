using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

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
            throw new NotImplementedException();
        }

    }
}
