using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    class User
    {
        [JsonProperty("id")]
        public int Id
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
        public string FriendStatus
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

        public override string ToString()
        {
            return string.Concat(FirstName, " ", LastName);
        }
    }

    delegate void UserEventHandler(object serder, UserEventArgs e);
    class UserEventArgs : EventArgs
    {

        [JsonProperty("user")]
        public User User
        {
            get;
            private set;
        }
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
}
