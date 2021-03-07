using Agrirouter.Request;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Messaging.Abstraction;

namespace Agrirouter.Impl.Service.Messaging
{
    /// <summary>
    ///     Service to send multiple messages directly.
    /// </summary>
    public class SendMultipleDirectMessagesService : SendMultipleMessagesBaseService
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        public SendMultipleDirectMessagesService(IMessagingService<MessagingParameters> messagingService) : base(
            messagingService)
        {
        }

        protected override RequestEnvelope.Types.Mode Mode => RequestEnvelope.Types.Mode.Direct;
    }
}