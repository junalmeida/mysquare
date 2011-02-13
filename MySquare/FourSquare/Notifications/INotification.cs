using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace MySquare.FourSquare
{
    interface INotification
    {
    }

    class NotificationMessage :  INotification
    {
        [JsonProperty("message")]
        public string Message { get; private set; }
    }

    class NotificationConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(INotification);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            string discriminator = (string)obj["type"];

            INotification item;
            switch (discriminator)
            {
                case "message":
                    item = new NotificationMessage();
                    break;
                case "mayorship":
                    item = new MayorshipNotification();
                    break;
                case "score":
                    var scores = serializer.Deserialize<Score[]>(obj["item"]["scores"].CreateReader());
                    return new ScoreNotification(scores);
                    break;
                default:
                    throw new NotImplementedException();
            }

            serializer.Populate(obj["item"].CreateReader(), item);

            return item;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        //public override INotification Create(Type objectType)
        //{
        //    return new NotificationMessage();
        //    NotType type = serializer.Deserialize<NotType>(reader);

        //    string typeName = this.GetType().Namespace + ".Notification" + type.Type[0].ToString().ToUpper() + type.Type.Substring(1).ToLower();

        //    Type newType = this.GetType().Assembly.GetType(typeName);

         
        //    return (INotification)Activator.CreateInstance(newType);
        //}
    }
   
}
