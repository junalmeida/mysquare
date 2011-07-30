using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace MySquare.FourSquare
{
    class ScoreNotification : ReadOnlyCollection<Score>, INotification
    {
        public ScoreNotification(Score[] scores)
            : base(scores)
        {
        }
    }

    class Score : INotification
    {
        [JsonProperty("points")]
        public int Points
        { get; set; }

        [JsonProperty("recent")]
        public int Recent
        { get; set; }

        [JsonProperty("max")]
        public int Max
        { get; set; }

        [JsonProperty("checkinsCount")]
        public int CheckinsCount
        { get; set; }


        [JsonProperty("icon")]
        public string ImageUrl
        { get; set; }

        [JsonProperty("message")]
        public string Message
        { get; set; }

        public override string ToString()
        {
            return string.Format("{0} (+{1})", Message, Points);
        }
    }
}