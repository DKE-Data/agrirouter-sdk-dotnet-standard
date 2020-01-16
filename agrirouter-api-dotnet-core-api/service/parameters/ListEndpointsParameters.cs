using Agrirouter.Request.Payload.Account;

namespace Agrirouter.Api.Service.Parameters
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