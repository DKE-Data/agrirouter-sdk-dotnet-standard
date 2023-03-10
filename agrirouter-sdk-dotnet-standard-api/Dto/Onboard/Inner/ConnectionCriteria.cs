using Newtonsoft.Json;

namespace Agrirouter.Api.Dto.Onboard.Inner
{
    /// <summary>
    ///     Data transfer object for the communication.
    /// </summary>
    public class ConnectionCriteria
    {
        /// <summary>
        ///     Gateway ID.
        /// </summary>
        [JsonProperty(PropertyName = "gatewayId")]
        public string GatewayId { get; set; }

        /// <summary>
        ///     Measures.
        /// </summary>
        [JsonProperty(PropertyName = "measures")]
        public string Measures { get; set; }

        /// <summary>
        ///     Commands.
        /// </summary>
        [JsonProperty(PropertyName = "commands")]
        public string Commands { get; set; }

        /// <summary>
        ///     Host.
        /// </summary>
        [JsonProperty(PropertyName = "host")]
        public string Host { get; set; }

        /// <summary>
        ///     Port.
        /// </summary>
        [JsonProperty(PropertyName = "port")]
        public string Port { get; set; }

        /// <summary>
        ///     Client ID.
        /// </summary>
        [JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }
    }
}