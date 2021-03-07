using System;
using System.Linq;
using System.Text;
using Agrirouter.Request;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Impl.Service.Common;
using Google.Protobuf;

namespace Agrirouter.Impl.Service.Messaging.Abstraction
{
    public abstract class SendMultipleMessagesBaseService : ISendMultipleMessagesService
    {
        private readonly IMessagingService<MessagingParameters> _messagingService;

        protected SendMultipleMessagesBaseService(IMessagingService<MessagingParameters> messagingService)
        {
            _messagingService = messagingService;
        }

        protected abstract RequestEnvelope.Types.Mode Mode { get; }

        /// <summary>
        ///     Please see base class declaration for documentation.
        /// </summary>
        /// <param name="sendMultipleMessagesParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(SendMultipleMessagesParameters sendMultipleMessagesParameters)
        {
            var encodedMessages = sendMultipleMessagesParameters.MultipleMessageEntries
                .Select(sendMessageParameters => Encode(sendMessageParameters).Content).ToList();
            var messagingParameters = sendMultipleMessagesParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        /// <summary>
        ///     Please see <seealso cref="IEncodeMessageService{T}.Encode" /> for documentation.
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
                Recipients = multipleMessageEntry.Recipients
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
        ///     Checks whether a message has to be chunked or not.
        /// </summary>
        /// <param name="sendMessageParameters"></param>
        /// <returns></returns>
        public void CheckWetherMessageShouldHaveBeenChunked(SendMessageParameters sendMessageParameters)
        {
            var base64MessageContent = sendMessageParameters.Base64MessageContent;
            var byteCount = Encoding.Unicode.GetByteCount(base64MessageContent);
            if (byteCount / ChunkSizeDefinition.MaximumSupported > 1) throw new MessageShouldHaveBeenChunkedException();
        }
    }
}