using System;
using System.Collections.Generic;
using Agrirouter.Feed.Request;
using Agrirouter.Feed.Response;
using Agrirouter.Request;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace Agrirouter.Impl.Service.messaging.abstraction
{
    public abstract class QueryMessageBaseService : IQueryMessagesService,
        IDecodeMessageResponseService<MessageQueryResponse.Types.FeedMessage>
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        protected QueryMessageBaseService(MessagingService messagingService, EncodeMessageService encodeMessageService)
        {
            _messagingService = messagingService;
            _encodeMessageService = encodeMessageService;
        }

        /// <summary>
        /// Please see <see cref="MessagingService.Send"/> for documentation.
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
        /// Please see <seealso cref="IEncodeMessageService{T}.Encode"/> for documentation.
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
                Content = _encodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }

        protected abstract string TechnicalMessageType { get; }

        public MessageQueryResponse.Types.FeedMessage Decode(Any messageResponse)
        {
            try
            {
                return MessageQueryResponse.Types.FeedMessage.Parser.ParseFrom(messageResponse.ToByteString());
            }
            catch (Exception e)
            {
                throw new CouldNotDecodeMessageException("Could not decode query message header response.", e);
            }
        }
    }
}