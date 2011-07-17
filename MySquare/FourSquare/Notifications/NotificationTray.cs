using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MySquare.FourSquare
{
    class NotificationTray : INotification
    {
        [JsonProperty("unreadCount")]
        public int UnreadCount { get; set; }
    }
}
