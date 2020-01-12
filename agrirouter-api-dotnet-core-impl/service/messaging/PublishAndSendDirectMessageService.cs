using Agrirouter.Request;
using com.dke.data.agrirouter.impl.service.common;
using com.dke.data.agrirouter.impl.service.messaging.abstraction;

namespace com.dke.data.agrirouter.impl.service.messaging
{
    public class PublishAndSendMessageService : SendMessageBaseService
    {
        public PublishAndSendMessageService(MessagingService messagingService) : base(messagingService)
        {
        }

        public override RequestEnvelope.Types.Mode Mode => RequestEnvelope.Types.Mode.PublishWithDirect;
    }
}