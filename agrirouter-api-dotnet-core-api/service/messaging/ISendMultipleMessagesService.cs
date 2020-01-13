using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.service.parameters;

namespace com.dke.data.agrirouter.api.service.messaging
{
    public interface ISendMultipleMessagesService : IMessagingService<SendMultipleMessagesParameters>,
        IEncodeMessageService<SendMultipleMessagesParameters>
    {
    }
}