using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Messaging.Abstraction;
using Agrirouter.Request;

namespace Agrirouter.Impl.Service.Messaging
{
    /// <summary>
    ///     Service to send messages directly.
    /// </summary>
    public class SendDirectMessageService : SendMessageBaseService
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        public SendDirectMessageService(IMessagingService<MessagingParameters> messagingService) :
            base(messagingService)
        {
        }

        protected override RequestEnvelope.Types.Mode Mode => RequestEnvelope.Types.Mode.Direct;
    }
}