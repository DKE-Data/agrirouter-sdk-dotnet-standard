using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;

namespace Agrirouter.Api.Service.Messaging
{
    /// <summary>
    /// Service to send multiple messages.
    /// </summary>
    public interface ISendMultipleMessagesService : IMessagingService<SendMultipleMessagesParameters>,
        IEncodeMessageService<MultipleMessageEntry>
    {
    }
}