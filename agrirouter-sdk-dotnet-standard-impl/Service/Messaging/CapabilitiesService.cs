using System;
using System.Collections.Generic;
using Agrirouter.Request;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Google.Protobuf;

namespace Agrirouter.Impl.Service.Messaging
{
    /// <summary>
    ///     Please see <seealso cref="ICapabilitiesServices" /> for documentation.
    /// </summary>
    public class CapabilitiesService : ICapabilitiesServices
    {
        private readonly IMessagingService<MessagingParameters> _messagingService;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        public CapabilitiesService(IMessagingService<MessagingParameters> messagingService)
        {
            _messagingService = messagingService;
        }

        /// <summary>
        ///     Please see <seealso cref="IMessagingService{T}.Send" /> for documentation.
        /// </summary>
        /// <param name="capabilitiesParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(CapabilitiesParameters capabilitiesParameters)
        {
            var encodedMessages = new List<string> {Encode(capabilitiesParameters).Content};
            var messagingParameters = capabilitiesParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        /// <summary>
        ///     Please see <seealso cref="IEncodeMessageService{T}.Encode" /> for documentation.
        /// </summary>
        /// <param name="capabilitiesParameters"></param>
        /// <returns>-</returns>
        public EncodedMessage Encode(CapabilitiesParameters capabilitiesParameters)
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
                AppCertificationVersionId = capabilitiesParameters.CertificationVersionId,
                EnablePushNotifications = capabilitiesParameters.EnablePushNotifications,
            };
            capabilitiesParameters.CapabilityParameters.ForEach(capabilityParameter =>
            {
                var capability = new CapabilitySpecification.Types.Capability
                {
                    TechnicalMessageType = capabilityParameter.TechnicalMessageType,
                    Direction = capabilityParameter.Direction
                };
                capabilitySpecification.Capabilities.Add(capability);
            });
            messagePayloadParameters.Value = capabilitySpecification.ToByteString();

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = EncodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}