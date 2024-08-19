using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Feed.Request;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Request;
using Google.Protobuf;

namespace Agrirouter.Impl.Service.Messaging
{
    /// <summary>
    ///     Service to list the endpoints connected to an endpoint.
    /// </summary>
    public class PingService: IPingService
    {
        private readonly IMessagingService<MessagingParameters> _messagingService;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="messagingService"></param>
        public PingService(IMessagingService<MessagingParameters> messagingService)
        {
            _messagingService = messagingService;
        }

        /// <summary>
        ///     Please see base class declaration for documentation.
        /// </summary>
        /// <param name="pingParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(PingParameters pingParameters)
        {
            var encodedMessages = new List<string> {Encode(pingParameters).Content};
            var messagingParameters = pingParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        /// <summary>
        ///     Please see base class declaration for documentation.
        /// </summary>
        /// <param name="pingParameters">-</param>
        /// <returns>-</returns>
        public Task<MessagingResult> SendAsync(PingParameters pingParameters)
        {
            var encodedMessages = new List<string> {Encode(pingParameters).Content};
            var messagingParameters = pingParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.SendAsync(messagingParameters);
        }

        /// <summary>
        ///     Please see <seealso cref="IEncodeMessageService{T}.Encode" /> for documentation.
        /// </summary>
        /// <param name="pingParameters">-</param>
        /// <returns>-</returns>
        public EncodedMessage Encode(PingParameters pingParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = pingParameters.ApplicationMessageId,
                TeamSetContextId = pingParameters.TeamsetContextId ?? "",
                TechnicalMessageType = TechnicalMessageTypes.DkePing,
                Mode = RequestEnvelope.Types.Mode.Direct
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = "",
                Value = ByteString.Empty
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