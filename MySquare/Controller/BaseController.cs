using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MySquare.FourSquare;
using System.Windows.Forms;
using System.Collections;
using MySquare.UI;
using MySquare.Service;
using System.Drawing;

namespace MySquare.Controller
{

    abstract class BaseController : IDisposable
    {
        internal static Stack<object[]> Navigation = new Stack<object[]>();

        protected void SaveNavigation(object value)
        {
            if (Navigation.Count > 0)
            {
                object[] last = Navigation.Peek();
                if (last[0] == this && last[1] == value)
                    return;
            }
            Navigation.Push(new object[] { this, value });
        }

        protected bool GoBack()
        {
            if (Navigation.Count == 0)
                return false;
            object[] last = Navigation.Pop();
            if (Navigation.Count == 0)
                return false;
            last = Navigation.Pop();
            BaseController controller = last[0] as BaseController;
            if (controller is VenueDetailsController)
            {
                controller.Activate();
                ((VenueDetailsController)controller).OpenVenue((Venue)last[1]);
            }
            else if (controller is UserController)
            {
                controller.Activate();
                ((UserController)controller).LoadUser((User)last[1]);
            }
            else
                return false;
            CurrentController = Controllers.IndexOf(controller);
            return true;
        }




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

        protected static string[] lastTags;

        internal static void Terminate()
        {
            foreach (var ct in Controllers)
                ct.Dispose();
            Controllers.Clear();
            CurrentController = -1;
        }

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
            else if (view is UI.Help)
                type = typeof(HelpController);
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
            "http://maps.google.com/maps/api/staticmap?zoom={4}&sensor=false&mobile=true&format=jpeg&size={0}x{1}&markers=color:blue|{2},{3}";

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
        protected MainController MainController
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


        protected virtual void AddLeftSubMenu(MenuItem item)
        {
            MainController.AddLeftSubMenu(item);
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

        protected void ShowError(Exception ex)
        {
            string text = null;

            if (ex is UnauthorizedAccessException)
                text = "Invalid credentials, change your settings and try again.";
            else
            {
                text = "Cannot connect to foursquare, try again.";
                Log.RegisterLog(ex);
            }

            ShowError(text);

        }

        public override void Dispose()
        {
            base.Dispose();
            waitThread = null;
        }

    }
}
