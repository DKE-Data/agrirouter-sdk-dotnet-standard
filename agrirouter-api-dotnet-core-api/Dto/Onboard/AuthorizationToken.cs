using Newtonsoft.Json;

namespace Agrirouter.Api.Dto.Onboard
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class AuthorizationToken
    {
        /// <summary>
        /// Account.
        /// </summary>
        [JsonProperty(PropertyName = "account")]
        public string Account { get; set; }

        /// <summary>
        /// Registration code.
        /// </summary>
        [JsonProperty(PropertyName = "regcode")]
        public string RegistrationCode { get; set; }

        /// <summary>
        /// Expiry date.
        /// </summary>
        [JsonProperty(PropertyName = "expires")]
        public string Expires { get; set; }
    }
}