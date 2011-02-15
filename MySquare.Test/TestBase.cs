using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MySquare.Test
{
    public abstract class TestBase
    {
        [TestInitialize]
        public void SetProxy()
        {

            var proxy = new WebProxy("web-proxy.atl.hp.com", 8080);
            proxy.BypassProxyOnLocal = true;
            HttpWebRequest.DefaultWebProxy = proxy;
            //HttpWebRequest.DefaultWebProxy.Credentials = new NetworkCredential("", "");

        }
    }
}
