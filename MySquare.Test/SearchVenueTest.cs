using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySquare.FourSquare;
namespace MySquare.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class SearchVenueTest : TestBase
    {

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
