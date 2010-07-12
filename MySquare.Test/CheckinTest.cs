using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySquare.FourSquare;
using System.Threading;
using MySquare.Service;

namespace MySquare.Test
{
    /// <summary>
    /// Summary description for CheckinTest
    /// </summary>
    [TestClass]
    public class CheckinTest
    {
        AutoResetEvent wait = new AutoResetEvent(false);
        CheckIn result;
        [TestMethod]
        public void CheckIn()
        {
            var service = new Service.FourSquare();
            Configuration.Login = "junalmeida@gmail.com";
            Configuration.Password = "htc9377@";

            service.Error += new ErrorEventHandler(service_Error);
            service.CheckInResult += new CheckInEventHandler(service_CheckInResult);
            wait.Reset();
            result = null;
            service.CheckIn(new Venue() { Id = 5080252 }, "teste", false, null, null);
            wait.WaitOne();
            Assert.IsNotNull(result);
        }

        void service_CheckInResult(object serder, CheckInEventArgs e)
        {
            result = e.CheckIn;
            wait.Set();
        }

        void service_Error(object serder, ErrorEventArgs e)
        {
            wait.Set();
        }
    }
}
