﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Threading;

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
        private static bool? isPremium = null;
        public static bool IsPremium
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


        static AutoResetEvent aEvent;
        private static void LoadPremiumInfo()
        {
            if (string.IsNullOrEmpty(Login))
            {
                isPremium = false;
                return;
            }
            aEvent = new AutoResetEvent(false);
            RisingMobility service = new RisingMobility();
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

        static int limit = 20;
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
