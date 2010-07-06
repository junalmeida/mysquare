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

    abstract class BaseController
    {
        public abstract Service Service { get; }
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
            BaseController newController = (BaseController)ctors[0].Invoke(new object[] { view });
            Controllers.Add(newController);
            CurrentController = Controllers.Count - 1;
            newController.Activate();

            return newController;
        }

        internal const string googleMapsUrl =
            "http://maps.google.com/maps/api/staticmap?zoom=16&sensor=false&mobile=true&format=jpeg&size={0}x{1}&markers=color:blue|{2},{3}";

    }

    abstract class BaseController<T> : BaseController, IDisposable where T : IView
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
                service = new Service();
                waitThread = new AutoResetEvent(false);
            }
            else
            {
                service = Controllers[0].Service;
                waitThread = Controllers[0].WaitThread;
            }
        }


        private Service service;
        private AutoResetEvent waitThread;
        public override Service Service { get { return service; }}
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

        public void Dispose()
        {
            WaitThread.Set();
            Service.Abort();
            service = null;
            waitThread = null;
        }

        protected void ShowError(string text)
        {
            CurrentController = 0;
            MainController.ShowErrorForm(text);
        }

    }
}
