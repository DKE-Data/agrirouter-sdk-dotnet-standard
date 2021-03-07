using Agrirouter.Request.Payload.Endpoint;

namespace Agrirouter.Api.Service.Parameters.Inner
{
    /// <summary>
    ///     Parameter container definition.
    /// </summary>
    public class CapabilityParameter
    {
        /// <summary>
        ///     Technical message type.
        /// </summary>
        public string TechnicalMessageType { get; set; }

        /// <summary>
        ///     Direction.
        /// </summary>
        public CapabilitySpecification.Types.Direction Direction { get; set; }
    }
}