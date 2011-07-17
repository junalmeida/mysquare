using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
 
namespace MySquare.FourSquare
{
    abstract class EnvelopeEventArgs<T> : EventArgs
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("response")]
        public T Response { get; set; }

        [JsonProperty("notifications")]
        public INotification[] Notifications { get; set; }
    }

    class Meta
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("errorType")]
        public ErrorType ErrorType { get; set; }

        [JsonProperty("errorDetail")]
        public string Details { get; set; }
    }

    class Image
    {
        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("sizes")]
        public int[] Sizes { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public override string ToString()
        {
            if (Sizes != null && Sizes.Length > 0)
                return string.Format("{0}{1}{2}", Prefix, Sizes.Min(), Name);
            else
                return string.Empty;
        }
    }

    enum ErrorType
    {
        /// <summary>
        /// OAuth token was not provided or was invalid.
        /// </summary>
        invalid_auth,
        /// <summary>
        /// A required parameter was missing or a parameter was malformed. This is also used if the resource ID in the path is incorrect.
        /// </summary>
        param_error,
        /// <summary>
        /// The requested path does not exist.
        /// </summary>
        endpoint_error,
        /// <summary>
        /// Although authentication succeeded, the acting user is not allowed to see this information due to privacy restrictions.
        /// </summary>
        not_authorized,
        /// <summary>
        /// Rate limit for this hour exceeded.
        /// </summary>
        rate_limit_exceeded,
        /// <summary>
        /// Something about this request is using deprecated functionality, or the response format may be about to change.
        /// </summary>
        deprecated,
        /// <summary>
        /// Server is currently experiencing issues. Check status.foursquare.com for udpates.
        /// </summary>
        server_error,
        other
    }
}
