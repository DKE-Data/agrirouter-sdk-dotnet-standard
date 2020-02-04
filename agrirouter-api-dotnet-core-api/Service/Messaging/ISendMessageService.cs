using Agrirouter.Api.Service.Parameters;

namespace Agrirouter.Api.Service.Messaging
{
    /// <summary>
    /// Service to send messages.
    /// </summary>
    public interface ISendMessageService : IMessagingService<SendMessageParameters>,
        IEncodeMessageService<SendMessageParameters>
    {
    }
}