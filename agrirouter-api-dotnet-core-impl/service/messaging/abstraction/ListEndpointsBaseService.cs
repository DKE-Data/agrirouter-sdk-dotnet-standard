using System;
using System.Collections.Generic;
using Agrirouter.Request;
using Agrirouter.Request.Payload.Account;
using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.service.messaging;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.impl.service.common;
using Google.Protobuf;

namespace com.dke.data.agrirouter.impl.service.messaging.abstraction
{
    public abstract class ListEndpointsBaseService : IListEndpointsService
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        protected ListEndpointsBaseService(MessagingService messagingService)
        {
            _messagingService = messagingService;
            _encodeMessageService = new EncodeMessageService();
        }

        public MessagingResult Send(ListEndpointsParameters listEndpointsParameters)
        {
            var encodedMessages = new List<string> {Encode(listEndpointsParameters).Content};
            var messagingParameters = listEndpointsParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

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