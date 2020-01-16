using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.api.service.parameters.inner;

namespace com.dke.data.agrirouter.api.service.messaging
{
    /// <summary>
    /// Service to send multiple messages.
    /// </summary>
    public interface ISendMultipleMessagesService : IMessagingService<SendMultipleMessagesParameters>,
        IEncodeMessageService<MultipleMessageEntry>
    {
    }
}