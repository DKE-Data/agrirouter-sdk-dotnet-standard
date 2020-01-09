using System;
using System.Collections.Generic;
using Agrirouter.Request;
using Agrirouter.Request.Payload.Account;
using Agrirouter.Request.Payload.Endpoint;
using com.dke.data.agrirouter.api.definitions;
using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.service.messaging;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.impl.service.common;
using Google.Protobuf;

namespace com.dke.data.agrirouter.impl.service.messaging
{
    public class ListEndpointsService : IListEndpointsService
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        public ListEndpointsService(MessagingService messagingService)
        {
            _messagingService = messagingService;
            _encodeMessageService = new EncodeMessageService();
        }

        public string Send(ListEndpointsParameters listEndpointsParameters)
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
                TechnicalMessageType = TechnicalMessageTypes.DkeListEndpoints,
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
    }
}