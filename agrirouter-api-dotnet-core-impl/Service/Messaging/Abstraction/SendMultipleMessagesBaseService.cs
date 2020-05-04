using System;
using System.Linq;
using System.Text;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Request;
using Google.Protobuf;

namespace Agrirouter.Impl.Service.messaging.abstraction
{
    public abstract class SendMultipleMessagesBaseService : ISendMultipleMessagesService
    {
        private readonly HttpMessagingService _httpMessagingService;
        private readonly EncodeMessageService _encodeMessageService;

        protected SendMultipleMessagesBaseService(HttpMessagingService httpMessagingService,
            EncodeMessageService encodeMessageService)
        {
            _httpMessagingService = httpMessagingService;
            _encodeMessageService = encodeMessageService;
        }

        /// <summary>
        /// Please see <see cref="HttpMessagingService.Send"/> for documentation.
        /// </summary>
        /// <param name="sendMultipleMessagesParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(SendMultipleMessagesParameters sendMultipleMessagesParameters)
        {
            var encodedMessages = sendMultipleMessagesParameters.MultipleMessageEntries
                .Select(sendMessageParameters => Encode(sendMessageParameters).Content).ToList();
            var messagingParameters = sendMultipleMessagesParameters.BuildMessagingParameter(encodedMessages);
            return _httpMessagingService.Send(messagingParameters);
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
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = multipleMessageEntry.TypeUrl ?? TechnicalMessageTypes.Empty,
                Value = ByteString.FromBase64(multipleMessageEntry.Base64MessageContent)
            };

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = EncodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }

        /// <summary>
        /// Checks whether a message has to be chunked or not.
        /// </summary>
        /// <param name="sendMessageParameters"></param>
        /// <returns></returns>
        public void CheckWetherMessageShouldHaveBeenChunked(SendMessageParameters sendMessageParameters)
        {
            var base64MessageContent = sendMessageParameters.Base64MessageContent;
            var byteCount = Encoding.Unicode.GetByteCount(base64MessageContent);
            if (byteCount / ChunkSizeDefinition.MaximumSupported > 1)
            {
                throw new MessageShouldHaveBeenChunkedException();
            }

            ;
        }

        protected abstract RequestEnvelope.Types.Mode Mode { get; }
    }
}