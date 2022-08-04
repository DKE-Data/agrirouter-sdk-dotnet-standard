using System;
using System.Collections.Generic;
using System.Text;

namespace Agrirouter.Api.Dto.Onboard.Inner
{
    public class RouterDeviceConnectionCriteria
    {
        public string ClientId { get; set; } = null!;

        public string GatewayId { get; set; } = null!;

        public string Host { get; set; } = null!;

        public int Port { get; set; }
    }

}
