using System;
using System.Collections.Generic;
using Agrirouter.Request;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Sdk.Api.Definitions;
using Agrirouter.Sdk.Api.Dto.Messaging;
using Agrirouter.Sdk.Api.Service.Messaging;
using Agrirouter.Sdk.Api.Service.Parameters;
using Agrirouter.Sdk.Impl.Service.Common;
using Google.Protobuf;

namespace Agrirouter.Sdk.Impl.Service.Messaging
{
    /// <summary>
    ///     Service to send the subscriptions.
    /// </summary>
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IMessagingService<MessagingParameters> _messagingService;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        /// <param name="encodeMessageService">-</param>
        public SubscriptionService(IMessagingService<MessagingParameters> messagingService)
        {
            _messagingService = messagingService;
        }

        /// <summary>
        ///     Please see <seealso cref="IMessagingService{T}.Send" /> for documentation.
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
        ///     Please see <seealso cref="IEncodeMessageService{T}.Encode" /> for documentation.
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
                Content = EncodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}