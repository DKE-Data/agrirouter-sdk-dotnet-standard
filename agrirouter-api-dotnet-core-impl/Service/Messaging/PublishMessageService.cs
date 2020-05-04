using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.messaging.abstraction;
using Agrirouter.Request;

namespace Agrirouter.Impl.Service.messaging
{
    /// <summary>
    /// Service to publish messages.
    /// </summary>
    public class PublishMessageService : SendMessageBaseService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        public PublishMessageService(IMessagingService<MessagingParameters> messagingService) : base(messagingService)
        {
        }

        protected override RequestEnvelope.Types.Mode Mode => RequestEnvelope.Types.Mode.Publish;
    }
}