using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    class LeaderboardUser
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("scores")]
        public Score Scores { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }
    }


    delegate void LeaderboardEventHandler(object serder, LeaderboardEventArgs e);
    class LeaderboardEventArgs : EnvelopeEventArgs<LeaderboardResult>
    {
        public LeaderboardEventArgs() { }

        public LeaderboardUser[] Leaderboard { get { return this.Response.Data.Leaderboard; } }
    }

    class LeaderboardResult
    {
        public class LB
        {
            [JsonProperty("items")]
            public LeaderboardUser[] Leaderboard { get; set; }
        }

        [JsonProperty("leaderboard")]
        public LB Data { get; set; }

    }

}
