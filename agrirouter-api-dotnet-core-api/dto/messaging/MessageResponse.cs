using Agrirouter.Commons;
using com.dke.data.agrirouter.api.dto.messaging.inner;

namespace com.dke.data.agrirouter.api.dto.messaging
{
    public class MessageResponse
    {
        public string SensorAlternateId { get; set; }

        public string CapabilityAlternateId { get; set; }

        public Command Command { get; set; }
    }
}