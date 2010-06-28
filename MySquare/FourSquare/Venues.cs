using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    class SearchResponse
    {
        [JsonProperty("groups")]
        public Group[] Groups
        { get; set; }
    }

    class VenueResponse
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

        public override string ToString()
        {
            return Name;
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
