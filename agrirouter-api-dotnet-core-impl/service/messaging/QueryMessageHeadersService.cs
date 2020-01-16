using Agrirouter.Api.Definitions;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.messaging.abstraction;

namespace Agrirouter.Impl.Service.messaging
{
    /// <summary>
    /// Service to query message headers.
    /// </summary>
    public class QueryMessageHeadersService : QueryMessageBaseService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        /// <param name="encodeMessageService">-</param>
        public QueryMessageHeadersService(MessagingService messagingService, EncodeMessageService encodeMessageService)
            : base(messagingService, encodeMessageService)
        {
        }

        protected override string TechnicalMessageType => TechnicalMessageTypes.DkeFeedHeaderQuery;
    }
}