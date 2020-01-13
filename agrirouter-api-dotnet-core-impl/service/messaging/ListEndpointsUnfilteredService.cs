using com.dke.data.agrirouter.api.definitions;
using com.dke.data.agrirouter.impl.service.common;
using com.dke.data.agrirouter.impl.service.messaging.abstraction;

namespace com.dke.data.agrirouter.impl.service.messaging
{
    public class ListEndpointsUnfilteredService : ListEndpointsBaseService
    {
        public ListEndpointsUnfilteredService(MessagingService messagingService, EncodeMessageService encodeMessageService) : base(messagingService, encodeMessageService)
        {
        }

        protected override string TechnicalMessageType => TechnicalMessageTypes.DkeListEndpointsUnfiltered;
    }
}