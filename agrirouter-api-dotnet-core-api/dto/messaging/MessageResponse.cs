using com.dke.data.agrirouter.api.dto.messaging.inner;

namespace com.dke.data.agrirouter.api.dto.messaging
{
    /**
     * Data transfer object for the communication.
     */
    public class MessageResponse
    {
        public string SensorAlternateId { get; set; }

        public string CapabilityAlternateId { get; set; }

        public Command Command { get; set; }
    }
}