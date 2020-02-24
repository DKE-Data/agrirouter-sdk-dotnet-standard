using System;
using System.Collections.Generic;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Request;
using Google.Protobuf;

namespace Agrirouter.Impl.Service.messaging.abstraction
{
    public abstract class SendMessageBaseService : ISendMessageService
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        protected SendMessageBaseService(MessagingService messagingService, EncodeMessageService encodeMessageService)
        {
            _messagingService = messagingService;
            _encodeMessageService = encodeMessageService;
        }

        /// <summary>
        /// Please see <see cref="MessagingService.Send"/> for documentation.
        /// </summary>
        /// <param name="sendMessageParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(SendMessageParameters sendMessageParameters)
        {
            var encodedMessages = new List<string> {Encode(sendMessageParameters).Content};
            var messagingParameters = sendMessageParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        /// <summary>
        /// Please see <seealso cref="IEncodeMessageService{T}.Encode"/> for documentation.
        /// </summary>
        /// <param name="sendMessageParameters">-</param>
        /// <returns>-</returns>
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

        protected abstract RequestEnvelope.Types.Mode Mode { get; }
    }
}