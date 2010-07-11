﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MySquare.Service
{
    internal static class Log
    {
        private static string GetLogPath()
        {
            string appPath = Network.GetAppPath();
            string path = System.IO.Path.Combine(appPath, "debug");
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            DateTime date = DateTime.Now;

            string filePath = string.Format("{0}.txt", date.Ticks);
            filePath = System.IO.Path.Combine(path, filePath);
            return filePath;
        }

        internal static void RegisterLog(Exception ex)
        {
            using (FileStream file = new FileStream(GetLogPath(), FileMode.Create))
            using (StreamWriter writer = new StreamWriter(file))
            {
                RegisterLog(writer, ex);
            }
        }

        private static void RegisterLog(StreamWriter writer, Exception ex)
        {
            writer.WriteLine(ex.Message);
            writer.WriteLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                writer.WriteLine("----");
                RegisterLog(writer, ex.InnerException);
            }
        }

    }
}
