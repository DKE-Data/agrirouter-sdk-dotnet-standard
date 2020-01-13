using System;
using System.Collections.Generic;
using Agrirouter.Feed.Request;
using Agrirouter.Request;
using com.dke.data.agrirouter.api.definitions;
using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.service.messaging;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.impl.service.common;
using Google.Protobuf;

namespace com.dke.data.agrirouter.impl.service.messaging
{
    public class FeedConfirmService : IFeedConfirmService
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        public FeedConfirmService(MessagingService messagingService)
        {
            _messagingService = messagingService;
            _encodeMessageService = new EncodeMessageService();
        }

        public MessagingResult Send(FeedConfirmParameters feedConfirmParameters)
        {
            var encodedMessages = new List<string> {Encode(feedConfirmParameters).Content};
            var messagingParameters = feedConfirmParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

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
                Content = _encodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}