using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MySquare.FourSquare;
using System.Windows.Forms;
using System.Collections;
using MySquare.UI;

namespace MySquare.Controller
{

    abstract class BaseController : IDisposable
    {
        public BaseController()
        {
            Service = new Service.FourSquare();
        }

        public Service.FourSquare Service { get; private set; }

        public abstract AutoResetEvent WaitThread { get; }

        public virtual void Activate() { }
        public virtual void Deactivate() { }

        public virtual void OnLeftSoftButtonClick() { }
        public virtual void OnRightSoftButtonClick() { }


        protected static IList<BaseController> Controllers = new List<BaseController>();
        protected static int CurrentController = -1;
        internal static BaseController OpenController(MySquare.UI.IView view)
        {
            Type type = null;
            if (view is UI.Main)
                type = typeof(MainController);
            else if (view is UI.Places.Places)
                type = typeof(PlacesController);
            else if (view is UI.Places.VenueDetails)
                type = typeof(VenueDetailsController);
            else if (view is UI.Settings.Settings)
                type = typeof(SettingsController);
            else if (view is UI.Places.Create.CreateVenue)
                type = typeof(CreateVenueController);
            else if (view is UI.Friends.Friends)
                type = typeof(FriendsController);
            else if (view is UI.Friends.UserDetail)
                type = typeof(UserController);
            else
                throw new NotImplementedException();


            if (CurrentController > -1)
                Controllers[CurrentController].Deactivate();

            for (int i = 0; i < Controllers.Count; i++)
                if (Controllers[i].GetType() == type)
                {
                    CurrentController = i;
                    Controllers[i].Activate();
                    return Controllers[i];
                }


            var ctors = type.GetConstructors();
            BaseController newController = (BaseController)ctors[0].Invoke(new object[] { view });
            Controllers.Add(newController);
            CurrentController = Controllers.Count - 1;
            newController.Activate();

            return newController;
        }

        internal const string googleMapsUrl =
            "http://maps.google.com/maps/api/staticmap?zoom=16&sensor=false&mobile=true&format=jpeg&size={0}x{1}&markers=color:blue|{2},{3}";

        public virtual void Dispose()
        {
            if (WaitThread != null)
                WaitThread.Set();
            if (Service != null)
            {
                Service.Abort();
                Service = null;
            }
        }
    }

    abstract class BaseController<T> : BaseController where T : IView
    {

        protected T View
        {
            get;
            private set;
        }


        public BaseController(T view)
        {
            this.View = view;
            Controllers.Add(this);
            if (Controllers.Count == 1 && Controllers[0] == this)
            {
                waitThread = new AutoResetEvent(false);
            }
            else
            {
                waitThread = Controllers[0].WaitThread;
            }
        }



        private AutoResetEvent waitThread;
        public override AutoResetEvent WaitThread { get { return waitThread; } }


 



        #region Menu Control
        MainController MainController
        {
            get
            {
                return (MainController)Controllers[0];
            }
        }

        protected virtual string LeftSoftButtonText
        {
            get
            {
                return MainController.LeftSoftButtonText;
            }
            set
            {
                MainController.LeftSoftButtonText = value;
            }
        }

        protected virtual string RightSoftButtonText
        {
            get
            {
                return MainController.RightSoftButtonText;
            }
            set
            {
                MainController.RightSoftButtonText = value;
            }
        }

        protected virtual bool LeftSoftButtonEnabled
        {
            get
            {
                return MainController.LeftSoftButtonEnabled;
            }
            set
            {
                MainController.LeftSoftButtonEnabled = value;
            }
        }

        protected virtual bool RightSoftButtonEnabled
        {
            get
            {
                return MainController.RightSoftButtonEnabled;
            }
            set
            {
                MainController.RightSoftButtonEnabled = value;
            }
        }

        protected void HitLeftButton()
        {
            Controllers[CurrentController].OnLeftSoftButtonClick();
        }

        protected void HitRightButton()
        {
            Controllers[CurrentController].OnRightSoftButtonClick();
        }
        #endregion

 

        protected void ShowError(string text)
        {
            CurrentController = 0;
            MainController.ShowErrorForm(text);
        }

        public override void Dispose()
        {
            base.Dispose();
            waitThread = null;
        }
    }
}
