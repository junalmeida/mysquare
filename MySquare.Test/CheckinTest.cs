using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySquare.FourSquare;

namespace MySquare.Test
{
    /// <summary>
    /// Summary description for CheckinTest
    /// </summary>
    [TestClass]
    public class CheckinTest
    {
 
        [TestMethod]
        public void CheckIn()
        {
            Service service = new Service();
            service.Login = "blah";
            service.Password= "blah";

        }
    }
}
