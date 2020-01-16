using Newtonsoft.Json;

namespace com.dke.data.agrirouter.api.dto.messaging.inner
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