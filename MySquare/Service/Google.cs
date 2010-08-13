using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MySquare.FourSquare;

namespace MySquare.Service
{
    //class Google : Network
    //{
    //    enum ServiceResource
    //    {
    //        Geocoding
    //    }

    //    internal void GetGeocoding(double latitude, double longitude)
    //    {
    //        Dictionary<string, string> parameters = new Dictionary<string, string>();
    //        parameters.Add("latlng", string.Format("{0},{1}", latitude.ToString(culture), longitude.ToString(culture)));
    //        parameters.Add("sensor", "true");

    //        Post(ServiceResource.Geocoding, parameters);
    //    }

    //    internal event GeocodeEventHandler GeocodeResult;
    //    private void OnGeocodeResult(GeocodeEventArgs e)
    //    {
    //        if (GeocodeResult != null)
    //        {
    //            GeocodeResult(this, e);
    //        }
    //    }


    //    private void Post(ServiceResource service, Dictionary<string, string> parameters)
    //    {
    //        string url = null;

    //        switch (service)
    //        {
    //            case ServiceResource.Geocoding:
    //                url = "http://maps.google.com/maps/api/geocode/json";
    //                break;
    //            default:
    //                throw new NotImplementedException();
    //        }

    //        base.Post(0, url, false, null, null, parameters);
    //    }

    //    protected override Type GetJsonType(int key)
    //    {
    //        return typeof(GeocodeEventArgs);
    //    }

    //    protected override void OnResult(object result, int key)
    //    {
    //        OnGeocodeResult((GeocodeEventArgs)result);
    //    }


    //}
}
