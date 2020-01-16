using com.dke.data.agrirouter.api.definitions;
using com.dke.data.agrirouter.impl.service.common;
using com.dke.data.agrirouter.impl.service.messaging.abstraction;

namespace com.dke.data.agrirouter.impl.service.messaging
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