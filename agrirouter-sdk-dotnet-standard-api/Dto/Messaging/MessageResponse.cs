using Agrirouter.Sdk.Api.Dto.Messaging.Inner;

namespace Agrirouter.Sdk.Api.Dto.Messaging
{
    /// <summary>
    ///     Data transfer object for the communication.
    /// </summary>
    public class MessageResponse
    {
        /// <summary>
        ///     Sensor alternate ID.
        /// </summary>
        public string SensorAlternateId { get; set; }

        /// <summary>
        ///     Capability alternate ID.
        /// </summary>
        public string CapabilityAlternateId { get; set; }

        /// <summary>
        ///     Command.
        /// </summary>
        public Command Command { get; set; }
    }
}