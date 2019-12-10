using System;
using System.Collections.Generic;
using Agrirouter.Request;
using Agrirouter.Request.Payload.Endpoint;
using com.dke.data.agrirouter.api.definitions;
using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.service;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.impl.service.common;
using Google.Protobuf;

namespace com.dke.data.agrirouter.impl.service.messaging
{
    public class CapabilitiesService : ICapabilitiesServices
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        public CapabilitiesService(MessagingService messagingService)
        {
            _messagingService = messagingService;
            _encodeMessageService = new EncodeMessageService();
        }

        public string send(CapabilitiesParameters capabilitiesParameters)
        {
            var encodedMessages = new List<string> {Encode(capabilitiesParameters).Content};
            var messagingParameters = new MessagingParameters
            {
                ApplicationMessageId = capabilitiesParameters.ApplicationMessageId,
                TeamsetContextId = capabilitiesParameters.TeamsetContextId,
                OnboardingResponse = capabilitiesParameters.OnboardingResponse, EncodedMessages = encodedMessages
            };

            return _messagingService.send(messagingParameters);
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

            var capabilitySpecification = new CapabilitySpecification
            {
                AppCertificationId = capabilitiesParameters.ApplicationId,
                AppCertificationVersionId = capabilitiesParameters.CertificationVersionId,
                EnablePushNotifications = capabilitiesParameters.EnablePushNotifications
            };

            foreach (var capabilityParameter in capabilitiesParameters.CapabilityParameters)
            {
                var capability = new CapabilitySpecification.Types.Capability
                {
                    TechnicalMessageType = capabilityParameter.TechnicalMessageType
                };
                capability.Direction = capability.Direction;
                capabilitySpecification.Capabilities.Add(capability);
            }

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = CapabilitySpecification.Descriptor.FullName,
                Value = capabilitySpecification.ToByteString()
            };

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = _encodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}