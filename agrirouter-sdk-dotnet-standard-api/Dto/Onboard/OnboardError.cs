using System.Collections.Generic;
using Newtonsoft.Json;

namespace Agrirouter.Api.Dto.Onboard
{
    /// <summary>
    ///     Data transfer object for the onboard and revoke error response.
    /// </summary>
    public class OnboardErrorResponse {
        /// <summary>
        ///     The error.
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        public OnboardError OnboardError { get; set; }
    }
    /// <summary>
    ///     Data transfer object for the onboard and revoke error.
    /// </summary>
    public class OnboardError
    {
        /// <summary>
        ///     Error code.
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        /// <summary>
        ///     Error Message.
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        ///     Error target.
        /// </summary>
        [JsonProperty(PropertyName = "target")]
        public string Target { get; set; }

        /// <summary>
        ///     Error details.
        /// </summary>
        [JsonProperty(PropertyName = "details")]
        public List<OnboardError> Details { get; set; }
    }
}
