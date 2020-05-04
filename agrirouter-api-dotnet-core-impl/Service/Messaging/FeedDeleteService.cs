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
    /// Please see <seealso cref="IFeedDeleteService"/> for documentation.
    /// </summary>
    public class FeedDeleteService : IFeedDeleteService
    {
        private readonly HttpMessagingService _httpMessagingService;
        private readonly EncodeMessageService _encodeMessageService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpMessagingService">-</param>
        /// <param name="encodeMessageService">-</param>
        public FeedDeleteService(HttpMessagingService httpMessagingService, EncodeMessageService encodeMessageService)
        {
            _httpMessagingService = httpMessagingService;
            _encodeMessageService = encodeMessageService;
        }

        /// <summary>
        /// Please see <seealso cref="IMessagingService{T}.Send"/> for documentation.
        /// </summary>
        /// <param name="feedDeleteParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(FeedDeleteParameters feedDeleteParameters)
        {
            var encodedMessages = new List<string> {Encode(feedDeleteParameters).Content};
            var messagingParameters = feedDeleteParameters.BuildMessagingParameter(encodedMessages);
            return _httpMessagingService.Send(messagingParameters);
        }

        /// <summary>
        /// Please see <seealso cref="IEncodeMessageService{T}.Encode"/> for documentation.
        /// </summary>
        /// <param name="feedDeleteParameters">-</param>
        /// <returns>-</returns>
        public EncodedMessage Encode(FeedDeleteParameters feedDeleteParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = feedDeleteParameters.ApplicationMessageId,
                TeamSetContextId = feedDeleteParameters.TeamsetContextId ?? "",
                TechnicalMessageType = TechnicalMessageTypes.DkeFeedConfirm,
                Mode = RequestEnvelope.Types.Mode.Direct
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = MessageDelete.Descriptor.FullName
            };

            var messageDelete = new MessageDelete();
            feedDeleteParameters.Senders?.ForEach(sender => messageDelete.Senders.Add(sender));
            feedDeleteParameters.MessageIds?.ForEach(messageId => messageDelete.MessageIds.Add(messageId));
            feedDeleteParameters.ValidityPeriod = feedDeleteParameters.ValidityPeriod;

            messagePayloadParameters.Value = messageDelete.ToByteString();

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = EncodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}