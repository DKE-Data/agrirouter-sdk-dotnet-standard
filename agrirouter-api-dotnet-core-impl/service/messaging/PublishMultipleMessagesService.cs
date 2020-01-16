using Agrirouter.Request;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.messaging.abstraction;

namespace Agrirouter.Impl.Service.messaging
{
    /// <summary>
    /// Service to publish multiple messages.
    /// </summary>
    public class PublishMultipleMessagesService : SendMultipleMessagesBaseService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        /// <param name="encodeMessageService">-</param>
        public PublishMultipleMessagesService(MessagingService messagingService,
            EncodeMessageService encodeMessageService) : base(messagingService, encodeMessageService)
        {
        }

        protected override RequestEnvelope.Types.Mode Mode => RequestEnvelope.Types.Mode.Publish;
    }
}