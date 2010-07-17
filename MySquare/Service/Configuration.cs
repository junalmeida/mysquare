using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace MySquare.Service
{
    static class Configuration
    {
        static Configuration()
        {
            string keyPath = "Software\\RisingMobility\\MySquare";
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
                key = Registry.LocalMachine.CreateSubKey(keyPath);
            else
                key = Registry.CurrentUser.CreateSubKey(keyPath);

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
                key.SetValue("login", value);
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
            return typeof(Configuration).Assembly.GetName().Version.ToString();
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
