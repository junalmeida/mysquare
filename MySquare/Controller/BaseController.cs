using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MySquare.FourSquare;
using System.Windows.Forms;
using System.Collections;

namespace MySquare.Controller
{
    abstract class BaseController : IDisposable
    {

        protected MySquare.UI.IView view;

        static IList<BaseController> Controllers = new List<BaseController>();
        static int CurrentController = 0;
        
        public BaseController()
        {
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


        public static BaseController OpenController(MySquare.UI.IView view)
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
            else
                throw new NotImplementedException();


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
        protected virtual void ShowError(string text)
        {
            MainController.ShowError(text);
        }

    }
}
