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
    interface IController
    {
        Service Service { get; }
        AutoResetEvent WaitThread { get; }
        void Activate();
        void Deactivate();
        void OnLeftSoftButtonClick();
        void OnRightSoftButtonClick();
    }

    abstract class BaseController<T> : IController, IDisposable where T : IView
    {

        protected T View
        {
            get;
            private set;
        }

        static IList<IController> Controllers = new List<IController>();
        static int CurrentController = -1;

        public BaseController(T view)
        {
            this.View = view;
            Controllers.Add(this);
            if (Controllers.Count == 1 && Controllers[0] == this)
            {
                Service = new Service();
                WaitThread = new AutoResetEvent(false);
            }
            else
            {
                Service = Controllers[0].Service;
                WaitThread = Controllers[0].WaitThread;
            }
        }


        protected Service Service { get; private set; }
        protected AutoResetEvent WaitThread { get; private set; }

        protected const string googleMapsUrl =
            "http://maps.google.com/maps/api/staticmap?zoom=16&sensor=false&mobile=true&format=jpeg&size={0}x{1}&markers=color:blue|{2},{3}";

        public static IController OpenController(MySquare.UI.IView view)
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
            else
                throw new NotImplementedException();


            for (int i = 0; i < Controllers.Count; i++)
                if (Controllers[i].GetType() == type)
                {
                    CurrentController = i;
                    Controllers[i].Activate();
                    return Controllers[i];
                }

            if (CurrentController > -1)
                Controllers[CurrentController].Deactivate();

            var ctors = type.GetConstructors();
            IController newController = (IController)ctors[0].Invoke(new object[] { view });
            Controllers.Add(newController);
            CurrentController = Controllers.Count - 1;
            newController.Activate();

            return newController;
        }



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

        protected virtual void OnLeftSoftButtonClick()
        {
        }

        protected virtual void OnRightSoftButtonClick()
        {
        }

        #endregion

        public void Dispose()
        {
            WaitThread.Set();
            Service.Abort();
            Service = null;
            WaitThread = null;
        }

        protected abstract void Activate();
        protected virtual void Deactivate() { }
        protected void ShowError(string text)
        {
            CurrentController = 0;
            MainController.ShowErrorForm(text);
        }

        #region IController Members
        void IController.Activate()
        {
            this.Activate();
        }

        void IController.Deactivate()
        {
            this.Deactivate();
        }


        Service IController.Service
        {
            get { return this.Service; }
        }

        AutoResetEvent IController.WaitThread
        {
            get { return this.WaitThread; }
        }

        void IController.OnLeftSoftButtonClick()
        {
            this.OnLeftSoftButtonClick();
        }

        void IController.OnRightSoftButtonClick()
        {
            this.OnRightSoftButtonClick();
        }

        #endregion
    }
}
