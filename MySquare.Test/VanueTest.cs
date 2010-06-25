using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySquare.FourSquare;
using System.Threading;
namespace MySquare.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class VanueTest
    {
        const int TimeOut = 10000;
        AutoResetEvent wait = new AutoResetEvent(false);


        [TestMethod]
        public void SearchVenues()
        {
            Service service = new Service();
            service.SearchArrives += new SearchEventHandler(service_SearchArrives);

            wait.Reset();
            service.SearchNearby(null, -22.878073, -43.223305);
            if (wait.WaitOne(TimeOut))
            {
            }
            else
                Assert.Inconclusive("Request timed out.");
            
        }

        void service_SearchArrives(object sender, SearchEventArgs e)
        {
            wait.Set();
        }
    }
}
