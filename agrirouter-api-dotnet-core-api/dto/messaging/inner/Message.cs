using Newtonsoft.Json;

namespace com.dke.data.agrirouter.api.dto.messaging.inner
{
    /**
     * Data transfer object for the communication.
     */
    public class Message
    {
        [JsonProperty("message")]
        public string Content{ get; set; }
        
        [JsonProperty("timestamp")]
        public string Timestamp{ get; set; }
    }
}