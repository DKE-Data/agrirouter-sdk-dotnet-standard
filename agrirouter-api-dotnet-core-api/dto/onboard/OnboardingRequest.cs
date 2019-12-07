using System;
using Newtonsoft.Json;

namespace com.dke.data.agrirouter.api.dto.onboard
{
    /**
     * Request which is used to onbooard an endpoint while not using the secured onboarding process.
     */
    public class OnboardingRequest
    {
        [JsonProperty(PropertyName = "id")]
        public String Id { get; set; }

        [JsonProperty(PropertyName = "applicationId")]
        public String ApplicationId { get; set; }

        [JsonProperty(PropertyName = "certificationVersionId")]
        public String CertificationVersionId { get; set; }

        [JsonProperty(PropertyName = "gatewayId")]
        public String GatewayId { get; set; }

        [JsonProperty(PropertyName = "UTCTimestamp")]
        public String UTCTimestamp { get; set; }

        [JsonProperty(PropertyName = "timeZone")]
        public String TimeZone { get; set; }

        [JsonProperty(PropertyName = "certificateType")]
        public String CertificateType { get; set; }
    }
}