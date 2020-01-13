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
    public class FeedDeleteService : IFeedDeleteService
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        public FeedDeleteService(MessagingService messagingService)
        {
            _messagingService = messagingService;
            _encodeMessageService = new EncodeMessageService();
        }

        public MessagingResult Send(FeedDeleteParameters feedDeleteParameters)
        {
            var encodedMessages = new List<string> {Encode(feedDeleteParameters).Content};
            var messagingParameters = feedDeleteParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

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
                Content = _encodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}