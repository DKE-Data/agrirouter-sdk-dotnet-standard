using System;
using System.Collections.Generic;
using Agrirouter.Feed.Request;
using Agrirouter.Feed.Response;
using Agrirouter.Request;
using com.dke.data.agrirouter.api.definitions;
using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.service.messaging;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.impl.service.common;
using Google.Protobuf;

namespace com.dke.data.agrirouter.impl.service.messaging
{
    public class QueryMessageHeadersService : IQueryMessagesService
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        public QueryMessageHeadersService(MessagingService messagingService)
        {
            _messagingService = messagingService;
            _encodeMessageService = new EncodeMessageService();
        }

        public string Send(QueryMessagesParameters queryMessagesParameters)
        {
            var encodedMessages = new List<string> {Encode(queryMessagesParameters).Content};
            var messagingParameters = queryMessagesParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        public EncodedMessage Encode(QueryMessagesParameters queryMessagesParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = queryMessagesParameters.ApplicationMessageId,
                TeamSetContextId = queryMessagesParameters.TeamsetContextId ?? "",
                TechnicalMessageType = TechnicalMessageTypes.DkeFeedHeaderQuery,
                Mode = RequestEnvelope.Types.Mode.Direct
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = MessageQuery.Descriptor.FullName
            };

            var messageQuery = new MessageQuery();
            if (null != queryMessagesParameters.Senders)
            {
                foreach (var sender in queryMessagesParameters.Senders)
                {
                    messageQuery.Senders.Add(sender);
                }
            }
            if (null != queryMessagesParameters.MessageIds)
            {
                foreach (var messageId in queryMessagesParameters.MessageIds)
                {
                    messageQuery.MessageIds.Add(messageId);
                }
            }
            messageQuery.ValidityPeriod = queryMessagesParameters.ValidityPeriod;
            messagePayloadParameters.Value = messageQuery.ToByteString();

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = _encodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}