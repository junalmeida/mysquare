using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MySquare.Test
{
    /// <summary>
    /// Summary description for InterfaceTest
    /// </summary>
    [TestClass]
    public class InterfaceTest : TestBase
    {
        [TestMethod]
        public void CreateOAuth()
        {
            using (OAuth oauth = new OAuth())
            {
                Application.Run(oauth);
                if (!string.IsNullOrEmpty(oauth.Token))
                    MySquare.Service.Configuration.Token = oauth.Token;
            }

        }


        //[TestMethod]
        public void ApplicationTest()
        {
#if !TESTING
            Assert.Inconclusive("Not in test mode.");
#endif
            //MySquare.Program.Main();
            //Application.Run(new Form1());
        }
    }
}
