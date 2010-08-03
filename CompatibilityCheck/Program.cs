using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace CompatibilityCheck
{
    class Program
    {

        /// <summary>
        /// Gets the string os OEM.
        /// </summary>
        public static string OemInfo
        {
            get
            {
                return GetSystemParameter(SPI_GETOEMINFO);
            }
        }

        public static string Manufacturer
        {
            get
            {
                return GetSystemParameter(SPI_GETPLATFORMMANUFACTURER);
            }
        }

        [DllImport("coredll.dll")]
        private static extern int SystemParametersInfo(uint uiAction, uint uiParam, StringBuilder pvParam, uint fWiniIni);
        private const uint SPI_GETPLATFORMTYPE = 257;
        private const uint SPI_GETPLATFORMMANUFACTURER = 262;
        private const uint SPI_GETOEMINFO = 258;
        private static string GetSystemParameter(uint uiParam)
        {
            StringBuilder sb = new StringBuilder(128);
            if (SystemParametersInfo(uiParam, (uint)sb.Capacity, sb, 0) == 0)
                throw new ApplicationException("Failed to get system parameter");
            return sb.ToString();
        }

        static string[] files = new string[] {
            @"\WINDOWS\COREDLL.DLL",
            @"\WINDOWS\toolhelp.dll",
            @"\WINDOWS\gpsapi.dll",
            @"\WINDOWS\ril.dll",
            @"\WINDOWS\cellcore.dll",
            @"\WINDOWS\aygshell.dll"

        };

        static void Main(string[] args)
        {
            using (FileStream file = new FileStream(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "mysquare.txt"), FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.WriteLine("MySquare Compatibility Check");
                writer.WriteLine(DateTime.Now.ToString());
                writer.WriteLine(DateTime.UtcNow.ToString());
                writer.WriteLine("============================");
                writer.WriteLine(".NET Framework version: ");

                using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey
                    (@"Software\Microsoft\.NETCompactFramework"))
                {
                    foreach (var valueName in key.GetValueNames())
                    {
                        try
                        {
                            if ((int)key.GetValue(valueName, 0) == 1)
                                writer.WriteLine("  * " + valueName);
                        }
                        catch { }
                    }
                    writer.WriteLine();
                }
                writer.WriteLine("Native Libraries:");
                foreach (string f in files)
                {
                    writer.WriteLine(@"  * " + f + ": " + File.Exists(f).ToString());
                }
                writer.WriteLine();
                writer.WriteLine("Device Information:");
                writer.WriteLine("  * " + Manufacturer + " " + OemInfo);
                writer.WriteLine("Windows:");
                writer.WriteLine("  * " + Environment.OSVersion.ToString());
                writer.WriteLine("Platform:");
                writer.WriteLine("  * " + Environment.Version.ToString());

            }
        }
    }
}
