using System;
using com.dke.data.agrirouter.api.dto.onboard.inner;
using Newtonsoft.Json;

namespace com.dke.data.agrirouter.api.dto.onboard
{
    /**
     * The response after sending an onboarding request.
     */
    public class OnboardingResponse
    {
        public String DeviceAlternateId { get; set; }

        public String CapabilityAlternateId { get; set; }

        public String SensorAlternateId { get; set; }

        [JsonProperty(PropertyName = "connectionCriteria")]
        public ConnectionCriteria ConnectionCriteria { get; set; }

        [JsonProperty(PropertyName = "authentication")]
        public Authentication Authentication { get; set; }
    }
}