namespace Agrirouter.Api.Dto.Onboard.Inner
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class ConnectionCriteria
    {
        public string GatewayId { get; set; }

        public string Measures { get; set; }

        public string Commands { get; set; }

        public string Host { get; set; }

        public string Port { get; set; }

        public string ClientId { get; set; }
    }
}