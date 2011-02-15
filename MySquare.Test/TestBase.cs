using System;
using System.Net;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MySquare.Test
{
    [TestClass]
    public abstract class TestBase
    {
        public const int TimeOut = 10000;
        protected AutoResetEvent wait = new AutoResetEvent(false);
        protected Exception exception;

        [TestInitialize]
        public void Initialize()
        {
            wait.Reset();
            exception = null;

            SetProxy();
        }

        public void SetProxy()
        {

            var proxy = new WebProxy("web-proxy.atl.hp.com", 8080);
            proxy.BypassProxyOnLocal = true;
            HttpWebRequest.DefaultWebProxy = proxy;
            //HttpWebRequest.DefaultWebProxy.Credentials = new NetworkCredential("", "");

        }
    }
}
