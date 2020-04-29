using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;

namespace Agrirouter.Api.Service.Messaging
{
    /// <summary>
    /// Service to send multiple messages. The messages will not be chunked automatically.
    /// </summary>
    public interface ISendMultipleMessagesService : IMessagingService<SendMultipleMessagesParameters>,
        IEncodeMessageService<MultipleMessageEntry>
    {
    }
}