using Newtonsoft.Json;

namespace Agrirouter.Api.Dto.Messaging.Inner
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Content.
        /// </summary>
        [JsonProperty("message")]
        public string Content { get; set; }

        /// <summary>
        /// Timestamp.
        /// </summary>
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}