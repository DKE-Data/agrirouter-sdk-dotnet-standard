using System;

namespace com.dke.data.agrirouter.api.dto.onboard.inner
{
    public class ConnectionCriteria
    {
        String GatewayId { get; set; }

        String Measures { get; set; }

        String Commands { get; set; }

        String Host { get; set; }

        String Port { get; set; }

        String ClientId { get; set; }
    }
}