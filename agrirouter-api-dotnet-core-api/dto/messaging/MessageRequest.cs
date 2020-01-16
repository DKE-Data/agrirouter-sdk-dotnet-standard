using System.Collections.Generic;
using Newtonsoft.Json;

namespace Agrirouter.Api.Dto.Messaging
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class MessageRequest
    {
        [JsonProperty("sensorAlternateId")] public string SensorAlternateId { get; set; }

        [JsonProperty("capabilityAlternateId")]
        public string CapabilityAlternateId { get; set; }

        [JsonProperty("measures")] public List<Inner.Message> Messages { get; set; }
    }
}