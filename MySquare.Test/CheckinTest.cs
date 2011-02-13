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
        CheckInEventArgs result;
        Exception exception;
        [TestMethod]
        public void CheckIn()
        {
#if !TESTING
            Assert.Inconclusive("Not in test mode.");
#endif
            var service = new Service.FourSquare();

            service.Error += new ErrorEventHandler(service_Error);
            service.CheckInResult += new CheckInEventHandler(service_CheckInResult);
            wait.Reset();
            result = null;
            exception = null;
            //css festas
            service.CheckIn(new Venue() { Id = "4c1d4a0b63750f47ff08b867" }, "teste", false, null, null);
            wait.WaitOne();
            
            if (exception != null)
                Assert.Fail(exception.InnerException == null ? exception.Message : exception.InnerException.Message);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.CheckIn);
            Assert.IsNotNull(result.CheckIn.Venue);

            Assert.IsNotNull(result.Notifications);
            bool hasMessage=false;
            foreach (INotification notif in result.Notifications)
            {
                var message = notif as NotificationMessage;
                var score = notif as ScoreNotification;
                var badge = notif as BadgeNotification;
                var mayorship = notif as MayorshipNotification;
                var special = notif as SpecialNotification;

                if (message != null)
                    hasMessage = true;
            }
            Assert.IsTrue(hasMessage);
            
        }

        void service_CheckInResult(object serder, CheckInEventArgs e)
        {
            result = e;
            wait.Set();
        }

        void service_Error(object serder, ErrorEventArgs e)
        {
            exception = e.Exception;
            wait.Set();
        }
    }
}
