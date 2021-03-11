using System.Collections.Generic;
using Newtonsoft.Json;

namespace Agrirouter.Api.Dto.Onboard
{
    /// <summary>
    ///     Data transfer object for the communication.
    /// </summary>
    public class RevokeRequest
    {
        /// <summary>
        ///     Account ID.
        /// </summary>
        [JsonProperty(PropertyName = "accountId")]
        public string AccountId { get; set; }

        /// <summary>
        ///     Endpoint IDs.
        /// </summary>
        [JsonProperty(PropertyName = "endpointIds")]
        public List<string> EndpointIds { get; set; }

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
    }
}