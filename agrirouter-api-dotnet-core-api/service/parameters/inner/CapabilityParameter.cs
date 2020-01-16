using Agrirouter.Request.Payload.Endpoint;

namespace com.dke.data.agrirouter.api.service.parameters.inner
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