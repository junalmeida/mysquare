using System;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MySquare.FourSquare
{
    delegate void SearchEventHandler(object serder, SearchEventArgs e);

    class SearchEventArgs : EnvelopeEventArgs<SearchResults>
    {
        public VenueGroup[] Groups
        {
            get
            {
                return Response == null ? null : Response.Groups;
            }
        }
    }

    class SearchResults
    {
        [JsonProperty("venues")]
        private Venue[] Venues
        { get; set; }

        private VenueGroup[] groups;
        [JsonProperty("groups")]
        public VenueGroup[] Groups
        {
            get
            {
                if (groups == null)
                    return new VenueGroup[] { new VenueGroup() { Venues = this.Venues } };
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
    class VenueEventArgs : EnvelopeEventArgs<VenueResult>
    {
        public Venue Venue
        {
            get
            {
                return Response == null ? null : Response.Venue;
            }
            set
            {
                if (Response != null)
                {
                    Response.Venue = value;
                }
                else
                    throw new InvalidOperationException();
            }
        }
    }

    class VenueResult
    {
        [JsonProperty("venue")]
        public Venue Venue
        { get; set; }
    }

    class VenueGroup
    {
        [JsonProperty("type")]
        public string Type
        { get; set; }

        [JsonProperty("items")]
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
        public string Id
        { get; set; }


        [JsonProperty("name")]
        public string Name
        { get; set; }

        [JsonProperty("location")]
        public AddressLocation Location
        { get; set; }

        [JsonProperty("contact")]
        public Contact Contact
        { get; set; }


        [JsonProperty("verified")]
        public bool Verified
        { get; set; }


        public Category PrimaryCategory
        {
            get
            {
                return
                    (from cat in Categories
                     where cat.Primary == true
                     select cat).SingleOrDefault();
            }
        }




        [JsonProperty("stats")]
        public Status Status
        { get; set; }


        [JsonProperty("mayor")]
        public Mayorship Mayor
        { get; set; }

        [JsonProperty("distance")]
        public int Distance
        { get; set; }

        [JsonProperty("tips")]
        public TipGroupCollection TipGroups
        { get; set; }

        [JsonProperty("categories")]
        public Category[] Categories
        { get; set; }

        internal class SpecialsGroup
        {
            public SpecialsGroup(){}
            [JsonProperty("items")]
            public Special[] Specials
            { get; set; }
        }

        [JsonProperty("specials")]
        private SpecialsGroup _SpecialsGroup  { get; set; }

        public Special[] Specials
        {
            get
            {
                if (_SpecialsGroup == null)
                    return null;
                else
                    return _SpecialsGroup.Specials;
            }
            set
            {
                if (_SpecialsGroup == null)
                    _SpecialsGroup = new SpecialsGroup();
                _SpecialsGroup.Specials = value;
            }
        }

        


        [JsonProperty("tags")]
        public string[] Tags
        { get; set; }

        [JsonProperty("herenow")]
        public HereNow HereNow
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
            if (Location != null)
                venue.Location = new AddressLocation()
                {
                    Address = Location.Address,
                    CrossStreet = Location.CrossStreet,
                    City = Location.City,
                    State = Location.State,
                    ZipCode = Location.ZipCode,
                    Latitude = Location.Latitude,
                    Longitude = Location.Longitude
                };

            if (Contact != null)
                venue.Contact = new FourSquare.Contact()
                {
                    Phone = Contact.Phone
                };

            //venue.Twitter = Twitter;
            venue.Status = Status;
            venue.Distance = Distance;
            venue.TipGroups = TipGroups;
            venue.Categories = Categories;
            venue.Specials = Specials;
            venue.Tags = Tags;
            venue.HereNow = HereNow;
            venue.Mayor = Mayor;
            venue.Verified = Verified;
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

    class Contact
    {
        [JsonProperty("phone")]
        public string Phone
        { get; set; }

        [JsonProperty("email")]
        public string Email
        { get; set; }

        [JsonProperty("twitter")]
        public string Twitter
        { get; set; }

        [JsonProperty("facebook")]
        public string Facebook
        { get; set; }
    }
    class AddressLocation
    {

        [JsonProperty("address")]
        public string Address
        { get; set; }

        [JsonProperty("crossStreet")]
        public string CrossStreet
        { get; set; }

        [JsonProperty("city")]
        public string City
        { get; set; }

        [JsonProperty("state")]
        public string State
        { get; set; }

        [JsonProperty("postalCode")]
        public string ZipCode
        { get; set; }

        [JsonProperty("country")]
        public string Country
        { get; set; }

        [JsonProperty("lat")]
        public double Latitude
        { get; set; }

        [JsonProperty("lng")]
        public double Longitude
        { get; set; }
    }

    class Category
    {

        [JsonProperty("id")]
        public string Id
        { get; set; }

        [JsonProperty("name")]
        public string FullName
        { get; set; }

        [JsonProperty("parents")]
        public string[] Parents
        { get; set; }

        [JsonProperty("primary")]
        public bool Primary
        { get; set; }

        [JsonProperty("icon")]
        public Image IconUrl
        { get; set; }

        public override string ToString()
        {
            return FullName;
        }

    }



    class Status
    {

        //[JsonProperty("herenow")]
        //public int HereNow
        //{ get; set; }

        [JsonProperty("checkinsCount")]
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



    delegate void FlagEventHandler(object sender, FlagEventArgs e);
    class FlagEventArgs : EnvelopeEventArgs<object>
    {
        public bool Success
        { get { return this.Response == null || string.Equals(this.Response.ToString(), "ok"); } }
    }

    [JsonObject]
    class HereNow : Collection<CheckIn>
    {
        private class HereNowGroup
        {
            [JsonProperty("items")]
            public CheckIn[] CheckIns
            { get; set; }
        }

        [JsonProperty("groups")]
        private HereNowGroup[] Groups
        {
            get { return null; }
            set
            {
                this.Clear();
                foreach (var g in value)
                    foreach (var i in g.CheckIns)
                        this.Add(i);
            }
        }
    }



}
