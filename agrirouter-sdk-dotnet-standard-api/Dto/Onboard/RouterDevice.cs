using Agrirouter.Api.Dto.Onboard.Inner;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agrirouter.Api.Dto.Onboard
{
    public class RouterDevice
    {
    
        public Authentication Authentication { get; set; } = null!;

        public string DeviceAlternateId { get; set; } = null!;

        public RouterDeviceConnectionCriteria ConnectionCriteria { get; set; } = null!;
    }
}
