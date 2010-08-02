using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CompatibilityCheck
{
    class Program
    {
        static string[] files = new string[] {
            @"C:\WINDOWS\COREDLL.DLL",
            @"C:\WINDOWS\toolhelp.dll",
            @"C:\WINDOWS\gpsapi.dll",
            @"C:\WINDOWS\ril.dll",
            @"C:\WINDOWS\cellcore.dll",
            @"C:\WINDOWS\aygshell.dll"

        };

        static void Main(string[] args)
        {
            using (FileStream file = new FileStream(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "mysquare.txt"), FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.WriteLine("MySquare Compatibility Check");
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

            }
        }
    }
}
