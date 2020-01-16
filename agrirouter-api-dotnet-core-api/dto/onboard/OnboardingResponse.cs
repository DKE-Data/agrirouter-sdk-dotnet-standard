using Agrirouter.Api.Dto.Onboard.Inner;
using Newtonsoft.Json;

namespace Agrirouter.Api.Dto.Onboard
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class OnboardingResponse
    {
        public string DeviceAlternateId { get; set; }

        public string CapabilityAlternateId { get; set; }

        public string SensorAlternateId { get; set; }

        [JsonProperty(PropertyName = "connectionCriteria")]
        public ConnectionCriteria ConnectionCriteria { get; set; }

        [JsonProperty(PropertyName = "authentication")]
        public Authentication Authentication { get; set; }
    }
}