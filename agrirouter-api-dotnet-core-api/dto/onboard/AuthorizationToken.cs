using Newtonsoft.Json;

namespace com.dke.data.agrirouter.api.dto.onboard
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class AuthorizationToken
    {
        [JsonProperty(PropertyName = "account")]
        public string Account { get; set; }

        [JsonProperty(PropertyName = "regcode")]
        public string RegistrationCode { get; set; }

        [JsonProperty(PropertyName = "expires")]
        public string Expires { get; set; }
    }
}