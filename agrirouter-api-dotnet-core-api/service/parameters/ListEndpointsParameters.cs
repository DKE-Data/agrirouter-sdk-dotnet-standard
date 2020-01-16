using Agrirouter.Request.Payload.Account;

namespace com.dke.data.agrirouter.api.service.parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class ListEndpointsParameters : MessageParameters
    {
        public ListEndpointsQuery.Types.Direction Direction { get; set; }

        public string TechnicalMessageType { get; set; }
    }
}