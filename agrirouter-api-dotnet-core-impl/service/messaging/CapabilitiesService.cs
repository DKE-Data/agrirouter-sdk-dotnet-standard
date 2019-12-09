using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Agrirouter.Request;
using Agrirouter.Request.Payload.Endpoint;
using com.dke.data.agrirouter.api.definitions;
using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.exception;
using com.dke.data.agrirouter.api.service;
using com.dke.data.agrirouter.api.service.parameters;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Serilog;

namespace com.dke.data.agrirouter.impl.service.messaging
{
    public class CapabilitiesService : MessagingService, ICapabilitiesServices
    {
        public string send(CapabilitiesParameters capabilitiesParameters)
        {
            List<string> encodedMessages = new List<string>();
            EncodedMessage(capabilitiesParameters);
            var messageParameters = new MessagingParameters
            {
                OnboardingResponse = capabilitiesParameters.OnboardingResponse, EncodedMessages = encodedMessages
            };

            return send(messageParameters);
        }

        public EncodedMessage EncodedMessage(CapabilitiesParameters capabilitiesParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = capabilitiesParameters.ApplicationMessageId,
                TeamSetContextId = capabilitiesParameters.TeamsetContextId ?? "",
                ApplicationMessageSeqNo = capabilitiesParameters.ApplicationMessageSeqNo,
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
                var capability = new CapabilitySpecification.Types.Capability();
                capability.TechnicalMessageType = capabilityParameter.TechnicalMessageType
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
                Id = Guid.NewGuid().ToString(), Content = Encode(messageHeaderParameters, messagePayloadParameters)
            };
        }

        public string Encode(MessageHeaderParameters messageHeaderParameters,
            MessagePayloadParameters messagePayloadParameters)
        {
            Log.Debug("Start encoding of the message.");

            if (null == messageHeaderParameters || null == messagePayloadParameters)
            {
                throw new MissingParameterException();
            }

            using var memoryStream = new MemoryStream();
            Header(messageHeaderParameters).WriteDelimitedTo(memoryStream);
            PayloadWrapper(messagePayloadParameters).WriteDelimitedTo(memoryStream);
            var encodedMessage = Convert.ToBase64String(memoryStream.GetBuffer());

            Log.Debug("Finished encoding of the message.");
            return encodedMessage;
        }

        private RequestEnvelope Header(MessageHeaderParameters messageHeaderParameters)
        {
            Log.Debug("Begin creating the header of the message.");
            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            var timestamp = new Timestamp {Seconds = (long) timeSpan.TotalSeconds, Nanos = 1000000};

            var requestEnvelope = new RequestEnvelope
            {
                ApplicationMessageId = messageHeaderParameters.ApplicationMessageId,
                ApplicationMessageSeqNo = messageHeaderParameters.ApplicationMessageSeqNo,
                TechnicalMessageType = messageHeaderParameters.TechnicalMessageType,
                Mode = messageHeaderParameters.Mode,
                Timestamp = timestamp
            };

            if (!string.IsNullOrEmpty(messageHeaderParameters.TeamSetContextId))
            {
                requestEnvelope.TeamSetContextId = messageHeaderParameters.TeamSetContextId;
            }

            if (messageHeaderParameters.Recipients != null)
            {
                foreach (var recipient in messageHeaderParameters.Recipients)
                {
                    requestEnvelope.Recipients.Add(recipient);
                }
            }

            if (messageHeaderParameters.ChunkInfo != null)
            {
                requestEnvelope.ChunkInfo = messageHeaderParameters.ChunkInfo;
            }

            Log.Debug("Finished creating the header of the message.");

            return requestEnvelope;
        }

        private RequestPayloadWrapper PayloadWrapper(MessagePayloadParameters messagePayloadParameters)
        {
            Log.Debug("Begin creating the payload of the message.");
            var any = new Any();
            any.TypeUrl = messagePayloadParameters.TypeUrl;
            any.Value = messagePayloadParameters.Value;

            var requestPayloadWrapper = new RequestPayloadWrapper();
            requestPayloadWrapper.Details = any;

            Log.Debug("Finished creating the payload of the message.");
            return requestPayloadWrapper;
        }
    }
}