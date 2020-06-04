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
        public string GatewayId { get; set; }

        /// <summary>
        ///     Measures.
        /// </summary>
        public string Measures { get; set; }

        /// <summary>
        ///     Commands.
        /// </summary>
        public string Commands { get; set; }

        /// <summary>
        ///     Host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        ///     Port.
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        ///     Client ID.
        /// </summary>
        public string ClientId { get; set; }
    }
}