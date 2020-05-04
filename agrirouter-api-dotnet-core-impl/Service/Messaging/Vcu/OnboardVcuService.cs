using System;
using System.Collections.Generic;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Messaging.Vcu;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Cloud.Registration;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Request;
using Google.Protobuf;

namespace Agrirouter.Impl.Service.Messaging.Vcu
{
    /// <summary>
    /// Service to onboard VCUs.
    /// </summary>
    public class OnboardVcuService : IOnboardVcuService
    {
        private readonly HttpMessagingService _httpMessagingService;
        private readonly EncodeMessageService _encodeMessageService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpMessagingService">-</param>
        /// <param name="encodeMessageService">-</param>
        public OnboardVcuService(HttpMessagingService httpMessagingService, EncodeMessageService encodeMessageService)
        {
            _httpMessagingService = httpMessagingService;
            _encodeMessageService = encodeMessageService;
        }

        /// <summary>
        /// Please see <seealso cref="IMessagingService{T}.Send"/> for documentation.
        /// </summary>
        /// <param name="onboardVcuParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(OnboardVcuParameters onboardVcuParameters)
        {
            var encodedMessages = new List<string> {Encode(onboardVcuParameters).Content};
            var messagingParameters = onboardVcuParameters.BuildMessagingParameter(encodedMessages);
            return _httpMessagingService.Send(messagingParameters);
        }

        /// <summary>
        /// Please see <seealso cref="IEncodeMessageService{T}.Encode"/> for documentation.
        /// </summary>
        /// <param name="onboardVcuParameters"></param>
        /// <returns>-</returns>
        public EncodedMessage Encode(OnboardVcuParameters onboardVcuParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = onboardVcuParameters.ApplicationMessageId,
                TeamSetContextId = onboardVcuParameters.TeamsetContextId ?? "",
                TechnicalMessageType = TechnicalMessageTypes.DkeCloudOnboardEndpoints,
                Mode = RequestEnvelope.Types.Mode.Direct
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = OnboardingRequest.Descriptor.FullName
            };

            var onboardingRequest = new OnboardingRequest();
            foreach (var onboardingRequestEntry in onboardVcuParameters.OnboardingRequests)
            {
                onboardingRequest.OnboardingRequests.Add(onboardingRequestEntry);
            }

            messagePayloadParameters.Value = onboardingRequest.ToByteString();

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = EncodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}