using Agrirouter.Api.Definitions;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.messaging.abstraction;

namespace Agrirouter.Impl.Service.messaging
{
    /// <summary>
    /// Service to list the endpoints connected to an endpoint.
    /// </summary>
    public class ListEndpointsService : ListEndpointsBaseService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messagingService"></param>
        public ListEndpointsService(IMessagingService<MessagingParameters> messagingService) :
            base(messagingService)
        {
        }

        protected override string TechnicalMessageType => TechnicalMessageTypes.DkeListEndpoints;
    }
}