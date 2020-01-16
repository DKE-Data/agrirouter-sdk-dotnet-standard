using Newtonsoft.Json;

namespace Agrirouter.Api.Dto.Messaging.Inner
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class Message
    {
        [JsonProperty("message")] public string Content { get; set; }

        [JsonProperty("timestamp")] public string Timestamp { get; set; }
    }
}