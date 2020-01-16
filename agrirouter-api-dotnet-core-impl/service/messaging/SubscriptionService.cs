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
    /// <summary>
    /// Service to send the subscriptions.
    /// </summary>
    public class SubscriptionService : ISubscriptionService
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        /// <param name="encodeMessageService">-</param>
        public SubscriptionService(MessagingService messagingService, EncodeMessageService encodeMessageService)
        {
            _messagingService = messagingService;
            _encodeMessageService = encodeMessageService;
        }

        /// <summary>
        /// Please see <seealso cref="IMessagingService{T}.Send"/> for documentation.
        /// </summary>
        /// <param name="subscriptionParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(SubscriptionParameters subscriptionParameters)
        {
            var encodedMessages = new List<string> {Encode(subscriptionParameters).Content};
            var messagingParameters = subscriptionParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        /// <summary>
        /// Please see <seealso cref="IEncodeMessageService{T}.Encode"/> for documentation.
        /// </summary>
        /// <param name="subscriptionParameters">-</param>
        /// <returns>-</returns>
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