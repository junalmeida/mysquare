//#define TEST
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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            using (UI.Main mainForm = new UI.Main())
            using (Controller.MainController mainController = new MySquare.Controller.MainController(mainForm))
            {
                Application.Run(mainForm);
            }
        }
    }
}

#if TEST
namespace Microsoft.WindowsCE.Forms
{
    public class InputPanel : System.ComponentModel.Component
    {
        public InputPanel()
        {

        }

        public InputPanel(System.ComponentModel.IContainer container)
        {
        }
        public bool Enabled { get; set; }
        public System.Drawing.Rectangle Bounds
        {
            get
            {
                return System.Drawing.Rectangle.Empty;
            }
        }
        public event EventHandler EnabledChanged;
    }
}
#endif