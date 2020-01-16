using System.Collections.Generic;
using Newtonsoft.Json;

namespace Agrirouter.Api.Dto.Onboard
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class RevokeRequest
    {
        [JsonProperty(PropertyName = "accountId")]
        public string AccountId { get; set; }

        [JsonProperty(PropertyName = "endpointIds")]
        public List<string> EndpointIds { get; set; }

        [JsonProperty(PropertyName = "UTCTimestamp")]
        public string UTCTimestamp { get; set; }

        [JsonProperty(PropertyName = "timeZone")]
        public string TimeZone { get; set; }
    }
}