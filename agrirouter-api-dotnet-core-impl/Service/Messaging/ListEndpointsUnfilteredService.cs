using Agrirouter.Api.Definitions;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.messaging.abstraction;

namespace Agrirouter.Impl.Service.messaging
{
    /// <summary>
    /// Service to list the endpoints connected to an endpoint.
    /// </summary>
    public class ListEndpointsUnfilteredService : ListEndpointsBaseService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpMessagingService"></param>
        /// <param name="encodeMessageService"></param>
        public ListEndpointsUnfilteredService(HttpMessagingService httpMessagingService,
            EncodeMessageService encodeMessageService) : base(httpMessagingService, encodeMessageService)
        {
        }

        protected override string TechnicalMessageType => TechnicalMessageTypes.DkeListEndpointsUnfiltered;
    }
}