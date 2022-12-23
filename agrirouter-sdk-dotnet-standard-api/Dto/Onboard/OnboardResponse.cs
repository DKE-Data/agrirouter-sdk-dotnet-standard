using Agrirouter.Api.Dto.Onboard.Inner;
using Newtonsoft.Json;

namespace Agrirouter.Api.Dto.Onboard
{
    /// <summary>
    ///     Data transfer object for the communication.
    /// </summary>
    public class OnboardResponse
    {
        /// <summary>
        ///     Device alternate ID.
        /// </summary>
        [JsonProperty(PropertyName = "deviceAlternateId")]
        public string DeviceAlternateId { get; set; }

        /// <summary>
        ///     Capability alternate ID.
        /// </summary>
        [JsonProperty(PropertyName = "capabilityAlternateId")]
        public string CapabilityAlternateId { get; set; }

        /// <summary>
        ///     Sensor alternate ID.
        /// </summary>
        [JsonProperty(PropertyName = "sensorAlternateId")]
        public string SensorAlternateId { get; set; }

        /// <summary>
        ///     Connection criteria.
        /// </summary>
        [JsonProperty(PropertyName = "connectionCriteria")]
        public ConnectionCriteria ConnectionCriteria { get; set; }

        /// <summary>
        ///     Authentication.
        /// </summary>
        [JsonProperty(PropertyName = "authentication")]
        public Authentication Authentication { get; set; }
    }
}