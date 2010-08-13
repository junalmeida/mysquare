using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    delegate void SearchEventHandler(object serder, SearchEventArgs e);

    class SearchEventArgs : EventArgs
    {
        [JsonProperty("venues")]
        private Venue[] Venues
        { get; set; }

        private Group[] groups;
        [JsonProperty("groups")]
        public Group[] Groups
        {
            get
            {
                if (groups == null)
                    return new Group[] { new Group() { Venues = this.Venues }};
                else
                    return groups;
            }
            set
            {
                groups = value;
            }
        }
    }

    delegate void VenueEventHandler(object sender, VenueEventArgs e);
    class VenueEventArgs : EventArgs
    {
        [JsonProperty("venue")]
        public Venue Venue
        { get; set; }
    }

    class Group
    {
        [JsonProperty("type")]
        public string Type
        { get; set; }

        [JsonProperty("venues")]
        public Venue[] Venues
        { get; set; }

        public override string ToString()
        {
            return Type;
        }

    }

    class Venue
    {
        internal bool fullData;

        [JsonProperty("id")]
        public int Id
        { get; set; }

        [JsonProperty("name")]
        public string Name
        { get; set; }

        [JsonProperty("primarycategory")]
        public Category PrimaryCategory
        { get; set; }

        [JsonProperty("address")]
        public string Address
        { get; set; }

        [JsonProperty("crossstreet")]
        public string CrossStreet
        { get; set; }

        [JsonProperty("phone")]
        public string Phone
        { get; set; }


        [JsonProperty("city")]
        public string City
        { get; set; }

        [JsonProperty("state")]
        public string State
        { get; set; }

        [JsonProperty("zip")]
        public string ZipCode
        { get; set; }

        [JsonProperty("twitter")]
        public string Twitter
        { get; set; }


        [JsonProperty("geolat")]
        public double Latitude
        { get; set; }

        [JsonProperty("geolong")]
        public double Longitude
        { get; set; }

        [JsonProperty("stats")]
        public Status Status
        { get; set; }

        [JsonProperty("distance")]
        public int Distance
        { get; set; }

        [JsonProperty("tips")]
        public Tip[] Tips
        { get; set; }

        [JsonProperty("categories")]
        public Category[] Categories
        { get; set; }

        [JsonProperty("specials")]
        public Special[] Specials
        { get; set; }

        [JsonProperty("tags")]
        public string[] Tags
        { get; set; }


        [JsonProperty("checkins")]
        public CheckIn[] CheckIns
        { get; set; }

        public override string ToString()
        {
            return Name;
        }


        internal void CopyTo(Venue venue)
        {
            venue.fullData = fullData;
            venue.Id = Id;
            venue.Name = Name;
            venue.PrimaryCategory = PrimaryCategory;
            venue.Address = Address;
            venue.CrossStreet = CrossStreet;
            venue.Phone = Phone;
            venue.City = City;
            venue.State = State;
            venue.ZipCode = ZipCode;
            venue.Twitter = Twitter;
            venue.Latitude = Latitude;
            venue.Longitude = Longitude;
            venue.Status = Status;
            venue.Distance = Distance;
            venue.Tips = Tips;
            venue.Categories = Categories;
            venue.Specials = Specials;
            venue.Tags = Tags;
            venue.CheckIns = CheckIns;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return Id == ((Venue)obj).Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    class Category
    {

        [JsonProperty("id")]
        public int Id
        { get; set; }

        [JsonProperty("fullpathname")]
        public string FullName
        { get; set; }

        [JsonProperty("nodename")]
        public string NodeName
        { get; set; }

        [JsonProperty("iconurl")]
        public string IconUrl
        { get; set; }

        public override string ToString()
        {
            return FullName;
        }
 
    }

    class Status
    {

        [JsonProperty("herenow")]
        public int HereNow
        { get; set; }

        [JsonProperty("checkins")]
        public int CheckIns
        { get; set; }

        [JsonProperty("friendrequests")]
        public int FriendRequests
        { get; set; }

        [JsonProperty("mayorcount")]
        public int MayorCount
        { get; set; }

        [JsonProperty("beenhere")]
        public BeenHere BeenHere
        { get; set; }

        [JsonProperty("mayor")]
        public Mayor Mayor
        { get; set; }
    }

    class BeenHere
    {
        [JsonProperty("me")]
        public bool Me
        { get; set; }

        [JsonProperty("friends")]
        public bool Friends
        { get; set; }
    }
}
