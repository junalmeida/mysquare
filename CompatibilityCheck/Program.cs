using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

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

        private static StreamWriter Debug;

        static void Main(string[] args)
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "mysquare.txt");
            FileStream file = new FileStream(
                           fileName, FileMode.Create, FileAccess.Write);
            Debug = new StreamWriter(file);
            Debug.AutoFlush = true;

            try
            {
                MessageBox.Show("This program will collect some debug data from your device. This may take some time and will try to discover your position. Wait for the success message.");

                Debug.WriteLine("MySquare Compatibility Check");
                Debug.WriteLine(DateTime.Now.ToString());
                Debug.WriteLine(DateTime.UtcNow.ToString());
                Debug.WriteLine("========================");
                Debug.WriteLine(".NET Framework version: ");

                using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey
                    (@"Software\Microsoft\.NETCompactFramework"))
                {
                    foreach (var valueName in key.GetValueNames())
                    {
                        try
                        {
                            if ((int)key.GetValue(valueName, 0) == 1)
                                Debug.WriteLine("  * " + valueName);
                        }
                        catch { }
                    }
                    Debug.WriteLine(string.Empty);
                }
                Debug.WriteLine("Native Libraries:");
                foreach (string f in files)
                {
                    Debug.WriteLine(@"  * " + f + ": " + File.Exists(f).ToString());
                }
                Debug.WriteLine(string.Empty);
                try
                {
                    using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("System\\State\\Phone"))
                    {
                        Debug.WriteLine("Operator:");
                        Debug.WriteLine("  * " + key.GetValue("Current Operator Name", string.Empty));

                    }
                }
                catch { }
                Debug.WriteLine("Device Information:");
                Debug.WriteLine("  * " + Manufacturer + " " + OemInfo);
                Debug.WriteLine("Windows:");
                Debug.WriteLine("  * " + Environment.OSVersion.ToString());
                Debug.WriteLine("Platform:");
                Debug.WriteLine("  * " + Environment.Version.ToString());

                Debug.WriteLine(string.Empty);
                Debug.WriteLine(string.Empty);
                RIL_Test();

#if XPS
                    Debug.WriteLine();
                    Debug.WriteLine("Locating your cellphone:");
                    Debug.WriteLine();
                    Debug.Write(" * WPS:");

                    double lat, lng;
                    if (Tenor.Mobile.Location.Xps.WPSLocation("risingmobility", "junalmeida", out lat, out lng) == Tenor.Mobile.Location.WPS_ReturnCode.WPS_OK)
                    {
                        Debug.WriteLine(lat.ToString() + " - " + lng.ToString());
                    }
                    else
                        Debug.WriteLine("Error");
#endif


                waithandle.Reset();

                pos = new Tenor.Mobile.Location.WorldPosition(true, true);
                pos.PollHit += new EventHandler(pos_LocationChanged);
                pos.Error += new Tenor.Mobile.Location.ErrorEventHandler(pos_Error);
                if (!waithandle.WaitOne(60000 * 1, false))
                    Debug.Write("Gps give up. ");
                Debug.WriteLine("Done.");

                pos.PollHit -= new EventHandler(pos_LocationChanged);
                pos.Error -= new Tenor.Mobile.Location.ErrorEventHandler(pos_Error);
                pos.Dispose();

                MessageBox.Show("Done.\r\n Check the file at: " + fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Debug.Close();
            }
        }

        static Tenor.Mobile.Location.WorldPosition pos = null;
        static int count = 0;
        static int gpsCount = 0;
        static void pos_Error(object sender, Tenor.Mobile.Location.ErrorEventArgs e)
        {
            Debug.WriteLine("  * " + e.Error.GetType().FullName + ": " + e.Error.Message);
            waithandle.Set();
        }

        static void pos_LocationChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("  * Attemp " + (count + 1).ToString() + ": " +
                pos.ToString() + " - " + 
                pos.WorldPoint.ToString() + " - " + pos.FixType.ToString() + " - " + pos.WorldPoint.FixTime.ToString());
            Debug.WriteLine(string.Empty);
            Debug.WriteLine(string.Empty);
            if (pos.FixType == Tenor.Mobile.Location.FixType.Gps)
                gpsCount++;
            if (gpsCount > 4)
                waithandle.Set();
            count++;
        }

        static void RIL_Test()
        {
            for (uint i = 1; i <= 4; i++)
            {
                Debug.WriteLine(string.Empty);
                Debug.WriteLine(string.Format("Testing RIL functions on RIL{0}:", i));
                IntPtr hRil = IntPtr.Zero;
                try
                {
                    IntPtr hRes = IntPtr.Zero;

                    // initialise RIL
                    hRes = RIL_Initialize(i,                      // RIL port 1
                        new RILRESULTCALLBACK(rilResultCallback), // function to call with result
                        null,                                     // function to call with notify
                        0,                                        // classes of notification to enable
                        0,                                        // RIL parameters
                        out hRil);                                // RIL handle returned
                    Debug.WriteLine("RIL:" + hRes.ToString());

                    // initialised successfully

                    // use RIL to get cell tower info with the RIL handle just created
                    hRes = RIL_GetCellTowerInfo(hRil);

                    // wait for cell tower info to be returned
                    Debug.Write("Waiting 5 secs...");
                    DateTime d1 = DateTime.Now;
                    waithandle.WaitOne(5000, false);

                    // finished - release the RIL handle
                    Debug.WriteLine(" Done." + (DateTime.Now - d1).TotalMilliseconds.ToString());


                    //celltower info finished
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("  ** ERROR: " + ex.Message);
                }
                finally
                {
                    Debug.WriteLine("Freeing RIL...");
                    if (hRil != IntPtr.Zero)
                        RIL_Deinitialize(hRil);

                }
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
                Debug.WriteLine("rilResultCallback");
                // create empty structure to store cell tower info in
                RILCELLTOWERINFO rilCellTowerInfo = new RILCELLTOWERINFO();

                // copy result returned from RIL into structure
                Marshal.PtrToStructure(lpData, rilCellTowerInfo);
                Debug.WriteLine("struct recieved");

                // get the bits out of the RIL cell tower response that we want
                Debug.WriteLine("CellId: " + rilCellTowerInfo.dwCellID.ToString());
                Debug.WriteLine("AreaCode: " + rilCellTowerInfo.dwLocationAreaCode.ToString());
                Debug.WriteLine("CountryCode: " + rilCellTowerInfo.dwMobileCountryCode.ToString());
                Debug.WriteLine("NetworkCode: " + rilCellTowerInfo.dwMobileNetworkCode.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
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
