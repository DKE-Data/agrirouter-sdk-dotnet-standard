using System;
using System.Collections.Generic;
using Agrirouter.Request;
using com.dke.data.agrirouter.api.definitions;
using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.service.messaging;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.api.service.parameters.inner;
using com.dke.data.agrirouter.impl.service.common;
using Google.Protobuf;

namespace com.dke.data.agrirouter.impl.service.messaging.abstraction
{
    public abstract class SendMultipleMessagesBaseService : ISendMultipleMessagesService
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        protected SendMultipleMessagesBaseService(MessagingService messagingService,
            EncodeMessageService encodeMessageService)
        {
            _messagingService = messagingService;
            _encodeMessageService = encodeMessageService;
        }

        /// <summary>
        /// Please see <see cref="MessagingService.Send"/> for documentation.
        /// </summary>
        /// <param name="sendMultipleMessagesParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(SendMultipleMessagesParameters sendMultipleMessagesParameters)
        {
            List<string> encodedMessages = new List<string>();
            foreach (var sendMessageParameters in sendMultipleMessagesParameters.MultipleMessageEntries)
            {
                var encodedMessage = Encode(sendMessageParameters).Content;
                encodedMessages.Add(encodedMessage);
            }

            var messagingParameters = sendMultipleMessagesParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        /// <summary>
        /// Please see <seealso cref="IEncodeMessageService{T}.Encode"/> for documentation.
        /// </summary>
        /// <param name="multipleMessageEntry">-</param>
        /// <returns>-</returns>
        public EncodedMessage Encode(MultipleMessageEntry multipleMessageEntry)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = multipleMessageEntry.ApplicationMessageId,
                TeamSetContextId = multipleMessageEntry.TeamsetContextId ?? "",
                TechnicalMessageType = multipleMessageEntry.TechnicalMessageType,
                Mode = Mode,
                Recipients = multipleMessageEntry.Recipients,
                ChunkInfo = multipleMessageEntry.ChunkInfo
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = multipleMessageEntry.TypeUrl ?? TechnicalMessageTypes.Empty,
                Value = ByteString.FromBase64(multipleMessageEntry.Base64MessageContent)
            };

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = _encodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }

        protected abstract RequestEnvelope.Types.Mode Mode { get; }
    }
}