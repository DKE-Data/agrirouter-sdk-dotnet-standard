using System;
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
            var encodedMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(sendMessageParameters.Base64MessageContent))
            {
                throw new CouldNotSendEmptyMessageException("Sending empty messages does not make any sense.");
            }

            if (MessageCanBeChunked(sendMessageParameters.TechnicalMessageType))
            {
                if (MessageHasToBeChunked(sendMessageParameters.Base64MessageContent))
                {
                    var chunkContextId = Guid.NewGuid().ToString();
                    var totalSize = Encoding.Unicode.GetByteCount(sendMessageParameters.Base64MessageContent);

                    var chunkedMessages = ChunkMessageContent(sendMessageParameters.Base64MessageContent,
                        sendMessageParameters.ChunkSize > 0
                            ? sendMessageParameters.ChunkSize
                            : ChunkSizeDefinition.MaximumSupported);

                    var current = 1;
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
            }
            else
            {
                encodedMessages = new List<string> {Encode(sendMessageParameters).Content};
            }

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
            
            var encodedMessages = new List<string> {Encode(sendMessageParameters).Content};

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
                Metadata = sendMessageParameters.Metadata
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
                Metadata = sendMessageParameters.Metadata
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

        private bool MessageCanBeChunked(string technicalMessageType)
        {
            return TechnicalMessageTypes.IsChunkable(technicalMessageType);
        }

        /// <summary>
        ///     Checks whether a message has to be chunked or not.
        /// </summary>
        /// <param name="base64MessageContent"></param>
        /// <returns></returns>
        public bool MessageHasToBeChunked(string base64MessageContent)
        {
            var byteCount = Encoding.Unicode.GetByteCount(base64MessageContent) / 4;
            return byteCount / ChunkSizeDefinition.MaximumSupported > 1;
        }

        /// <summary>
        ///     Chunk the message content to be sure, that the message will not be rejected by the AR.
        /// </summary>
        /// <param name="base64MessageContent">Content of the message.</param>
        /// <param name="chunkSize">Size of the chunks, <seealso cref="ChunkSizeDefinition" /></param>
        /// <returns>-</returns>
        /// <exception cref="ChunkSizeNotSupportedException">-</exception>
        public IEnumerable<string> ChunkMessageContent(string base64MessageContent, int chunkSize)
        {
            if (chunkSize > ChunkSizeDefinition.MaximumSupported) throw new ChunkSizeNotSupportedException();

            var chunks = new List<string>();
            var source = Encoding.UTF8.GetBytes(base64MessageContent);
            byte[] chunk;
            for (var i = 0; i < source.Length; i += chunkSize)
            {
                if (i + chunkSize > source.Length) continue;

                chunk = new byte[chunkSize];
                Array.Copy(source, i, chunk, 0, chunkSize);
                chunks.Add(Encoding.UTF8.GetString(chunk));
            }

            var lastSourceIndex = chunks.Count * chunkSize;
            chunk = new byte[source.Length - lastSourceIndex];
            Array.Copy(source, lastSourceIndex, chunk, 0, source.Length - lastSourceIndex);
            chunks.Add(Encoding.UTF8.GetString(chunk));

            return chunks;
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
                ChunkInfo = sendMessageParameters.ChunkInfo,
                Metadata = sendMessageParameters.Metadata,
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