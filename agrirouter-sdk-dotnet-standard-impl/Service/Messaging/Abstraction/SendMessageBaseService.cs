using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Agrirouter.Commons;
using Agrirouter.Request;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Google.Protobuf;
using Base64 = Org.BouncyCastle.Utilities.Encoders.Base64;

namespace Agrirouter.Impl.Service.Messaging.Abstraction
{
    public abstract class SendMessageBaseService : ISendMessageService
    {
        private readonly IMessagingService<MessagingParameters> _messagingService;

        protected SendMessageBaseService(IMessagingService<MessagingParameters> messagingService)
        {
            _messagingService = messagingService;
        }

        protected abstract RequestEnvelope.Types.Mode Mode { get; }

        /// <summary>
        ///     Please see base class declaration for documentation.
        /// </summary>
        /// <param name="messagingParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(MessagingParameters messagingParameters)
        {
            return _messagingService.Send(messagingParameters);
        }

        /// <summary>
        ///     Please see base class declaration for documentation.
        /// </summary>
        /// <param name="messagingParameters">-</param>
        /// <returns>-</returns>
        public Task<MessagingResult> SendAsync(MessagingParameters messagingParameters)
        {
            return _messagingService.SendAsync(messagingParameters);
        }

        /// <summary>
        ///     Please see base class declaration for documentation.
        /// </summary>
        /// <param name="sendMessageParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(SendMessageParameters sendMessageParameters)
        {
            return _messagingService.Send(BuildMessagingParameters(sendMessageParameters));
        }

        /// <summary>
        ///     Please see base class declaration for documentation.
        /// </summary>
        /// <param name="sendMessageParameters">-</param>
        /// <returns>-</returns>
        public Task<MessagingResult> SendAsync(SendMessageParameters sendMessageParameters)
        {
            return _messagingService.SendAsync(BuildMessagingParameters(sendMessageParameters));
        }

        private MessagingParameters BuildMessagingParameters(SendMessageParameters sendMessageParameters)
        {
            var encodedMessages = new List<string> { Encode(sendMessageParameters).Content };
            var messagingParameters = sendMessageParameters.BuildMessagingParameter(encodedMessages);
            return messagingParameters;
        }

        /// <summary>
        ///     Please see base class declaration for documentation.
        /// </summary>
        /// <param name="sendMessageParameters">-</param>
        /// <returns>-</returns>
        public Task<MessagingResult> SendAsync(SendProtobufMessageParameters sendMessageParameters)
        {
            if (sendMessageParameters.ProtobufMessageContent == null)
            {
                throw new CouldNotSendEmptyMessageException("Sending empty messages does not make any sense.");
            }

            var encodedMessages = new List<string> { Encode(sendMessageParameters).Content };

            var messagingParameters = sendMessageParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.SendAsync(messagingParameters);
        }

        /// <summary>
        ///     Please see <seealso cref="IEncodeMessageService{T}.Encode" /> for documentation.
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
                Metadata = sendMessageParameters.Metadata,
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
                Content = EncodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }

        /// <summary>
        ///     Please see <seealso cref="IEncodeMessageService{T}.Encode" /> for documentation.
        /// </summary>
        /// <param name="sendMessageParameters">-</param>
        /// <returns>-</returns>
        public EncodedMessage Encode(SendProtobufMessageParameters sendMessageParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = sendMessageParameters.ApplicationMessageId,
                TeamSetContextId = sendMessageParameters.TeamsetContextId ?? "",
                TechnicalMessageType = sendMessageParameters.TechnicalMessageType,
                Mode = Mode,
                Recipients = sendMessageParameters.Recipients,
                Metadata = sendMessageParameters.Metadata,
                ChunkInfo = sendMessageParameters.ChunkInfo
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = sendMessageParameters.TypeUrl ?? TechnicalMessageTypes.Empty,
                Value = sendMessageParameters.ProtobufMessageContent
            };

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = EncodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }

        /// <summary>
        ///     Please see <seealso cref="IEncodeMessageService{T}.Encode" /> for documentation.
        /// </summary>
        /// <param name="sendMessageParameters">-</param>
        /// <returns>-</returns>
        public EncodedMessage Encode(SendChunkedMessageParameters sendMessageParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = sendMessageParameters.ApplicationMessageId,
                TeamSetContextId = sendMessageParameters.TeamsetContextId ?? "",
                TechnicalMessageType = sendMessageParameters.TechnicalMessageType,
                Mode = Mode,
                Recipients = sendMessageParameters.Recipients,
                Metadata = sendMessageParameters.Metadata,
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
                Content = EncodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}