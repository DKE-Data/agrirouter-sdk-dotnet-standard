using System.Collections.Generic;
using com.dke.data.agrirouter.api.dto.messaging.inner;
using Newtonsoft.Json;

namespace com.dke.data.agrirouter.api.dto.messaging
{
    /**
     * Data transfer object for the communication.
     */
    public class MessageRequest
    {
        [JsonProperty("sensorAlternateId")]
        public string SensorAlternateId { get; set; }

        [JsonProperty("capabilityAlternateId")]
        public string CapabilityAlternateId { get; set; }

        [JsonProperty("measures")]
        public List<Message> Messages { get; set; }
    }
}