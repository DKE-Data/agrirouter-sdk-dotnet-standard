using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.messaging.abstraction;
using Agrirouter.Request;

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
        /// <param name="httpMessagingService">-</param>
        /// <param name="encodeMessageService">-</param>
        public PublishMultipleMessagesService(HttpMessagingService httpMessagingService,
            EncodeMessageService encodeMessageService) : base(httpMessagingService, encodeMessageService)
        {
        }

        protected override RequestEnvelope.Types.Mode Mode => RequestEnvelope.Types.Mode.Publish;
    }
}