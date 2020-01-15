using System;
using com.dke.data.agrirouter.api.dto.onboard.inner;
using Newtonsoft.Json;

namespace com.dke.data.agrirouter.api.dto.onboard
{
    /**
     * Data transfer object for the communication.
     */
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