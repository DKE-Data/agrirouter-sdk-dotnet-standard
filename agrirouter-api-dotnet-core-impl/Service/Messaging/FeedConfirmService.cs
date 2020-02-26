using System;
using System.Collections.Generic;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Feed.Request;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Request;
using Google.Protobuf;

namespace Agrirouter.Impl.Service.messaging
{
    /// <summary>
    /// Please see <seealso cref="IFeedConfirmService"/> for documentation.
    /// </summary>
    public class FeedConfirmService : IFeedConfirmService
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        /// <param name="encodeMessageService">-</param>
        public FeedConfirmService(MessagingService messagingService, EncodeMessageService encodeMessageService)
        {
            _messagingService = messagingService;
            _encodeMessageService = encodeMessageService;
        }

        /// <summary>
        /// Please see <seealso cref="IMessagingService{T}.Send"/> for documentation.
        /// </summary>
        /// <param name="feedConfirmParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(FeedConfirmParameters feedConfirmParameters)
        {
            var encodedMessages = new List<string> {Encode(feedConfirmParameters).Content};
            var messagingParameters = feedConfirmParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        /// <summary>
        /// Please see <seealso cref="IEncodeMessageService{T}.Encode"/> for documentation.
        /// </summary>
        /// <param name="feedConfirmParameters">-</param>
        /// <returns>-</returns>
        public EncodedMessage Encode(FeedConfirmParameters feedConfirmParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = feedConfirmParameters.ApplicationMessageId,
                TeamSetContextId = feedConfirmParameters.TeamsetContextId ?? "",
                TechnicalMessageType = TechnicalMessageTypes.DkeFeedConfirm,
                Mode = RequestEnvelope.Types.Mode.Direct
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = MessageConfirm.Descriptor.FullName
            };

            var messageConfirm = new MessageConfirm();
            feedConfirmParameters.MessageIds?.ForEach(messageId => messageConfirm.MessageIds.Add(messageId));

            messagePayloadParameters.Value = messageConfirm.ToByteString();

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = EncodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}