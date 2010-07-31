using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.Win32;
using System.Threading;
using MySquare.FourSquare;

namespace MySquare.Service
{
    class FourSquare : Network
    {

        #region Data Cache 

        static List<User> cacheUsers = new List<User>();
        static List<Venue> cacheVenues = new List<Venue>();


        #endregion



        enum ServiceResource
        {
            SearchNearby,
            CheckIn,
            Venue,
            AddTip,
            AddVenue,
            CheckIns,
            User,
            Friends, 
            PendingFriends,
            AcceptFriend,
            RejectFriend
        }
        

        internal void GetUser(int uid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (uid > 0)
                parameters.Add("uid", uid.ToString());
            parameters.Add("badges", "1");
            parameters.Add("mayor", "0");

            Post(ServiceResource.User, parameters);
        }

        internal void GetFriends(int uid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (uid > 0)
                parameters.Add("uid", uid.ToString());

            Post(ServiceResource.Friends, parameters);
        }

        internal void GetPendingFriends()
        {
            Post(ServiceResource.PendingFriends, null);
        }

        internal void AcceptFriend(int uid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (uid > 0)
                parameters.Add("uid", uid.ToString());
            Post(ServiceResource.AcceptFriend, parameters);
        }

        internal void RejectFriend(int uid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (uid > 0)
                parameters.Add("uid", uid.ToString());
            Post(ServiceResource.RejectFriend, parameters);
        }

        internal void GetFriendsCheckins(double latitude, double longitude)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("geolat", latitude.ToString(culture));
            parameters.Add("geolong", longitude.ToString(culture));

            Post(ServiceResource.CheckIns, parameters);
        }

        internal void SearchNearby(string search, double lat, double lgn)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("geolat", lat.ToString(culture));
            parameters.Add("geolong", lgn.ToString(culture));
            parameters.Add("l", MySquare.Service.Configuration.ResultsLimit.ToString(culture));
            if (!string.IsNullOrEmpty(search))
                parameters.Add("q", search);

            Post(ServiceResource.SearchNearby, parameters);
        }

        internal void CheckIn(Venue venue, string shout, bool tellFriends, bool? facebook, bool? twitter)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("vid", venue.Id.ToString());
            if (!string.IsNullOrEmpty(shout))
                parameters.Add("shout", shout);
            parameters.Add("private", Convert.ToInt32((!tellFriends)).ToString());
            if (twitter.HasValue)
                parameters.Add("twitter", Convert.ToInt32(twitter.Value).ToString());
            if (facebook.HasValue)
                parameters.Add("facebook", Convert.ToInt32(facebook.Value).ToString());


            Post(ServiceResource.CheckIn, parameters);
        }

        internal void GetVenue(int vid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("vid", vid.ToString());

            Post(ServiceResource.Venue, parameters);
        }


        internal void AddTip(int vid, string text)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("vid", vid.ToString());
            parameters.Add("text", text.ToString());
            //parameters.Add("geolat", lat.ToString(culture));
            //parameters.Add("geolong", lng.ToString(culture));

            Post(ServiceResource.AddTip, parameters);
        }

        internal void AddVenue(
            string name, string address, string crossStreet,
            string city, string state, string zip, string phone,
            double lat, double lng, int? primaryCategoryId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("name", name);
            parameters.Add("address", address);
            parameters.Add("crossstreet", crossStreet);
            parameters.Add("city", city);
            parameters.Add("state", state);
            parameters.Add("zip", zip);
            parameters.Add("phone", phone);
            if (primaryCategoryId.HasValue)
                parameters.Add("primarycategoryid", primaryCategoryId.Value.ToString());

            parameters.Add("geolat", lat.ToString(culture));
            parameters.Add("geolong", lng.ToString(culture));

            Post(ServiceResource.AddVenue, parameters);
        }




        #region Events     
        internal event PendingFriendsEventHandler PendingFriendsResult;
        private void OnPendingFriendsResult(PendingFriendsEventArgs e)
        {
            if (PendingFriendsResult != null)
            {
                for (int i = 0; i < e.Friends.Length; i++)
                {
                    int index = (cacheUsers.IndexOf(e.Friends[i]));
                    if (index > -1)
                        e.Friends[i] = cacheUsers[index];
                    else
                        cacheUsers.Add(e.Friends[i]);
                }
                PendingFriendsResult(this, e);
            }
        }
        internal event FriendsEventHandler FriendsResult;
        private void OnFriendsResult(FriendsEventArgs e)
        {
            if (FriendsResult != null)
            {
                for (int i = 0; i < e.Friends.Length; i++)
                {
                    int index = (cacheUsers.IndexOf(e.Friends[i]));
                    if (index > -1)
                        e.Friends[i] = cacheUsers[index];
                    else
                        cacheUsers.Add(e.Friends[i]);
                }
                FriendsResult(this, e);
            }
        }


        internal event UserEventHandler UserResult;
        private void OnUserResult(UserEventArgs e)
        {
            if (UserResult != null)
            {
                if (e.Accepted.HasValue)
                    e.User.fullData = false; //force it to get out from cache.
                else
                    e.User.fullData = true;

                int index = cacheUsers.IndexOf(e.User);
                if (index > -1)
                {
                    e.User.CopyTo(cacheUsers[index]);
                    e.User = cacheUsers[index];
                }
                else
                    cacheUsers.Add(e.User);
                UserResult(this, e);
            }
        }


        internal event CheckInsEventHandler CheckInsResult;
        private void OnCheckInsResult(CheckInsEventArgs e)
        {
            if (CheckInsResult != null)
            {
                foreach (var chkIn in e.CheckIns)
                {
                    int index = cacheVenues.IndexOf(chkIn.Venue);
                    if (index == -1)
                        cacheVenues.Add(chkIn.Venue);
                    else
                        chkIn.Venue = cacheVenues[index];

                    index = cacheUsers.IndexOf(chkIn.User);
                    if (index == -1)
                    {
                        cacheUsers.Add(chkIn.User);
                        if (chkIn.User.CheckIn == null)
                            chkIn.User.CheckIn = chkIn;
                    }
                    else
                        chkIn.User = cacheUsers[index];

                }
                CheckInsResult(this, e);
            }
        }


        internal event AddTipEventHandler AddTipResult;
        private void OnAddTipResult(TipEventArgs e)
        {
            if (AddTipResult != null)
            {
                AddTipResult(this, e);
            }
        }

   

        internal event VenueEventHandler VenueResult;
        private void OnVenueResult(VenueEventArgs e)
        {
            if (VenueResult != null)
            {
                e.Venue.fullData = true;
                int index = cacheVenues.IndexOf(e.Venue);
                if (index > -1)
                {
                    e.Venue.CopyTo(cacheVenues[index]);
                    e.Venue = cacheVenues[index];
                }
                else
                    cacheVenues.Add(e.Venue);

                if (e.Venue.Status != null && e.Venue.Status.Mayor != null)
                {
                    index = cacheUsers.IndexOf(e.Venue.Status.Mayor.User);
                    if (index == -1)
                        cacheUsers.Add(e.Venue.Status.Mayor.User);
                    else if (cacheUsers[index].fullData)
                        e.Venue.Status.Mayor.User = cacheUsers[index];
                }

                VenueResult(this, e);
            }
        }


        internal event CheckInEventHandler CheckInResult;
        private void OnCheckInResult(CheckInEventArgs e)
        {
            if (CheckInResult != null)
            {
                CheckInResult(this, e);
            }
        }



        internal event SearchEventHandler SearchArrives;
        private void OnSearchArrives(SearchEventArgs e)
        {
            if (SearchArrives != null)
            {
                foreach (Group g in e.Groups)
                    if (g.Venues != null)
                        for (int i = 0; i < g.Venues.Length; i++)
                        {
                            int index = cacheVenues.IndexOf(g.Venues[i]);
                            if (index > -1)
                                g.Venues[i] = cacheVenues[index];
                            else
                                cacheVenues.Add(g.Venues[i]);
                        }
                SearchArrives(this, e);
            }
        }

        #endregion

        private void Post(ServiceResource service, Dictionary<string, string> parameters)
        {
            string url = null;
            bool post = false;
            bool auth = false;

            switch (service)
            {
                case ServiceResource.SearchNearby:
                    url = "http://api.foursquare.com/v1/venues.json";
                    auth = !string.IsNullOrEmpty(Configuration.Login);
                    break;
                case ServiceResource.CheckIn:
                    url = "http://api.foursquare.com/v1/checkin.json";
                    post = true; auth = true;
                    break;
                case ServiceResource.Venue:
                    url = "http://api.foursquare.com/v1/venue.json";
                    auth = !string.IsNullOrEmpty(Configuration.Login);
                    break;
                case ServiceResource.AddTip:
                    url = "http://api.foursquare.com/v1/addtip.json";
                    auth = true; post = true;
                    break;
                case ServiceResource.AddVenue:
                    url = "http://api.foursquare.com/v1/addvenue.json";
                    auth = true; post = true;
                    break;
                case ServiceResource.CheckIns:
                    url = "http://api.foursquare.com/v1/checkins.json";
                    auth = true; post = false;
                    break;
                case ServiceResource.User:
                    url = "http://api.foursquare.com/v1/user.json";
                    auth = true; post = false;
                    break;
                case ServiceResource.Friends:
                    url = "http://api.foursquare.com/v1/friends.json";
                    auth = true; post = false;
                    break;
                case ServiceResource.PendingFriends:
                    url = "http://api.foursquare.com/v1/friend/requests.json";
                    auth = true; post = false;
                    break;
                case ServiceResource.AcceptFriend:
                    url = "http://api.foursquare.com/v1/friend/approve.json";
                    auth = true; post = true;
                    break;
                case ServiceResource.RejectFriend:
                    url = "http://api.foursquare.com/v1/friend/deny.json";
                    auth = true; post = true;
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (auth &&
                string.IsNullOrEmpty(MySquare.Service.Configuration.Login))
                OnError(new ErrorEventArgs(new UnauthorizedAccessException()));
            else 
                base.Post((int)service, url, post, 
                    (auth ? MySquare.Service.Configuration.Login : null),
                    (auth ? MySquare.Service.Configuration.Password : null), parameters);
        }

        protected override Type GetJsonType(int key)
        {
            ServiceResource service = (ServiceResource)(int)key;
            Type type = null;
            switch (service)
            {
                case ServiceResource.SearchNearby:
                    type = typeof(SearchEventArgs);
                    break;
                case ServiceResource.CheckIn:
                    type = typeof(CheckInEventArgs);
                    break;
                case ServiceResource.Venue:
                    type = typeof(VenueEventArgs);
                    break;
                case ServiceResource.AddTip:
                    type = typeof(TipEventArgs);
                    break;
                case ServiceResource.AddVenue:
                    type = typeof(VenueEventArgs);
                    break;
                case ServiceResource.CheckIns:
                    type = typeof(CheckInsEventArgs);
                    break;
                case ServiceResource.User:
                    type = typeof(UserEventArgs);
                    break;
                case ServiceResource.Friends:
                    type = typeof(FriendsEventArgs);
                    break;
                case ServiceResource.PendingFriends:
                    type = typeof(PendingFriendsEventArgs);
                    break;
                case ServiceResource.RejectFriend:
                case ServiceResource.AcceptFriend:
                    type = typeof(UserEventArgs);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return type;
        }

        protected override void OnResult(object result, int key)
        {
            if (result is SearchEventArgs)
                OnSearchArrives((SearchEventArgs)result);
            else if (result is CheckInEventArgs)
                OnCheckInResult((CheckInEventArgs)result);
            else if (result is VenueEventArgs)
                OnVenueResult((VenueEventArgs)result);
            else if (result is TipEventArgs)
                OnAddTipResult((TipEventArgs)result);
            else if (result is CheckInsEventArgs)
                OnCheckInsResult((CheckInsEventArgs)result);
            else if (result is UserEventArgs)
            {
                ServiceResource service = (ServiceResource)key;
                UserEventArgs userResult = (UserEventArgs)result;
                if (service == ServiceResource.AcceptFriend)
                    userResult.Accepted = true;
                else if (service == ServiceResource.RejectFriend)
                    userResult.Accepted = false;

                OnUserResult(userResult);
            }
            else if (result is FriendsEventArgs)
                OnFriendsResult((FriendsEventArgs)result);
            else if (result is PendingFriendsEventArgs)
                OnPendingFriendsResult((PendingFriendsEventArgs)result);
            else
                throw new NotImplementedException();

        }

    }
    


}
