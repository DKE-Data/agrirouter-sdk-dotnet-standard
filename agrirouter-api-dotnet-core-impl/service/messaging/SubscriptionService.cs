using System;
using System.Collections.Generic;
using Agrirouter.Request;
using Agrirouter.Request.Payload.Endpoint;
using com.dke.data.agrirouter.api.definitions;
using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.service.messaging;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.impl.service.common;
using Google.Protobuf;

namespace com.dke.data.agrirouter.impl.service.messaging
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        public SubscriptionService(MessagingService messagingService, EncodeMessageService encodeMessageService)
        {
            _messagingService = messagingService;
            _encodeMessageService = encodeMessageService;
        }

        public MessagingResult Send(SubscriptionParameters subscriptionParameters)
        {
            var encodedMessages = new List<string> {Encode(subscriptionParameters).Content};
            var messagingParameters = subscriptionParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        public EncodedMessage Encode(SubscriptionParameters subscriptionParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = subscriptionParameters.ApplicationMessageId,
                TeamSetContextId = subscriptionParameters.TeamsetContextId ?? "",
                TechnicalMessageType = TechnicalMessageTypes.DkeSubscription,
                Mode = RequestEnvelope.Types.Mode.Direct
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = Subscription.Descriptor.FullName
            };

            var subscription = new Subscription();
            subscriptionParameters.TechnicalMessageTypes?.ForEach(technicalMessageType =>
                subscription.TechnicalMessageTypes.Add(technicalMessageType));

            messagePayloadParameters.Value = subscription.ToByteString();

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = _encodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}