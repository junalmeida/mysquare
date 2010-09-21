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
        private static string current = null;
        private static int count = 0;
        private static string GetLogPath(string key)
        {
            string appPath = Configuration.GetAppPath();
            string path = System.IO.Path.Combine(appPath, "debug");
            if (!System.IO.Directory.Exists(path))
                return null;
            else if (current == null)
            {
                current = "d" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                System.IO.Directory.CreateDirectory(Path.Combine(path, current));
            }
            path = Path.Combine(path, current);
            DateTime date = DateTime.Now;

            string filePath = string.Format("{0:0000}_{1}.txt", count++, key);
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
                lock (fileName)
                {
                    try
                    {
                        using (FileStream file = new FileStream(fileName, FileMode.Create))
                        using (StreamWriter writer = new StreamWriter(file))
                        {
                            RegisterLog(writer, ex);
                        }
                    }
                    catch { }
                }
            }
            return true;
        }

        private static void RegisterLog(StreamWriter writer, Exception ex)
        {
            string title = ex.GetType().FullName;
            if (title != "System.Exception")
                writer.WriteLine(ex.GetType().FullName);
            writer.WriteLine(DateTime.Now.ToString());
            writer.WriteLine(ex.Message);
            try
            {
                if (ex is WebException)
                {
                    WebException wex = (WebException)ex;
                    writer.WriteLine("Status: " + wex.Status.ToString());
                    if (wex.Response != null && wex.Response.Headers != null)
                        writer.WriteLine("Headers: " + wex.Response.Headers.ToString());
                }
            }
            catch { }
            writer.WriteLine();
            writer.WriteLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                writer.WriteLine("----");
                RegisterLog(writer, ex.InnerException);
            }
        }

    }
}
