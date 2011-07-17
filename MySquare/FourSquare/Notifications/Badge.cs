using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MySquare.FourSquare
{
    class Badge : INotification
    {
        [JsonProperty("id")]
        public string Id
        { get; set; }

        [JsonProperty("name")]
        public string Name
        { get; set; }

        [JsonProperty("image")]
        public Image ImageUrl
        { get; set; }

        [JsonProperty("description")]
        public string Description
        { get; set; }

        [JsonProperty("hint")]
        public string Hint
        { get; set; }


        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Description);
        }
    }


    delegate void BadgesEventHandler(object serder, BadgesEventArgs e);
    class BadgesEventArgs : EnvelopeEventArgs<BadgesResult>
    {
        public Badge[] Badges
        {
            get { return this.Response.Badges; }
        }
    }

    class BadgesResult
    {
        [JsonConverter(typeof(BadgeConverter))]
        public Badge[] Badges
        {
            get;
            set;
        }

    }

    class BadgeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Badge[]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            var badges = new List<Badge>();

            foreach (var item in obj.Values())
            {
                Badge badge = new Badge() ;
                serializer.Populate(item.CreateReader(), badge);
                badges.Add(badge);
            }

            return badges.ToArray();
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
