using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySquare.FourSquare;
namespace MySquare.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TipsTest : TestBase
    {
        void service_Error(object serder, MySquare.Service.ErrorEventArgs e)
        {
            exception = e.Exception;
            wait.Set();
        }

        [TestMethod]
        public void TipsNearby()
        {
            Service.FourSquare service = new Service.FourSquare();
            service.TipsResult += new TipsEventHandler(service_TipsResult);
            service.Error += new MySquare.Service.ErrorEventHandler(service_Error);

            service.GetTipsNearby(-22.878073, -43.223305);
            if (wait.WaitOne(TimeOut))
            {
                if (exception != null)
                    Assert.Fail(exception.InnerException == null ? exception.Message : exception.InnerException.Message);
                Assert.IsNotNull(tipsNearby);
                Assert.IsNotNull(tipsNearby.Tips);
                foreach (var tip in tipsNearby.Tips)
                {
                    Assert.IsNotNull(tip.User);
                    Assert.IsNotNull(tip.Venue);
                }
            }
            else
                Assert.Inconclusive("Request timed out.");

        }

        TipsEventArgs tipsNearby;
        void service_TipsResult(object serder, TipsEventArgs e)
        {
            tipsNearby = e;
            wait.Set();
        }
    }
}
