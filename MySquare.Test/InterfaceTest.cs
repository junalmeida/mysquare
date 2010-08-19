#define PROXY

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using System.Net;

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
#if !TESTING
            Assert.Inconclusive("Not in test mode.");
#endif
#if PROXY
            HttpWebRequest.DefaultWebProxy = new WebProxy("inet-rj.petrobras.com.br", 8080);
            (HttpWebRequest.DefaultWebProxy as WebProxy).BypassProxyOnLocal = true;
            HttpWebRequest.DefaultWebProxy.Credentials = new NetworkCredential("y3tr", "htc9377J");
#endif

            Application.Run(new Form1());
        }
    }
}
