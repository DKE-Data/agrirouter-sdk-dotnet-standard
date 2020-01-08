using System;
using System.Collections.Generic;
using Agrirouter.Request;
using Agrirouter.Request.Payload.Endpoint;
using com.dke.data.agrirouter.api.definitions;
using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.service;
using com.dke.data.agrirouter.api.service.messaging;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.impl.service.common;
using Google.Protobuf;

namespace com.dke.data.agrirouter.impl.service.messaging
{
    /**
     * Service to send capabilites messages.
     */
    public class CapabilitiesService : ICapabilitiesServices
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        public CapabilitiesService(MessagingService messagingService)
        {
            _messagingService = messagingService;
            _encodeMessageService = new EncodeMessageService();
        }

        /**
         * Send capabilities message using the given parameters.
         */
        public string Send(CapabilitiesParameters capabilitiesParameters)
        {
            var encodedMessages = new List<string> {Encode(capabilitiesParameters).Content};
            var messagingParameters = new MessagingParameters
            {
                ApplicationMessageId = capabilitiesParameters.ApplicationMessageId,
                TeamsetContextId = capabilitiesParameters.TeamsetContextId,
                OnboardingResponse = capabilitiesParameters.OnboardingResponse, EncodedMessages = encodedMessages
            };

            return _messagingService.Send(messagingParameters);
        }

        private EncodedMessage Encode(CapabilitiesParameters capabilitiesParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = capabilitiesParameters.ApplicationMessageId,
                TeamSetContextId = capabilitiesParameters.TeamsetContextId ?? "",
                TechnicalMessageType = TechnicalMessageTypes.DkeCapabilities,
                Mode = RequestEnvelope.Types.Mode.Direct
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = CapabilitySpecification.Descriptor.FullName
            };

            var capabilitySpecification = new CapabilitySpecification
            {
                AppCertificationId = capabilitiesParameters.ApplicationId,
                AppCertificationVersionId = capabilitiesParameters.CertificationVersionId
            };
            capabilitiesParameters.CapabilityParameters.ForEach(capabilityParameter =>
            {
                var capability = new CapabilitySpecification.Types.Capability
                {
                    TechnicalMessageType = capabilityParameter.TechnicalMessageType,
                    Direction = capabilityParameter.Direction
                };
                capabilitySpecification.Capabilities.Add(capability);
            } );
            messagePayloadParameters.Value = capabilitySpecification.ToByteString();

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = _encodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}