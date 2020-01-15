using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.dke.data.agrirouter.api.dto.onboard
{
    /**
     * Data transfer object for the communication.
     */
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