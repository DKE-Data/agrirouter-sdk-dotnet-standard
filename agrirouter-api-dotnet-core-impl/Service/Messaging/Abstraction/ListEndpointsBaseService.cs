using System;
using System.Collections.Generic;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Request;
using Agrirouter.Request.Payload.Account;
using Google.Protobuf;

namespace Agrirouter.Impl.Service.messaging.abstraction
{
    /// <summary>
    /// Abstraction of the service to list endpoints to avoid multiple implementations.
    /// </summary>
    public abstract class ListEndpointsBaseService : IListEndpointsService
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        /// <summary>
        // Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        /// <param name="encodeMessageService">-</param>
        protected ListEndpointsBaseService(MessagingService messagingService, EncodeMessageService encodeMessageService)
        {
            _messagingService = messagingService;
            _encodeMessageService = encodeMessageService;
        }

        /// <summary>
        /// Please see <see cref="MessagingService.Send"/> for documentation.
        /// </summary>
        /// <param name="listEndpointsParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(ListEndpointsParameters listEndpointsParameters)
        {
            var encodedMessages = new List<string> {Encode(listEndpointsParameters).Content};
            var messagingParameters = listEndpointsParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        /// <summary>
        /// Please see <seealso cref="IEncodeMessageService{T}.Encode"/> for documentation.
        /// </summary>
        /// <param name="listEndpointsParameters">-</param>
        /// <returns>-</returns>
        public EncodedMessage Encode(ListEndpointsParameters listEndpointsParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = listEndpointsParameters.ApplicationMessageId,
                TeamSetContextId = listEndpointsParameters.TeamsetContextId ?? "",
                TechnicalMessageType = TechnicalMessageType,
                Mode = RequestEnvelope.Types.Mode.Direct
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = ListEndpointsQuery.Descriptor.FullName
            };

            var listEndpointsQuery = new ListEndpointsQuery {Direction = listEndpointsParameters.Direction};

            if (null != listEndpointsParameters.TechnicalMessageType)
            {
                listEndpointsQuery.TechnicalMessageType = listEndpointsParameters.TechnicalMessageType;
            }

            messagePayloadParameters.Value = listEndpointsQuery.ToByteString();

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = _encodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }

        protected abstract string TechnicalMessageType { get; }
    }
}