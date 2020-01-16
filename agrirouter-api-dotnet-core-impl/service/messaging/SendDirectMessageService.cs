using Agrirouter.Request;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.messaging.abstraction;

namespace Agrirouter.Impl.Service.messaging
{
    /// <summary>
    /// Service to send messages directly.
    /// </summary>
    public class SendDirectMessageService : SendMessageBaseService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        /// <param name="encodeMessageService">-</param>
        public SendDirectMessageService(MessagingService messagingService, EncodeMessageService encodeMessageService) :
            base(messagingService, encodeMessageService)
        {
        }

        protected override RequestEnvelope.Types.Mode Mode => RequestEnvelope.Types.Mode.Direct;
    }
}