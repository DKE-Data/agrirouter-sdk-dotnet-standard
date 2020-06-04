using Agrirouter.Api.Definitions;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Messaging.Abstraction;

namespace Agrirouter.Impl.Service.Messaging
{
    /// <summary>
    ///     Service to list the endpoints connected to an endpoint.
    /// </summary>
    public class ListEndpointsUnfilteredService : ListEndpointsBaseService
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="messagingService"></param>
        public ListEndpointsUnfilteredService(IMessagingService<MessagingParameters> messagingService) : base(
            messagingService)
        {
        }

        protected override string TechnicalMessageType => TechnicalMessageTypes.DkeListEndpointsUnfiltered;
    }
}