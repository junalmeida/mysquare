using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace MySquare.Service
{
    static class Configuration
    {
        private static object time;
        static Configuration()
        {
            time = DateTime.UtcNow.ToString("yyyy-MM");

            string keyPath = "Software\\RisingMobility\\MySquare";
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
                key = Registry.LocalMachine.CreateSubKey(keyPath);
            else
                key = Registry.CurrentUser.CreateSubKey(keyPath);

        }

        #region Premium Info
        internal static bool? isPremium = null;
        internal static bool IsPremium
        {
            get
            {
                if (string.IsNullOrEmpty(Login))
                    return false;
                else
                {
                    if (isPremium == null)
                        LoadPremiumInfo();
                    if (isPremium == null)
                        return false;
                    else
                        return isPremium.Value;
                }
            }
        }

        static string _appPath;
        internal static string GetAppPath()
        {
            if (string.IsNullOrEmpty(_appPath))
            {
                _appPath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                if (_appPath.StartsWith("file://"))
                    _appPath = _appPath.Substring(8).Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
                _appPath = System.IO.Path.GetDirectoryName(_appPath);

            }
            return _appPath;
        }


        static AutoResetEvent aEvent;
        static DateTime lastTry = DateTime.MinValue;

        private static void LoadPremiumInfo()
        {
            if ((DateTime.Now - lastTry).TotalMinutes < 0.5)
                return;
            else
                lastTry = DateTime.Now;

            if (string.IsNullOrEmpty(Token))
            {
                isPremium = false;
                return;
            }

            aEvent = new AutoResetEvent(false);
            if (string.IsNullOrEmpty(_Login))
            {
                FourSquare fservice = new FourSquare();
                fservice.Error += new ErrorEventHandler(service_Error);
                fservice.UserResult += new MySquare.FourSquare.UserEventHandler(fservice_UserResult);
                fservice.GetUser(null);
                aEvent.WaitOne(5000, false);

                if (string.IsNullOrEmpty(_Login))
                {
                    return;
                }
            }

            aEvent = new AutoResetEvent(false);
            RisingMobilityService service = new RisingMobilityService();
            service.PremiumArrived += new RisingMobilityEventHandler(service_PremiumArrived);
            service.Error += new ErrorEventHandler(service_Error);
            service.GetPremiumInfo(_Login);
            aEvent.WaitOne(5000, false);
        }

        static void fservice_UserResult(object serder, MySquare.FourSquare.UserEventArgs e)
        {
            if (e.User.FriendStatus.HasValue && e.User.FriendStatus.Value == MySquare.FourSquare.FriendStatus.self)
            {
                if (e.User.Contact != null && !string.IsNullOrEmpty(e.User.Contact.Email))
                    Configuration._Login = e.User.Contact.Email;
                else if (e.User.Contact != null && !string.IsNullOrEmpty(e.User.Contact.Phone))
                    Configuration._Login = e.User.Contact.Phone;
            }
            aEvent.Set();
        }

        static void service_Error(object serder, ErrorEventArgs e)
        {
            isPremium = null;
            aEvent.Set();
        }

        static void service_PremiumArrived(object sender, RisingMobilityEventArgs e)
        {
            if (e.Result == null)
                isPremium = false;
            else
            {
                string username = Login;
                if (username != null)
                    username = username.ToLower();

                string resultS = "true||" + time + "||" + username;
                var md5 = System.Security.Cryptography.MD5.Create();
                byte[] crypt = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(resultS));
                isPremium = crypt.SequenceEqual(e.Result);
            }
            aEvent.Set();
        }
        #endregion

        private static RegistryKey key;

        private static string _Login;
        public static string Login
        {
            get
            {
                if (string.IsNullOrEmpty(_Login))
                    LoadPremiumInfo();
                return _Login;
            }
        }

        public static string Token
        {
            get
            {
                return (string)key.GetValue("Token", null);
            }
            set
            {
                isPremium = null;
                key.SetValue("Token", value);
                LoadPremiumInfo();
            }
        }

        public static string Password
        {
            get
            {
                string password = (string)key.GetValue("password", null);
                if (string.IsNullOrEmpty(password))
                    return password;
                else
                {
                    try
                    {
                        byte[] passB = Convert.FromBase64String(password);
                        password = Encoding.UTF8.GetString(passB, 0, passB.Length);
                    }
                    catch
                    {
                        Password = password;
                    }
                    return password;
                }
            }
            set
            {
                key.SetValue("password", Convert.ToBase64String(Encoding.UTF8.GetBytes(value)));
            }
        }

        public static bool ShowAds
        {
            get
            {
                if (!IsPremium)
                    return true;
                else
                {
                    try
                    {
                        return Convert.ToBoolean(key.GetValue("ShowAds", true));
                    }
                    catch { return true; }
                }
            }
            set
            {
                key.SetValue("ShowAds", value);
            }
        }


        public static MapType MapType
        {
            get
            {

                try
                {
                    return (MapType)Enum.Parse(typeof(MapType), key.GetValue("MapType", MapType.Roadmap.ToString()).ToString(), true);
                }
                catch { return MapType.Roadmap; }

            }
            set
            {
                key.SetValue("MapType", value.ToString());
            }
        }


        public static bool AutoUpdate
        {
            get
            {

                try
                {
                    return Convert.ToBoolean((int)key.GetValue("AutoUpdate", 1));
                }
                catch { return true; }

            }
            set
            {
                key.SetValue("AutoUpdate", Convert.ToInt32(value));
            }
        }

        public static bool DoubleTap
        {
            get
            {

                try
                {
                    return Convert.ToBoolean((int)key.GetValue("DoubleTap", 1));
                }
                catch { return true; }

            }
            set
            {
                key.SetValue("DoubleTap", Convert.ToInt32(value));
            }
        }


        public const int DefaultPingInterval = 15;
        public static int PingInterval
        {
            get
            {
                if (!IsPremium)
                    return 0;
                else
                {
                    try
                    {
                        return Convert.ToInt32(key.GetValue("PingInterval", DefaultPingInterval));
                    }
                    catch { return 0; }
                }
            }
            set
            {
                key.SetValue("PingInterval", value);
            }
        }

        public static bool RetrievePings
        {
            get
            {
                try
                {
                    return Convert.ToInt32(key.GetValue("RetrievePings", 0)) != 0;
                }
                catch { return false; }
            }
            set
            {
                key.SetValue("RetrievePings", Convert.ToInt32(value));
            }
        }


        public static DateTime LastCheckIn
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(key.GetValue("LastCheckIn", DateTime.MinValue));
                }
                catch { return DateTime.MinValue; }
            }
            set
            {
                key.SetValue("LastCheckIn", value);
            }
        }



        public static bool UseGps
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(key.GetValue("UseGps", true));
                }
                catch { return true; }

            }
            set
            {
                key.SetValue("UseGps", value);
            }
        }

        public static string Cookie
        {
            get
            {
                string cookie = (string)key.GetValue("Cookie", null);
                if (string.IsNullOrEmpty(cookie))
                {
                    cookie = Guid.NewGuid().ToString().Replace("-", "");


                    key.SetValue("Cookie", cookie);
                }
                return cookie;
            }
        }

        static int limit = 25;
        public static int ResultsLimit
        {
            get
            {
                return limit;
            }
        }


        public static string GetVersion()
        {
            var a = typeof(Configuration).Assembly;
            Version versionObj = a.GetName().Version;
            string version = string.Format("{0}.{1}.{2}", versionObj.Major, versionObj.Minor, versionObj.Build);

            string suffix = null;
            object[] atts = a.GetCustomAttributes(typeof(System.Reflection.AssemblyConfigurationAttribute), true);
            foreach (var at in atts)
            {
                if (at is System.Reflection.AssemblyConfigurationAttribute)
                {
                    suffix = (at as System.Reflection.AssemblyConfigurationAttribute).Configuration;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(suffix))
                version += "." + suffix;
            return version;
        }



        public static Guid GetAppGuid()
        {
            var a = typeof(Configuration).Assembly;
            object[] atts = a.GetCustomAttributes(typeof(GuidAttribute), true);
            string guid = null;
            foreach (var at in atts)
            {
                if (at is GuidAttribute)
                {
                    guid = (at as GuidAttribute).Value;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(guid))
                return new Guid(guid);
            else
                throw new InvalidOperationException();
        }

        static bool? isFirstTime = null;
        public static bool IsFirstTime
        {
            get
            {
                if (!isFirstTime.HasValue)
                {
                    string version = key.GetValue("Version", string.Empty) as string;
                    string current = GetVersion();
                    if (!string.Equals(version, current))
                    {
                        key.SetValue("Version", current);
                        isFirstTime = true;
                    }
                    else
                        isFirstTime = false;
                }
                return isFirstTime.Value;

            }
        }

        internal static bool abortCheck = false;
        internal static void CheckNotifications()
        {
#if !TESTING_2010
            Thread t = new Thread(new ThreadStart(delegate()
            {
                try
                {
                    while (true)
                    {
                        bool doOpen = Configuration.PingInterval > 0;
                        if (doOpen)
                        {
                            Tenor.Mobile.Diagnostics.Process process = null;
                            foreach (var p in Tenor.Mobile.Diagnostics.Process.GetProcesses())
                            {
                                if (System.IO.Path.GetFileNameWithoutExtension(p.FileName) == "MySquare.Pings")
                                {
                                    process = p;
                                    break;
                                }
                            }
                            RetrievePings = true;
                            if (process == null)
                            {
                                string path = Configuration.GetAppPath();
                                System.Diagnostics.Process.Start(System.IO.Path.Combine(path, "MySquare.Pings.exe"), string.Empty);
                            }
                        }
                        else if (!isPremium.HasValue)
                        {
                            Thread.Sleep(15000);
                            if (abortCheck)
                                break;
                            else
                                continue;
                        }
                        else
                        {
                            RetrievePings = false;
                        }
                        break;
                    }
                }
                catch { }
            }));

            t.StartThread();
#endif

        }


        internal static bool IsAlpha
        {
            get
            {
#if ALPHA
                return true;
#else
                return false;
#endif
            }
        }

        static DateTime versionDate;
        internal static DateTime GetVersionDate()
        {
            if (versionDate == DateTime.MinValue)
            {
                var appPath = typeof(Configuration).Assembly.GetName().CodeBase;
                if (appPath.StartsWith("file://"))
                    appPath = appPath.Substring(8).Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
                var file = new System.IO.FileInfo(appPath);
                versionDate = file.CreationTime;
            }
            return versionDate;
        }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    enum MapType
    {
        Roadmap = 0,
        Satellite = 1,
        Hybrid = 2,
        Terrain = 3
    }




    static class ThreadExtensions
    {
        static List<Thread> threadList = new List<Thread>();
        public static void StartThread(this Thread t)
        {
            if (!threadList.Contains(t))
                threadList.Add(t);
            t.IsBackground = true;
            t.Start();
        }


        public static void AbortThreads()
        {
            foreach (Thread t in threadList)
            {
                try
                {
                    if (t != null)
                        t.Abort();
                }
                catch { }
            }
        }



    }

}
