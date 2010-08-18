using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Threading;
using System.Runtime.InteropServices;

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
        private static void LoadPremiumInfo()
        {
            if (string.IsNullOrEmpty(Login))
            {
                isPremium = false;
                return;
            }
            aEvent = new AutoResetEvent(false);
            RisingMobilityService service = new RisingMobilityService();
            service.PremiumArrived += new RisingMobilityEventHandler(service_PremiumArrived);
            service.Error += new ErrorEventHandler(service_Error);
            service.GetPremiumInfo(Login);
            aEvent.WaitOne(5000, false);
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
                string resultS = "true||" + time + "||" + Login;
                var md5 = System.Security.Cryptography.MD5.Create();
                byte[] crypt = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(resultS));
                isPremium = crypt.SequenceEqual(e.Result);
            }
            aEvent.Set();
        }
        #endregion

        private static RegistryKey key;
        public static string Login
        {
            get
            {
                return (string)key.GetValue("login", null);
            }
            set
            {
                isPremium = null;
                key.SetValue("login", value);
                LoadPremiumInfo();
            }
        }

        public static string Password
        {
            get
            {
                return (string)key.GetValue("password", null);
            }
            set
            {
                key.SetValue("password", value);
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


        public static int LastCheckIn
        {
            get
            {
                try
                {
                    return Convert.ToInt32(key.GetValue("LastCheckIn", 0));
                }
                catch { return 0; }
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
            string version = string.Format("{0}.{1}", versionObj.Major, versionObj.Minor);

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

        public static bool IsFirstTime()
        {
            string version = key.GetValue("Version", string.Empty) as string;
            string current = GetVersion();
            if (!string.Equals(version, current))
            {
                key.SetValue("Version", current);
                return true;
            }
            else
                return false;
        }

    }
}
