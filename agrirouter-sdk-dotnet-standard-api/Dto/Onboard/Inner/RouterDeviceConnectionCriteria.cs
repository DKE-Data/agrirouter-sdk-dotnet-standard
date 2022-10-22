namespace Agrirouter.Api.Dto.Onboard.Inner
{
    /// <summary>
    /// Connection criteria for router devices.
    /// </summary>
    public class RouterDeviceConnectionCriteria
    {
        /// <summary>
        /// The client ID.
        /// </summary>
        public string ClientId { get; set; } = null!;

        /// <summary>
        /// The gateway ID.
        /// </summary>
        public string GatewayId { get; set; } = null!;

        /// <summary>
        /// The host.
        /// </summary>
        public string Host { get; set; } = null!;

        /// <summary>
        /// The port.
        /// </summary>
        public string Port { get; set; }
    }
}