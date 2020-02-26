using Agrirouter.Request.Payload.Account;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class ListEndpointsParameters : MessageParameters
    {
        /// <summary>
        /// Direction.
        /// </summary>
        public ListEndpointsQuery.Types.Direction Direction { get; set; }

        /// <summary>
        /// Technical message type.
        /// </summary>
        public string TechnicalMessageType { get; set; }
    }
}