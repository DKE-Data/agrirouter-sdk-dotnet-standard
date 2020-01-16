using com.dke.data.agrirouter.api.service.parameters;

namespace com.dke.data.agrirouter.api.service.messaging
{
    /// <summary>
    /// Service to send messages.
    /// </summary>
    public interface ISendMessageService : IMessagingService<SendMessageParameters>,
        IEncodeMessageService<SendMessageParameters>
    {
    }
}