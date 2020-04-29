using Agrirouter.Api.Definitions;
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
        /// <param name="encodeMessageService"></param>
        public ListEndpointsService(MessagingService messagingService, EncodeMessageService encodeMessageService) :
            base(messagingService, encodeMessageService)
        {
        }

        protected override string TechnicalMessageType => TechnicalMessageTypes.DkeListEndpoints;
    }
}