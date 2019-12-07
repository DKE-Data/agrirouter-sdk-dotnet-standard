using System.Collections.Generic;
using com.dke.data.agrirouter.api.dto.messaging.inner;

namespace com.dke.data.agrirouter.api.dto.messaging
{
    public class MessageRequest
    {
        public string SensorAlternateId { get; set; }

        public string CapabilityAlternateId { get; set; }

        public List<Message> Messages { get; set; }
    }
}