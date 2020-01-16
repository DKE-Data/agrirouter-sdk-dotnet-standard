using Agrirouter.Api.Dto.Messaging.Inner;

namespace Agrirouter.Api.Dto.Messaging
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class MessageResponse
    {
        public string SensorAlternateId { get; set; }

        public string CapabilityAlternateId { get; set; }

        public Command Command { get; set; }
    }
}