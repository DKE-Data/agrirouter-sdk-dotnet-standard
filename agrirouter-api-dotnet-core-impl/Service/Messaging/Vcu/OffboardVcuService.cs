using System;
using System.Collections.Generic;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Service.Messaging.vcu;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Cloud.Registration;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Request;
using Agrirouter.Request.Payload.Endpoint;
using Google.Protobuf;

namespace Agrirouter.Impl.Service.Messaging.Vcu
{
    /// <summary>
    /// Service to onboard VCUs.
    /// </summary>
    public class OffboardVcuService : IOffboardVcuService
    {
        private readonly MessagingService _messagingService;
        private readonly EncodeMessageService _encodeMessageService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        /// <param name="encodeMessageService">-</param>
        public OffboardVcuService(MessagingService messagingService, EncodeMessageService encodeMessageService)
        {
            _messagingService = messagingService;
            _encodeMessageService = encodeMessageService;
        }

        /// <summary>
        /// Please see <seealso cref="IMessagingService{T}.Send"/> for documentation.
        /// </summary>
        /// <param name="onboardVcuParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(OffboardVcuParameters offboardVcuParameters)
        {
            var encodedMessages = new List<string> {Encode(offboardVcuParameters).Content};
            var messagingParameters = offboardVcuParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        /// <summary>
        /// Please see <seealso cref="IEncodeMessageService{T}.Encode"/> for documentation.
        /// </summary>
        /// <param name="onboardVcuParameters"></param>
        /// <returns>-</returns>
        public EncodedMessage Encode(OffboardVcuParameters offboardVcuParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = offboardVcuParameters.ApplicationMessageId,
                TeamSetContextId = offboardVcuParameters.TeamsetContextId ?? "",
                TechnicalMessageType = TechnicalMessageTypes.DkeCloudOffboardEndpoints,
                Mode = RequestEnvelope.Types.Mode.Direct
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = OffboardingRequest.Descriptor.FullName
            };

            var offboardingRequest = new OffboardingRequest();
            foreach (var endpoint in offboardVcuParameters.Endpoints)
            {
                offboardingRequest.Endpoints.Add(endpoint);
            }

            messagePayloadParameters.Value = offboardingRequest.ToByteString();

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = _encodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}