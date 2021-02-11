using System;
using System.Collections.Generic;
using Agrirouter.Feed.Request;
using Agrirouter.Request;
using Agrirouter.Sdk.Api.Dto.Messaging;
using Agrirouter.Sdk.Api.Service.Messaging;
using Agrirouter.Sdk.Api.Service.Parameters;
using Agrirouter.Sdk.Impl.Service.Common;
using Google.Protobuf;

namespace Agrirouter.Sdk.Impl.Service.Messaging.Abstraction
{
    public abstract class QueryMessageBaseService : IQueryMessagesService
    {
        private readonly IMessagingService<MessagingParameters> _messagingService;

        protected QueryMessageBaseService(IMessagingService<MessagingParameters> messagingService)
        {
            _messagingService = messagingService;
        }

        protected abstract string TechnicalMessageType { get; }

        /// <summary>
        ///     Please see base class declaration for documentation.
        /// </summary>
        /// <param name="queryMessagesParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(QueryMessagesParameters queryMessagesParameters)
        {
            var encodedMessages = new List<string> {Encode(queryMessagesParameters).Content};
            var messagingParameters = queryMessagesParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        /// <summary>
        ///     Please see <seealso cref="IEncodeMessageService{T}.Encode" /> for documentation.
        /// </summary>
        /// <param name="queryMessagesParameters">-</param>
        /// <returns>-</returns>
        public EncodedMessage Encode(QueryMessagesParameters queryMessagesParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = queryMessagesParameters.ApplicationMessageId,
                TeamSetContextId = queryMessagesParameters.TeamsetContextId ?? "",
                TechnicalMessageType = TechnicalMessageType,
                Mode = RequestEnvelope.Types.Mode.Direct
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = MessageQuery.Descriptor.FullName
            };

            var messageQuery = new MessageQuery();
            queryMessagesParameters.Senders?.ForEach(sender => messageQuery.Senders.Add(sender));
            queryMessagesParameters.MessageIds?.ForEach(messageId => messageQuery.MessageIds.Add(messageId));
            messageQuery.ValidityPeriod = queryMessagesParameters.ValidityPeriod;

            messagePayloadParameters.Value = messageQuery.ToByteString();

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = EncodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}