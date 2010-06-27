using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using MySquare.FourSquare;
using Tenor.Mobile.Location;
using System.Threading;
namespace MySquare
{
    static class Program
    {
        static Program()
        {
            Service = new Service();
        }
        internal static Service Service { get; private set; }
        internal static AutoResetEvent WaitThread = new AutoResetEvent(false);

        internal static void ShowError(string text)
        {
            mainForm.ShowError(text);
        }

        static UI.Main mainForm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            using (mainForm = new MySquare.UI.Main())
            {
                Application.Run(mainForm);
            }
        }
    }
}