using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;

namespace MySquare.Test
{
    /// <summary>
    /// Summary description for InterfaceTest
    /// </summary>
    [TestClass]
    public class InterfaceTest
    {
        [TestMethod]
        public void ApplicationTest()
        {
            using (UI.Main mainForm = new UI.Main())
            using (MySquare.Controller.MainController mainController = new MySquare.Controller.MainController(mainForm))
            {
                Application.Run(mainForm);
            }
        }
    }
}
