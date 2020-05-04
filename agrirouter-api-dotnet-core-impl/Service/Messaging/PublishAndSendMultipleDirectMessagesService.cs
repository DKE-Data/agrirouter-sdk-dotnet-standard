using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Messaging.Abstraction;
using Agrirouter.Request;

namespace Agrirouter.Impl.Service.Messaging
{
    /// <summary>
    /// Service to publish and send multiple messages.
    /// </summary>
    public class PublishAndSendMultipleDirectMessagesService : SendMultipleMessagesBaseService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        public PublishAndSendMultipleDirectMessagesService(IMessagingService<MessagingParameters> messagingService) : base(messagingService)
        {
        }

        protected override RequestEnvelope.Types.Mode Mode => RequestEnvelope.Types.Mode.PublishWithDirect;
    }
}