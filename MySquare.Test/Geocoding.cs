﻿//#define PROXY
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySquare.FourSquare;
using System.Threading;
using System.Drawing;
using RisingMobility.Mobile.Location;
using System.Net;
namespace MySquare.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class GeocodingTest : TestBase
    {
        //[TestMethod]
        public void Geocoding()
        {
#if PROXY
            WebProxy proxy = new WebProxy("10.2.108.25", 8080);
            proxy.Credentials = new NetworkCredential("y3tr", "htc9377J");
            System.Net.WebRequest.DefaultWebProxy = proxy;
#endif

            List<PointF> latlngList = new List<PointF>();
            latlngList.Add(new PointF(28.404931F, -131.850588F)); //Water


            latlngList.Add(new PointF(-22.909667F, -43.179656F)); //Rio de Janeiro
            latlngList.Add(new PointF(-22.857696F, -43.373414F)); //Rio de Janeiro
            latlngList.Add(new PointF(-9.967379F, -67.819979F)); //Acre
            latlngList.Add(new PointF(-1.295547F, -47.923565F)); //Roraima
            latlngList.Add(new PointF(42.983202F, -108.627434F)); //Wyoming
            latlngList.Add(new PointF(19.404819F, -72.367172F)); //Haiti, unknown address
            latlngList.Add(new PointF(18.555136F, -72.319565F)); //Haiti, port-au-prince
            latlngList.Add(new PointF(31.248382F, 121.243515F)); //Shanghai, China
            latlngList.Add(new PointF(43.665674F, -79.40938F)); //Toronto, Canada
            latlngList.Add(new PointF(45.577742F, -73.739641F)); //Montreal, Canada
            latlngList.Add(new PointF(45.543064F, -73.640699F)); //Montreal, Canada
            latlngList.Add(new PointF(45.501098F, -73.565912F)); //Montreal, Canada
            latlngList.Add(new PointF(45.501745F, -73.567414F)); //Montreal, Canada

            List<Geolocation> list = new List<Geolocation>();
            foreach (PointF latlng in latlngList)
            {
                var loc = Geolocation.Get(latlng.X, latlng.Y);
                if (latlngList.IndexOf(latlng) == 0)
                    Assert.IsNull(loc);
                else
                    Assert.IsNotNull(loc);
                if (loc != null)
                {
                    System.Diagnostics.Debug.WriteLine(loc.ToString());
                    list.Add(loc);
                }

            }
        }
    }
}