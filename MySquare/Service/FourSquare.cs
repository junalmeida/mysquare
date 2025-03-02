﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using MySquare.FourSquare;

namespace MySquare.Service
{
    class FourSquare : Network
    {

        #region Data Cache

        static List<User> cacheUsers = new List<User>();
        static List<Venue> cacheVenues = new List<Venue>();


        #endregion

        private const string client_id = "HAS1SHMA5HIZQSJZREGQEXQOHG51X3TZVX1BJCMVDQZT0FBF";
        private const string client_secret = "OPJZHF2G0SUT1DBLWEP2GQAV4LYFKAU005LUPDLS54V3IC3G";

        enum ServiceResource
        {
            SearchNearby,
            TipsNearby,
            CheckIn,
            Venue,
            AddTip,
            AddVenue,
            CheckIns,
            User,
            Friends,
            PendingFriends,
            AcceptFriend,
            RejectFriend,
            RequestFriend,
            Leaderboard,
            FlagClosed,
            FlagMislocated,
            FlagDuplicated,
            Badges,
            AllSettings
        }

        protected override void SetClientId(StringBuilder queryString)
        {
            queryString.Append("&client_id=");
            queryString.Append(UrlEncode(client_id));
            queryString.Append("&client_secret=");
            queryString.Append(UrlEncode(client_secret));
        }


        internal void GetLeaderBoard()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string uid = null;
            foreach (var u in cacheUsers)
            {
                if (u.FriendStatus.HasValue && u.FriendStatus.Value == FriendStatus.self)
                {
                    uid = u.Id;
                    break;
                }
            }

            Post(ServiceResource.Leaderboard, parameters);
        }


        internal void GetSiteSettings()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            Post(ServiceResource.AllSettings, parameters);
        }

        internal void GetUser(string uid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (uid != null)
                parameters.Add("uid", uid);
            parameters.Add("badges", "1");
            parameters.Add("mayor", "0");

            Post(ServiceResource.User, parameters);
        }

        internal void GetFriends(string uid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (uid != null)
                parameters.Add("uid", uid.ToString());

            Post(ServiceResource.Friends, parameters);
        }

        internal void GetBadges(string uid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (uid != null)
                parameters.Add("uid", uid.ToString());

            Post(ServiceResource.Badges, parameters);
        }

        internal void GetPendingFriends()
        {
            Post(ServiceResource.PendingFriends, null);
        }

        internal void AcceptFriend(string uid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (uid != null)
                parameters.Add("uid", uid);
            Post(ServiceResource.AcceptFriend, parameters);
        }

        internal void RejectFriend(string uid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (uid != null)
                parameters.Add("uid", uid);
            Post(ServiceResource.RejectFriend, parameters);
        }

        internal void GetFriendsCheckins(double? latitude, double? longitude)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (latitude.HasValue && longitude.HasValue)
            {
                parameters.Add("ll", string.Format("{0},{1}", latitude.Value.ToString(culture) , longitude.Value.ToString(culture)));
            }
            Post(ServiceResource.CheckIns, parameters);
        }

        internal void RequestFriend(string uid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (uid != null)
                parameters.Add("uid", uid);
            Post(ServiceResource.RequestFriend, parameters);
        }

        internal void SearchNearby(string search, double lat, double lgn)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("ll", string.Format("{0},{1}", lat.ToString(culture), lgn.ToString(culture)));
            parameters.Add("limit", MySquare.Service.Configuration.ResultsLimit.ToString(culture));
            if (!string.IsNullOrEmpty(search))
                parameters.Add("query", search);

            Post(ServiceResource.SearchNearby, parameters);
        }

        internal void GetTipsNearby(double lat, double lgn)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("ll", string.Format("{0},{1}", lat.ToString(culture), lgn.ToString(culture)));
            parameters.Add("limit", MySquare.Service.Configuration.ResultsLimit.ToString(culture));
            parameters.Add("filter", "nearby");
            parameters.Add("sort", "nearby");

            Post(ServiceResource.TipsNearby, parameters);
        }


        internal void CheckIn(Venue venue, string shout, bool tellFriends, bool? facebook, bool? twitter)
        {
            CheckIn(venue, shout, tellFriends, facebook, twitter, null, null, null, null);
        }

        internal void CheckIn(string shout, bool tellFriends, bool? facebook, bool? twitter, double? lat, double? lng, double? altitude, double? accuracy)
        {
            CheckIn(null, shout, tellFriends, facebook, twitter, lat, lng, altitude, accuracy);
        }

        internal void CheckIn(Venue venue, string shout, bool tellFriends, bool? facebook, bool? twitter, double? lat, double? lng, double? altitude, double? accuracy)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            if (venue != null)
                parameters.Add("venueId", venue.Id.ToString());
            if (!string.IsNullOrEmpty(shout))
                parameters.Add("shout", shout);

            List<string> broadcast = new List<string>();

            if (!tellFriends)
                broadcast.Add("private");
            else
            {
                broadcast.Add("public");
                if (facebook.HasValue && facebook.Value)
                    broadcast.Add("facebook");
                if (twitter.HasValue && twitter.Value)
                    broadcast.Add("twitter");
            }
            parameters.Add("broadcast", string.Join(",", broadcast.ToArray()));


            if (lat.HasValue && lng.HasValue)
            {
                parameters.Add("ll", string.Format(culture, "{0},{1}", lat.Value, lng.Value));

                if (altitude.HasValue)
                {
                    parameters.Add("alt", altitude.Value.ToString(culture));
                }
                if (accuracy.HasValue)
                {
                    parameters.Add("altAcc", accuracy.Value.ToString(culture));
                }
            }

            Post(ServiceResource.CheckIn, parameters);
        }

        internal void GetVenue(string vid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("id", vid.ToString());

            Post(ServiceResource.Venue, parameters);
        }


        internal void AddTip(string vid, string text)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("venueId", vid.ToString());
            parameters.Add("text", text.ToString());

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
            parameters.Add("crossStreet", crossStreet);
            parameters.Add("city", city);
            parameters.Add("state", state);
            parameters.Add("zip", zip);
            parameters.Add("phone", phone);
            if (primaryCategoryId.HasValue)
                parameters.Add("primarycategoryid", primaryCategoryId.Value.ToString());

            parameters.Add("ll", string.Format("{0},{1}",  lat.ToString(culture), lng.ToString(culture)));

            Post(ServiceResource.AddVenue, parameters);
        }

        internal void FlagAsClosed(string vid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("vid", vid.ToString());
            parameters.Add("problem", "closed");
            Post(ServiceResource.FlagClosed, parameters);
        }
        internal void FlagAsDuplicated(string vid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("vid", vid.ToString());
            parameters.Add("problem", "duplicate");
            Post(ServiceResource.FlagDuplicated, parameters);
        }
        internal void FlagAsMislocated(string vid)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("vid", vid.ToString());
            parameters.Add("problem", "mislocated");
            Post(ServiceResource.FlagMislocated, parameters);
        }




        #region Events
        internal event FlagEventHandler FlagResult;
        private void OnFlagResult(FlagEventArgs e)
        {
            if (FlagResult != null)
            {
                FlagResult(this, e);
            }
        }
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


        internal event BadgesEventHandler BadgesResult;
        private void OnBadgesResult(BadgesEventArgs e)
        {
            if (BadgesResult != null)
            {
                BadgesResult(this, e);
            }
        }



        internal event SiteSettingsEventHandler SiteSettingsResult;
        private void OnSiteSettingsResult(SiteSettingsEventArgs e)
        {
            if (SiteSettingsResult != null)
            {
                SiteSettingsResult(this, e);
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
                ManageCheckInsCache(e.CheckIns);
                CheckInsResult(this, e);
            }
        }

        private static void ManageCheckInsCache(CheckIn[] checkIns)
        {
            if (checkIns != null)
                foreach (var chkIn in checkIns)
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
        }


        internal event AddTipEventHandler AddTipResult;
        private void OnAddTipResult(TipEventArgs e)
        {
            if (AddTipResult != null)
            {
                AddTipResult(this, e);
            }
        }



        internal event LeaderboardEventHandler LeaderboardResult;
        private void OnLeaderboardResult(LeaderboardEventArgs e)
        {
            if (LeaderboardResult != null)
            {
                LeaderboardResult(this, e);
            }
        }


        internal event VenueEventHandler VenueResult;
        private void OnVenueResult(VenueEventArgs e)
        {
            if (VenueResult != null)
            {
                if (e.Venue == null)
                    OnError(new ErrorEventArgs(new Exception("Invalid foursquare response.")));
                else
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

                    if (e.Venue.Status != null && e.Venue.Mayor != null && e.Venue.Mayor.User != null)
                    {
                        index = cacheUsers.IndexOf(e.Venue.Mayor.User);
                        if (index == -1)
                            cacheUsers.Add(e.Venue.Mayor.User);
                        else if (cacheUsers[index].fullData)
                            e.Venue.Mayor.User = cacheUsers[index];
                    }

                    //if (e.Venue.CheckIns != null && e.Venue.CheckIns.Length > 0)
                    //{
                    //    ManageCheckInsCache(e.Venue.CheckIns);
                    //}


                    VenueResult(this, e);
                }
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

        internal event TipsEventHandler TipsResult;
        private void OnTipsResult(TipsEventArgs e)
        {
            if (TipsResult != null)
            {
                TipsResult(this, e);
            }
        }




        internal event SearchEventHandler SearchArrives;
        private void OnSearchArrives(SearchEventArgs e)
        {
            if (SearchArrives != null)
            {
                foreach (VenueGroup g in e.Groups)
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
                    url = "https://api.foursquare.com/v2/venues/search";
                    auth = !string.IsNullOrEmpty(Configuration.Token);
                    break;
                case ServiceResource.TipsNearby:
                    url = "https://api.foursquare.com/v2/tips/search";
                    post = false;
                    auth = !string.IsNullOrEmpty(Configuration.Token);
                    break;
                case ServiceResource.CheckIn:
                    url = "https://api.foursquare.com/v2/checkins/add";
                    post = true; auth = true;
                    break;
                case ServiceResource.Venue:
                    url = "https://api.foursquare.com/v2/venues/{0}";
                    auth = !string.IsNullOrEmpty(Configuration.Token);
                    break;
                case ServiceResource.AddTip:
                    url = "https://api.foursquare.com/v2/tips/add";
                    auth = true; post = true;
                    break;
                case ServiceResource.AddVenue:
                    url = "https://api.foursquare.com/v2/venues/add";
                    auth = true; post = true;
                    break;
                case ServiceResource.CheckIns:
                    url = "https://api.foursquare.com/v2/checkins/recent";
                    auth = true; post = false;
                    break;
                case ServiceResource.User:
                    url = "https://api.foursquare.com/v2/users/{0}";
                    auth = true; post = false;
                    break;
                case ServiceResource.Friends:
                    url = "https://api.foursquare.com/v2/users/{0}/friends";
                    auth = true; post = false;
                    break;
                case ServiceResource.PendingFriends:
                    url = "https://api.foursquare.com/v2/users/requests";
                    auth = true; post = false;
                    break;
                case ServiceResource.AcceptFriend:
                    url = "https://api.foursquare.com/v2/users/{0}/approve";
                    auth = true; post = true;
                    break;
                case ServiceResource.RejectFriend:
                    url = "https://api.foursquare.com/v2/users/{0}/deny";
                    auth = true; post = true;
                    break;
                case ServiceResource.RequestFriend:
                    url = "https://api.foursquare.com/v2/users/{0}/request";
                    auth = true; post = true;
                    break;
                case ServiceResource.Leaderboard:
                    url = "https://api.foursquare.com/v2/users/leaderboard";
                    auth = true; post = false;
                    break;
                case ServiceResource.FlagClosed:
                    url = "https://api.foursquare.com/v2/venues/{0}/flag";
                    auth = true; post = true;
                    break;
                case ServiceResource.FlagMislocated:
                    url = "https://api.foursquare.com/v2/venues/{0}/flag";
                    auth = true; post = true;
                    break;
                case ServiceResource.FlagDuplicated:
                    url = "https://api.foursquare.com/v2/venues/{0}/flag";
                    auth = true; post = true;
                    break;
                case ServiceResource.Badges:
                    url = "https://api.foursquare.com/v2/users/{0}/badges";
                    auth = true; post = false;
                    break;
                case ServiceResource.AllSettings:
                    url = "https://api.foursquare.com/v2/settings/all";
                    auth = true; post = false;
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (url.Contains("{0}"))
            {
                if (parameters != null && parameters.ContainsKey("id"))
                {
                    string id = parameters["id"];
                    parameters.Remove("id");
                    url = string.Format(url, id);
                }
                else if (parameters != null && parameters.ContainsKey("uid"))
                {
                    string id = parameters["uid"];
                    parameters.Remove("uid");
                    url = string.Format(url, id);
                }
                else if (parameters != null && parameters.ContainsKey("vid"))
                {
                    string id = parameters["vid"];
                    parameters.Remove("vid");
                    url = string.Format(url, id);
                }
                else
                {
                    url = string.Format(url, "self");
                }
            }


            if (auth &&
                string.IsNullOrEmpty(MySquare.Service.Configuration.Token))
                OnError(new ErrorEventArgs(new UnauthorizedAccessException()));
            else
                base.Post((int)service, url, post,
                    (auth ? MySquare.Service.Configuration.Token : null),
                     parameters);
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
                case ServiceResource.TipsNearby:
                    type = typeof(TipsEventArgs);
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
                case ServiceResource.RequestFriend:
                    type = typeof(UserEventArgs);
                    break;
                case ServiceResource.Leaderboard:
                    type = typeof(LeaderboardEventArgs);
                    break;
                case ServiceResource.FlagDuplicated:
                case ServiceResource.FlagClosed:
                case ServiceResource.FlagMislocated:
                    type = typeof(FlagEventArgs);
                    break;
                case ServiceResource.Badges:
                    type = typeof(BadgesEventArgs);
                    break;
                case ServiceResource.AllSettings:
                    type = typeof(SiteSettingsEventArgs);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return type;
        }

        protected override void OnResult(object result, int key)
        {

            ServiceResource service = (ServiceResource)key;
            if (result is SearchEventArgs)
                OnSearchArrives((SearchEventArgs)result);
            else if (result is TipsEventArgs)
                OnTipsResult((TipsEventArgs)result);
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
                UserEventArgs userResult = (UserEventArgs)result;
                if (service == ServiceResource.AcceptFriend)
                    userResult.Accepted = true;
                else if (service == ServiceResource.RejectFriend || service == ServiceResource.RequestFriend)
                    userResult.Accepted = false;

                OnUserResult(userResult);
            }
            else if (result is FriendsEventArgs)
                OnFriendsResult((FriendsEventArgs)result);
            else if (result is PendingFriendsEventArgs)
                OnPendingFriendsResult((PendingFriendsEventArgs)result);
            else if (result is ErrorEventArgs)
                OnError((ErrorEventArgs)result);
            else if (result is FlagEventArgs)
                OnFlagResult((FlagEventArgs)result);
            else if (result is LeaderboardEventArgs)
                OnLeaderboardResult((LeaderboardEventArgs)result);
            else if (result is BadgesEventArgs)
                OnBadgesResult((BadgesEventArgs)result);
            else if (result is SiteSettingsEventArgs)
                OnSiteSettingsResult((SiteSettingsEventArgs)result);
            else
                throw new NotImplementedException();

        }


    }
}
