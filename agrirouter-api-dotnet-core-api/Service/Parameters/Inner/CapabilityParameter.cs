using Agrirouter.Request.Payload.Endpoint;

namespace Agrirouter.Api.Service.Parameters.Inner
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class CapabilityParameter
    {
        public string TechnicalMessageType { get; set; }

        public CapabilitySpecification.Types.Direction Direction { get; set; }
    }
}