using Newtonsoft.Json;

namespace Agrirouter.Sdk.Api.Dto.Onboard
{
    /// <summary>
    ///     Data transfer object for the communication.
    /// </summary>
    public class OnboardRequest
    {
        /// <summary>
        ///     External ID.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string ExternalId { get; set; }

        /// <summary>
        ///     Application ID.
        /// </summary>
        [JsonProperty(PropertyName = "applicationId")]
        public string ApplicationId { get; set; }

        /// <summary>
        ///     Certification version ID.
        /// </summary>
        [JsonProperty(PropertyName = "certificationVersionId")]
        public string CertificationVersionId { get; set; }

        /// <summary>
        ///     Gateway ID.
        /// </summary>
        [JsonProperty(PropertyName = "gatewayId")]
        public string GatewayId { get; set; }

        /// <summary>
        ///     Timestamp.
        /// </summary>
        [JsonProperty(PropertyName = "UTCTimestamp")]
        public string UtcTimestamp { get; set; }

        /// <summary>
        ///     Timezone.
        /// </summary>
        [JsonProperty(PropertyName = "timeZone")]
        public string TimeZone { get; set; }

        /// <summary>
        ///     Certification type.
        /// </summary>
        [JsonProperty(PropertyName = "certificateType")]
        public string CertificateType { get; set; }
    }
}