using System;
using System.Collections.Generic;
using Agrirouter.Request;
using com.dke.data.agrirouter.api.definitions;
using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.service.messaging;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.impl.service.common;
using Google.Protobuf;

namespace com.dke.data.agrirouter.impl.service.messaging.abstraction
{
    public abstract class SendMessageBaseService : ISendMessageService
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        protected SendMessageBaseService(MessagingService messagingService)
        {
            _messagingService = messagingService;
            _encodeMessageService = new EncodeMessageService();
        }

        public string Send(SendMessageParameters sendMessageParameters)
        {
            var encodedMessages = new List<string> {Encode(sendMessageParameters).Content};
            var messagingParameters = sendMessageParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        public EncodedMessage Encode(SendMessageParameters sendMessageParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = sendMessageParameters.ApplicationMessageId,
                TeamSetContextId = sendMessageParameters.TeamsetContextId ?? "",
                TechnicalMessageType = sendMessageParameters.TechnicalMessageType,
                Mode = Mode,
                Recipients = sendMessageParameters.Recipients,
                ChunkInfo = sendMessageParameters.ChunkInfo
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = sendMessageParameters.TypeUrl ?? TechnicalMessageTypes.Empty,
                Value = ByteString.FromBase64(sendMessageParameters.Base64MessageContent)
            };

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = _encodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }

        public abstract RequestEnvelope.Types.Mode Mode { get; }
    }
}