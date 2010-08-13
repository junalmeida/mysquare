using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Tenor.Mobile.Drawing;
using System.Drawing;

namespace MySquare
{
    static class Extensions
    {
        public static string ToHumanTime(this DateTime register)
        {
            string value;
            TimeSpan time = DateTime.Now - register;
            if (time.TotalMinutes < 60)
                value = string.Format("{0} minutes ago.", time.Minutes);
            else if (time.TotalHours < 24)
                value = string.Format("{0} hours ago.", time.Hours);
            else if (time.TotalDays < 5)
                value = string.Format("{0} days ago.", time.Days);
            else
                value = register.ToShortDateString() + "  " + register.ToShortTimeString();
            return value;
        }

        public static void DrawSeparator(this Graphics g, Rectangle bounds, System.Drawing.Color color)
        {
            Rectangle rect2 = new Rectangle(
               bounds.X, bounds.Y, bounds.Width / 3, 1);
            Tenor.Mobile.Drawing.GradientFill.Fill(g, rect2, color, Color.LightGray, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
            rect2.X += rect2.Width;
            Tenor.Mobile.Drawing.GradientFill.Fill(g, rect2, Color.LightGray, Color.LightGray, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
            rect2.X += rect2.Width;
            Tenor.Mobile.Drawing.GradientFill.Fill(g, rect2, Color.LightGray, color, Tenor.Mobile.Drawing.GradientFill.FillDirection.LeftToRight);
        }

        public static void ClearImageList(this Dictionary<string, Bitmap> imageList)
        {
            if (imageList == null)
                return;
            else
                lock (imageList)
                {
                    foreach (string key in imageList.Keys.ToArray())
                    {
                        if (imageList[key] != null)
                            imageList[key].Dispose();
                        imageList.Remove(key);
                    }
                }
        }
        public static void ClearImageList(this Dictionary<string, AlphaImage> imageList)
        {
            if (imageList == null)
                return;
            else
                lock (imageList)
                {
                    foreach (string key in imageList.Keys.ToArray())
                    {
                        if (imageList[key] != null)
                            imageList[key].Dispose();
                        imageList.Remove(key);
                    }
                }
        }


    }
}
