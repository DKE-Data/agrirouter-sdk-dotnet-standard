using Newtonsoft.Json;

namespace com.dke.data.agrirouter.api.dto.onboard
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class OnboardingRequest
    {
        [JsonProperty(PropertyName = "id")] public string Id { get; set; }

        [JsonProperty(PropertyName = "applicationId")]
        public string ApplicationId { get; set; }

        [JsonProperty(PropertyName = "certificationVersionId")]
        public string CertificationVersionId { get; set; }

        [JsonProperty(PropertyName = "gatewayId")]
        public string GatewayId { get; set; }

        [JsonProperty(PropertyName = "UTCTimestamp")]
        public string UTCTimestamp { get; set; }

        [JsonProperty(PropertyName = "timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty(PropertyName = "certificateType")]
        public string CertificateType { get; set; }
    }
}