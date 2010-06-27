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
            Service.ImageResult += new ImageResultEventHandler(Service_ImageResult);
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


        static string appPath;
        private static string GetCachePath(string url)
        {
            if (string.IsNullOrEmpty(appPath))
            {
                appPath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                if (appPath.StartsWith("file://"))
                    appPath = appPath.Substring(8).Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
                appPath = System.IO.Path.GetDirectoryName(appPath);

            }
            string path = System.IO.Path.Combine(appPath, "cache");
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            string filePath = url.Substring(url.IndexOf(".com/") + 5).Replace("/", "_");
            filePath = System.IO.Path.Combine(path, filePath);
            return filePath;
        }

        private static System.Drawing.Image GetFromCache(string url)
        {
            string path = GetCachePath(url);
            if (System.IO.File.Exists(path))
                return new System.Drawing.Bitmap(path);
            else
                return null;
        }

        Dictionary<string, PictureBox> downloadsToPicBox = new Dictionary<string, PictureBox>();
        public void DownloadImageToPictureBox(string url, PictureBox box)
        {
            if (downloadsToPicBox.ContainsKey(url))
                downloadsToPicBox.Remove(url);

            System.Drawing.Image cache = GetFromCache(url);
            if (cache != null)
                box.Image = cache;
            else
            {
                downloadsToPicBox.Add(url, box);
                Service.DownloadImage(url);
            }
        }

        Dictionary<string, Dictionary<string, System.Drawing.Image>> downloadsToList = new Dictionary<string, Dictionary<string, System.Drawing.Image>>();
        public void DownloadImageToDictionary(string url, Dictionary<string, System.Drawing.Image> list)
        {
            if (downloadsToList.ContainsKey(url))
                downloadsToList.Remove(url);

            System.Drawing.Image cache = GetFromCache(url);
            if (cache != null)
                list[url] = cache;
            else
            {
                downloadsToList.Add(url, list);
                Service.DownloadImage(url);
            }
        }

        void Service_ImageResult(object serder, ImageEventArgs e)
        {
            SaveToCache(e.Url, e.Image);
            if (downloadsToPicBox.ContainsKey(e.Url))
            {
                PictureBox box = downloadsToPicBox[e.Url];
                if (box.InvokeRequired)
                    box.Invoke(new ThreadStart(delegate()
                    {
                        box.Image = e.Image;
                    }));
                else
                    box.Image = e.Image;
                downloadsToPicBox.Remove(e.Url);
            }
            else if (downloadsToList.ContainsKey(e.Url))
            {
                Dictionary<string, System.Drawing.Image> list = downloadsToList[e.Url];
                list[e.Url] = e.Image;
                downloadsToList.Remove(e.Url);
                if (view != null)
                {
                    UI.Places.Places places = view as UI.Places.Places;
                    if (places != null)
                        places.list1.listBox.Invalidate();
                }
            }
        }

        private static void SaveToCache(string url, System.Drawing.Image image)
        {
            try
            {
                string path = GetCachePath(url);
                image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch
            {
            }
        }
    }
}
