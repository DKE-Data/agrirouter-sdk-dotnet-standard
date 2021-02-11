using Agrirouter.Request;
using Agrirouter.Sdk.Api.Service.Messaging;
using Agrirouter.Sdk.Api.Service.Parameters;
using Agrirouter.Sdk.Impl.Service.Messaging.Abstraction;

namespace Agrirouter.Sdk.Impl.Service.Messaging
{
    /// <summary>
    ///     Service to publish multiple messages.
    /// </summary>
    public class PublishMultipleMessagesService : SendMultipleMessagesBaseService
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        public PublishMultipleMessagesService(IMessagingService<MessagingParameters> messagingService) : base(
            messagingService)
        {
        }

        protected override RequestEnvelope.Types.Mode Mode => RequestEnvelope.Types.Mode.Publish;
    }
}