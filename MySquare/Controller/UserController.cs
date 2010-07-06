using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.UI.Friends;
using MySquare.UI;
using MySquare.FourSquare;

namespace MySquare.Controller
{
    class UserController : BaseController<UserDetail>
    {

        public UserController(UserDetail view)
            : base(view)
        {
        }


        public override void Activate()
        {
            
            Service.UserResult += new MySquare.FourSquare.UserEventHandler(Service_UserResult);
        }

        public override void Deactivate()
        {

            Service.UserResult -= new MySquare.FourSquare.UserEventHandler(Service_UserResult);
        }

        void Service_UserResult(object serder, MySquare.FourSquare.UserEventArgs e)
        {
            LoadUser(e.User);
        }

        User user;
        private void LoadUser(MySquare.FourSquare.User user)
        {
            this.user = user;
        }

    }
}
