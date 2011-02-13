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
    public class SearchVenueTest
    {
        const int TimeOut = 10000;
        AutoResetEvent wait = new AutoResetEvent(false);

        [TestInitialize]
        public void Initialize()
        {
            wait.Reset();
            exception = null;
        }

        [TestMethod]
        public void SearchVenues()
        {
            Service.FourSquare service = new Service.FourSquare();
            service.SearchArrives += new SearchEventHandler(service_SearchArrives);
            service.Error += new MySquare.Service.ErrorEventHandler(service_Error);

            wait.Reset();

            service.SearchNearby(null, -22.878073, -43.223305);
            if (wait.WaitOne(TimeOut))
            {
                if (exception != null)
                    Assert.Fail(exception.InnerException == null ? exception.Message : exception.InnerException.Message);

                Assert.IsNotNull(searchResults.Groups);
                foreach (var group in searchResults.Groups)
                {
                    Assert.IsNotNull(group.Venues);
                    foreach (var venue in group.Venues)
                    {
                        Assert.IsNotNull(venue.Name);
                    }
                }
            }
            else
                Assert.Inconclusive("Request timed out.");
            
        }
        Exception exception;
        SearchEventArgs searchResults;

        void service_Error(object serder, MySquare.Service.ErrorEventArgs e)
        {
            exception = e.Exception;
            wait.Set();
        }

        void service_SearchArrives(object sender, SearchEventArgs e)
        {
            searchResults = e;
            wait.Set();
        }




    }
}
