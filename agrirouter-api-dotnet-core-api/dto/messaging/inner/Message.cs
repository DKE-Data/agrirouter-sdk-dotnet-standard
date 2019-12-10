using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace com.dke.data.agrirouter.api.dto.messaging.inner
{
    public class Message
    {
        [JsonProperty("message")]
        public string Content{ get; set; }
        
        [JsonProperty("timestamp")]
        public string Timestamp{ get; set; }
    }
}