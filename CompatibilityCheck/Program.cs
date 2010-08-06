using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

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
            try
            {
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "mysquare.txt");
                MessageBox.Show("This program will collect some debug data from your device. This may take some time and will try to discover your position. Wait for the success message.");
                using (FileStream file = new FileStream(
                    fileName, FileMode.Create, FileAccess.Write))
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
                    try
                    {
                        using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("System\\State\\Phone"))
                        {
                            writer.WriteLine("Operator:");
                            writer.WriteLine("  * " + key.GetValue("Current Operator Name", string.Empty));

                        }
                    }
                    catch { }
                    writer.WriteLine("Device Information:");
                    writer.WriteLine("  * " + Manufacturer + " " + OemInfo);
                    writer.WriteLine("Windows:");
                    writer.WriteLine("  * " + Environment.OSVersion.ToString());
                    writer.WriteLine("Platform:");
                    writer.WriteLine("  * " + Environment.Version.ToString());

                    writer.WriteLine();
                    writer.WriteLine();
                    RIL_Test(writer);

#if XPS
                    writer.WriteLine();
                    writer.WriteLine("Locating your cellphone:");
                    writer.WriteLine();
                    writer.Write(" * WPS:");

                    double lat, lng;
                    if (Tenor.Mobile.Location.Xps.WPSLocation("risingmobility", "junalmeida", out lat, out lng) == Tenor.Mobile.Location.WPS_ReturnCode.WPS_OK)
                    {
                        writer.WriteLine(lat.ToString() + " - " + lng.ToString());
                    }
                    else
                        writer.WriteLine("Error");
#endif


                    waithandle.Reset();
                    pos = new Tenor.Mobile.Location.WorldPosition(true, true);
                    pos.PollHit += new EventHandler(pos_LocationChanged);
                    pos.Error += new Tenor.Mobile.Location.ErrorEventHandler(pos_Error);
                    if (!waithandle.WaitOne(60000 * 5, false))
                        writer.Write("Gps give up. ");
                    writer.WriteLine("Done.");
                }
                pos.PollHit -= new EventHandler(pos_LocationChanged);
                pos.Error -= new Tenor.Mobile.Location.ErrorEventHandler(pos_Error);
                pos.Dispose();

                MessageBox.Show("Done.\r\n Check the file at: " + fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        static Tenor.Mobile.Location.WorldPosition pos = null;
        static int count = 0;
        static int gpsCount = 0;
        static void pos_Error(object sender, Tenor.Mobile.Location.ErrorEventArgs e)
        {
            output.WriteLine("  * Error: " + e.Error.Message);
            waithandle.Set();
        }

        static void pos_LocationChanged(object sender, EventArgs e)
        {
            output.WriteLine("  * Attemp " + (count + 1).ToString() + ": " + 
                pos.WorldPoint.ToString() + " - " + pos.FixType.ToString() + " - " + pos.WorldPoint.FixTime.ToString());
            if (pos.FixType == Tenor.Mobile.Location.FixType.Gps)
                gpsCount++;
            if (gpsCount > 4)
                waithandle.Set();
            count++;
        }

        static StreamWriter output;
        static void RIL_Test(StreamWriter output)
        {
            Program.output = output;
            IntPtr hRil = IntPtr.Zero;
            try
            {
                output.WriteLine("Testing RIL functions:");
                IntPtr hRes = IntPtr.Zero;

                // initialise RIL
                hRes = RIL_Initialize(1,                      // RIL port 1
                    new RILRESULTCALLBACK(rilResultCallback), // function to call with result
                    null,                                     // function to call with notify
                    0,                                        // classes of notification to enable
                    0,                                        // RIL parameters
                    out hRil);                                // RIL handle returned
                output.WriteLine("RIL:" + hRes.ToString());

                // initialised successfully

                // use RIL to get cell tower info with the RIL handle just created
                hRes = RIL_GetCellTowerInfo(hRil);

                // wait for cell tower info to be returned
                output.Write("Waiting 5 secs...");
                DateTime d1 = DateTime.Now;
                waithandle.WaitOne(5000, false);

                // finished - release the RIL handle
                output.WriteLine(" Done." + (DateTime.Now - d1).TotalMilliseconds.ToString());


                //celltower info finished
            }
            catch (Exception ex)
            {
                output.WriteLine("  ** ERROR: " + ex.Message);
            }
            finally
            {
                output.WriteLine("Freeing RIL...");
                if (hRil != IntPtr.Zero)
                    RIL_Deinitialize(hRil);

            }

        }

        // event used to notify user function that a response has
        //  been received from RIL
        private static AutoResetEvent waithandle = new AutoResetEvent(false);

        private static void rilResultCallback(uint dwCode,
                                             IntPtr hrCmdID,
                                             IntPtr lpData,
                                             uint cbData,
                                             uint dwParam)
        {
            try
            {
                output.WriteLine("rilResultCallback");
                // create empty structure to store cell tower info in
                RILCELLTOWERINFO rilCellTowerInfo = new RILCELLTOWERINFO();

                // copy result returned from RIL into structure
                Marshal.PtrToStructure(lpData, rilCellTowerInfo);
                output.WriteLine("struct recieved");

                // get the bits out of the RIL cell tower response that we want
                output.WriteLine("CellId: " + rilCellTowerInfo.dwCellID.ToString());
                output.WriteLine("AreaCode: " + rilCellTowerInfo.dwLocationAreaCode.ToString());
                output.WriteLine("CountryCode: " + rilCellTowerInfo.dwMobileCountryCode.ToString());
                output.WriteLine("NetworkCode: " + rilCellTowerInfo.dwMobileNetworkCode.ToString());
            }
            catch (Exception ex)
            {
                output.WriteLine("Exception: " + ex.Message);
            }
            // notify caller function that we have a result
            waithandle.Set();
        }

        #region Native Methods
        // -------------------------------------------------------------------
        //  RIL function definitions
        // -------------------------------------------------------------------
        /* 
         * Function definition converted from the definition 
         *  RILRESULTCALLBACK from MSDN:
         * 
         * http://msdn2.microsoft.com/en-us/library/aa920069.aspx
         */
        private delegate void RILRESULTCALLBACK(uint dwCode,
                                               IntPtr hrCmdID,
                                               IntPtr lpData,
                                               uint cbData,
                                               uint dwParam);

        /*
         * Function definition converted from the definition 
         *  RILNOTIFYCALLBACK from MSDN:
         * 
         * http://msdn2.microsoft.com/en-us/library/aa922465.aspx
         */
        private delegate void RILNOTIFYCALLBACK(uint dwCode,
                                               IntPtr lpData,
                                               uint cbData,
                                               uint dwParam);

        /*
         * Class definition converted from the struct definition 
         *  RILCELLTOWERINFO from MSDN:
         * 
         * http://msdn2.microsoft.com/en-us/library/aa921533.aspx
         */
        private class RILCELLTOWERINFO
        {
            public uint cbSize;
            public uint dwParams;
            public uint dwMobileCountryCode;
            public uint dwMobileNetworkCode;
            public uint dwLocationAreaCode;
            public uint dwCellID;
            public uint dwBaseStationID;
            public uint dwBroadcastControlChannel;
            public uint dwRxLevel;
            public uint dwRxLevelFull;
            public uint dwRxLevelSub;
            public uint dwRxQuality;
            public uint dwRxQualityFull;
            public uint dwRxQualitySub;
            public uint dwIdleTimeSlot;
            public uint dwTimingAdvance;
            public uint dwGPRSCellID;
            public uint dwGPRSBaseStationID;
            public uint dwNumBCCH;
        }

        // -------------------------------------------------------------------
        //  RIL DLL functions 
        // -------------------------------------------------------------------

        /* Definition from: http://msdn2.microsoft.com/en-us/library/aa919106.aspx */
        [DllImport("ril.dll")]
        private static extern IntPtr RIL_Initialize(
            uint dwIndex,
            RILRESULTCALLBACK pfnResult,
            RILNOTIFYCALLBACK pfnNotify,
            uint dwNotificationClasses,
            uint dwParam,
            out IntPtr lphRil);

        /* Definition from: http://msdn2.microsoft.com/en-us/library/aa923065.aspx */
        [DllImport("ril.dll")]
        private static extern IntPtr RIL_GetCellTowerInfo(IntPtr hRil);

        /* Definition from: http://msdn2.microsoft.com/en-us/library/aa919624.aspx */
        [DllImport("ril.dll")]
        private static extern IntPtr RIL_Deinitialize(IntPtr hRil);
        #endregion

    }
}
