using System;

namespace com.dke.data.agrirouter.api.dto.onboard.inner
{
    /**
     * Data transfer object for the communication.
     */
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