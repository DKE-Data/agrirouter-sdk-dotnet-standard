using System.Collections.Generic;
using Newtonsoft.Json;

namespace Agrirouter.Sdk.Api.Dto.Messaging
{
    /// <summary>
    ///     Data transfer object for the communication.
    /// </summary>
    public class MessageRequest
    {
        /// <summary>
        ///     Sensor alternate ID.
        /// </summary>
        [JsonProperty("sensorAlternateId")]
        public string SensorAlternateId { get; set; }

        /// <summary>
        ///     Capability alternate ID.
        /// </summary>
        [JsonProperty("capabilityAlternateId")]
        public string CapabilityAlternateId { get; set; }

        /// <summary>
        ///     Messages.
        /// </summary>
        [JsonProperty("measures")]
        public List<Inner.Message> Messages { get; set; }
    }
}