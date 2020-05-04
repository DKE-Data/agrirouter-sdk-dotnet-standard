using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.messaging.abstraction;
using Agrirouter.Request;

namespace Agrirouter.Impl.Service.messaging
{
    /// <summary>
    /// Service to send multiple messages directly.
    /// </summary>
    public class SendMultipleDirectMessagesService : SendMultipleMessagesBaseService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpMessagingService">-</param>
        /// <param name="encodeMessageService">-</param>
        public SendMultipleDirectMessagesService(HttpMessagingService httpMessagingService,
            EncodeMessageService encodeMessageService) : base(httpMessagingService, encodeMessageService)
        {
        }

        protected override RequestEnvelope.Types.Mode Mode => RequestEnvelope.Types.Mode.Direct;
    }
}