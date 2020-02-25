using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Commons;
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
            var encodedMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(sendMessageParameters.Base64MessageContent))
            {
                throw new CouldNotSendMessageException(HttpStatusCode.BadRequest,
                    "Sending empty messages does not make any sense.");
            }
            else
            {
                if (MessageHasToBeChunked(sendMessageParameters))
                {
                    var chunkContextId = Guid.NewGuid().ToString();
                    var totalSize = Encoding.Unicode.GetByteCount(sendMessageParameters.Base64MessageContent);

                    var chunkedMessages = ChunkMessageContent(sendMessageParameters.Base64MessageContent,
                        sendMessageParameters.ChunkSize > 0
                            ? sendMessageParameters.ChunkSize
                            : ChunkSizeDefinition.MaximumSupported);

                    var current = 0;
                    foreach (var chunkedMessage in chunkedMessages)
                    {
                        var sendMessageParametersDuplicate = new SendChunkedMessageParameters
                        {
                            Recipients = sendMessageParameters.Recipients,
                            TypeUrl = sendMessageParameters.TypeUrl,
                            TechnicalMessageType = sendMessageParameters.TechnicalMessageType,
                            ApplicationMessageId = MessageIdService.ApplicationMessageId()
                        };
                        var chunkComponent = new ChunkComponent
                        {
                            Current = current++,
                            Total = chunkedMessage.Length,
                            ContextId = chunkContextId,
                            TotalSize = totalSize
                        };
                        sendMessageParametersDuplicate.ChunkInfo = chunkComponent;
                        sendMessageParametersDuplicate.Base64MessageContent = chunkedMessage;
                        encodedMessages.Add(Encode(sendMessageParametersDuplicate).Content);
                    }
                }
                else
                {
                    encodedMessages = new List<string> {Encode(sendMessageParameters).Content};
                }

                var messagingParameters = sendMessageParameters.BuildMessagingParameter(encodedMessages);
                return _messagingService.Send(messagingParameters);
            }
        }

        /// <summary>
        /// Checks whether a message has to be chunked or not.
        /// </summary>
        /// <param name="sendMessageParameters"></param>
        /// <returns></returns>
        public bool MessageHasToBeChunked(SendMessageParameters sendMessageParameters)
        {
            var base64MessageContent = sendMessageParameters.Base64MessageContent;
            var byteCount = Encoding.Unicode.GetByteCount(base64MessageContent);
            return byteCount / ChunkSizeDefinition.MaximumSupported > 1;
        }

        /// <summary>
        /// Chunk the message content to be sure, that the message will not be rejected by the AR.
        /// </summary>
        /// <param name="base64MessageContent">Content of the message.</param>
        /// <param name="chunkSize">Size of the chunks, <seealso cref="ChunkSizeDefinition"/></param>
        /// <returns>-</returns>
        /// <exception cref="ChunkSizeNotSupportedException">-</exception>
        public IEnumerable<string> ChunkMessageContent(string base64MessageContent, int chunkSize)
        {
            if (chunkSize > ChunkSizeDefinition.MaximumSupported)
            {
                throw new ChunkSizeNotSupportedException();
            }

            var chunkedMessages = new List<string>();
            while (base64MessageContent.Length > chunkSize)
            {
                chunkedMessages.Add(base64MessageContent.Substring(0, chunkSize));
                base64MessageContent = base64MessageContent.Substring(chunkSize);
            }

            chunkedMessages.Add(base64MessageContent);

            return chunkedMessages;
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
        /// Please see <seealso cref="IEncodeMessageService{T}.Encode"/> for documentation.
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

        protected abstract RequestEnvelope.Types.Mode Mode { get; }
    }
}