using Agrirouter.Api.Service.Parameters;

namespace Agrirouter.Api.Service.Messaging
{
    /// <summary>
    ///     Service to send messages. Message which are too big to be proceeded by the AR will be chunked automatically.
    /// </summary>
    public interface ISendMessageService : IMessagingService<MessagingParameters>,
        IMessagingService<SendMessageParameters>, IEncodeMessageService<SendMessageParameters>
    {
    }
}