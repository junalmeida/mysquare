using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace MySquare.Service
{
    internal static class Log
    {
        private static string GetLogPath(string key)
        {
            string appPath = Network.GetAppPath();
            string path = System.IO.Path.Combine(appPath, "debug");
            if (!System.IO.Directory.Exists(path))
                return null;
            DateTime date = DateTime.Now;

            string filePath = string.Format("{0}.txt", key + "_" + date.Ticks);
            filePath = System.IO.Path.Combine(path, filePath);
            return filePath;
        }

        internal static bool RegisterLog(Exception ex)
        {
            return RegisterLog("general", ex);
        }


        internal static bool RegisterLog(string key, Exception ex)
        {
            if (ex == null)
                return false;
            else if (ex is ObjectDisposedException || ex is RequestAbortException || (ex.InnerException != null && ex.InnerException is WebException && ((WebException)ex.InnerException).Status == WebExceptionStatus.RequestCanceled))
                return false;

            string fileName = GetLogPath(key);
            if (!string.IsNullOrEmpty(fileName))
            {
                using (FileStream file = new FileStream(fileName, FileMode.Create))
                using (StreamWriter writer = new StreamWriter(file))
                {
                    RegisterLog(writer, ex);
                }
            }
            return true;
        }

        private static void RegisterLog(StreamWriter writer, Exception ex)
        {
            writer.WriteLine(ex.GetType().FullName);
            writer.WriteLine(ex.Message);
            try
            {
                if (ex is WebException)
                {
                    WebException wex = (WebException)ex;
                    writer.WriteLine("Status: " + wex.Status.ToString());
                    writer.WriteLine("Headers: " + wex.Response.Headers.ToString());

                }
            }
            catch { }
            writer.WriteLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                writer.WriteLine("----");
                RegisterLog(writer, ex.InnerException);
            }
        }

    }
}
