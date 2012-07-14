using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    class User
    {
        internal bool fullData;

        [JsonProperty("id")]
        public string Id
        { get; set; }

        [JsonProperty("firstName")]
        public string FirstName
        { get; set; }

        [JsonProperty("lastName")]
        public string LastName
        { get; set; }

        public string ImageUrl
        {
            get
            {
                if (_ImageUrl == null)
                    return string.Empty;
                else
                {
                    return _ImageUrl.ToString();
                }
            }
            set
            {
                _ImageUrl = new Image(value);
            }
        }

        [JsonProperty("photo")]
        private Image _ImageUrl
        {
            get;
            set;
        }

        [JsonProperty("gender")]
        public string Gender
        { get; set; }

        [JsonProperty("contact")]
        public Contact Contact
        { get; set; }


        [JsonProperty("relationship")]
        public FriendStatus? FriendStatus
        { get; set; }


        [JsonProperty("status")]
        public Status Status
        { get; set; }

        [JsonProperty("checkin")]
        public CheckIn CheckIn
        { get; set; }

        [JsonIgnore]
        public Badge[] Badges
        { get; set; }

        [JsonIgnore]
        public User[] Friends
        { get; set; }

        public override string ToString()
        {
            return string.Concat(FirstName, " ", LastName);
        }


        internal void CopyTo(User user)
        {
            user.fullData = fullData;
            user.Id = Id;
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.ImageUrl = ImageUrl;
            user.Gender = Gender;
            user.Contact = Contact;
            user.FriendStatus = FriendStatus;
            //user.Badges = Badges;
            user.Status = Status;
            user.CheckIn = CheckIn;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return Id == ((User)obj).Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    delegate void UserEventHandler(object serder, UserEventArgs e);
    class UserEventArgs :  EnvelopeEventArgs<UserResult>
    {
        public UserEventArgs()
        {
            Accepted = null;
        }

        public User User
        {
            get { return this.Response.User; }
            set
            {
                if (Response == null)
                    Response = new UserResult();
                this.Response.User = value;
            }
        }

        public bool? Accepted
        { get; set; }
    }

    class UserResult
    {
        [JsonProperty("user")]
        public User User
        {
            get;
            set;
        }
    }


    delegate void FriendsEventHandler(object serder, FriendsEventArgs e);
    class FriendsEventArgs : EnvelopeEventArgs<FriendsResult>
    {
        public User[] Friends
        {
            get { return this.Response.Friends.Items; }
        }
    }

    class FriendsResult
    {
        internal class FriendsItems
        {

            [JsonProperty("items")]
            public User[] Items
            {
                get;
                private set;
            }
        }
        [JsonProperty("friends")]
        public FriendsItems Friends
        {
            get;
            private set;
        }
    }

    enum FriendStatus
    {
        friend,
        pendingYou,
        pendingThem,
        pendingMe,
        followingThem,
        self
    }


    delegate void PendingFriendsEventHandler(object serder, PendingFriendsEventArgs e);
    class PendingFriendsEventArgs : EnvelopeEventArgs<PendingFriendsResult>
    {
        public User[] Friends
        {
            get { return this.Response.Friends; }
        }
    }

    class PendingFriendsResult
    {
        [JsonProperty("requests")]
        public User[] Friends
        {
            get;
            private set;
        }
    }
}
