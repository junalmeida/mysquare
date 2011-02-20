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

        [JsonProperty("firstname")]
        public string FirstName
        { get; set; }

        [JsonProperty("lastname")]
        public string LastName
        { get; set; }

        [JsonProperty("photo")]
        public string ImageUrl
        { get; set; }

        [JsonProperty("gender")]
        public string Gender
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

        [JsonProperty("friendstatus")]
        public FriendStatus? FriendStatus
        { get; set; }

        [JsonProperty("badges")]
        public Badge[] Badges
        { get; set; }

        [JsonProperty("status")]
        public Status Status
        { get; set; }

        [JsonProperty("checkin")]
        public CheckIn CheckIn
        { get; set; }

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
            user.Email = Email;
            user.Twitter = Twitter;
            user.Facebook = Facebook;
            user.FriendStatus = FriendStatus;
            user.Badges = Badges;
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
    class UserEventArgs : EventArgs
    {
        public UserEventArgs()
        {
            Accepted = null;
        }

        [JsonProperty("user")]
        public User User
        {
            get;
            set;
        }

        public bool? Accepted
        { get; set; }
    }


    delegate void FriendsEventHandler(object serder, FriendsEventArgs e);
    class FriendsEventArgs : EventArgs
    {
        [JsonProperty("friends")]
        public User[] Friends
        {
            get;
            private set;
        }
    }

    enum FriendStatus
    {
        friend,
        pendingyou,
        pendingthem,
        followingthem,
        self
    }


    delegate void PendingFriendsEventHandler(object serder, PendingFriendsEventArgs e);
    class PendingFriendsEventArgs : EventArgs
    {
        [JsonProperty("requests")]
        public User[] Friends
        {
            get;
            private set;
        }
    }
}
