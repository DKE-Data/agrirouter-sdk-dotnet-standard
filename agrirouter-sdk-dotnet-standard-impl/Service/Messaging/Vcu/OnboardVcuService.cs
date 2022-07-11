using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agrirouter.Cloud.Registration;
using Agrirouter.Request;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Messaging.Vcu;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace Agrirouter.Impl.Service.Messaging.Vcu
{
    /// <summary>
    ///     Service to onboard VCUs.
    /// </summary>
    public class OnboardVcuService : IOnboardVcuService, IDecodeMessageResponseService<OnboardingResponse>
    {
        private readonly IMessagingService<MessagingParameters> _messagingService;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        public OnboardVcuService(IMessagingService<MessagingParameters> messagingService)
        {
            _messagingService = messagingService;
        }

        /// <summary>
        ///     Please see <seealso cref="IMessagingService{T}.Send" /> for documentation.
        /// </summary>
        /// <param name="onboardVcuParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(OnboardVcuParameters onboardVcuParameters)
        {
            var encodedMessages = new List<string> { Encode(onboardVcuParameters).Content };
            var messagingParameters = onboardVcuParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        /// <summary>
        ///     Please see <seealso cref="IMessagingService{T}.SendAsync" /> for documentation.
        /// </summary>
        /// <param name="onboardVcuParameters">-</param>
        /// <returns>-</returns>
        public Task<MessagingResult> SendAsync(OnboardVcuParameters onboardVcuParameters)
        {
            var encodedMessages = new List<string> { Encode(onboardVcuParameters).Content };
            var messagingParameters = onboardVcuParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.SendAsync(messagingParameters);
        }

        /// <summary>
        ///     Please see <seealso cref="IEncodeMessageService{T}.Encode" /> for documentation.
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
                onboardingRequest.OnboardingRequests.Add(onboardingRequestEntry);

            messagePayloadParameters.Value = onboardingRequest.ToByteString();

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = EncodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }

        public OnboardingResponse Decode(Any messageResponse)
        {
            try
            {
                return OnboardingResponse.Parser.ParseFrom(messageResponse.Value);
            }
            catch (Exception e)
            {
                throw new CouldNotDecodeMessageException(
                    "Could not decode onboard response for virtual communication unit.", e);
            }
        }


        /// <summary>
        /// Please see <seealso cref="IOnboardVcuService.EnhanceVirtualCommunicationToFullyUsableOnboardResponse" /> for documentation.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="virtualCommunicationUnit"></param>
        /// <returns></returns>
        public OnboardResponse EnhanceVirtualCommunicationToFullyUsableOnboardResponse(OnboardResponse parent,
            OnboardingResponse.Types.EndpointRegistrationDetails virtualCommunicationUnit)
        {
            var capabilityAlternateId = virtualCommunicationUnit.CapabilityAlternateId;
            var deviceAlternateId = virtualCommunicationUnit.DeviceAlternateId;
            var sensorAlternateId = virtualCommunicationUnit.SensorAlternateId;

            parent.CapabilityAlternateId = capabilityAlternateId;
            parent.DeviceAlternateId = deviceAlternateId;
            parent.SensorAlternateId = sensorAlternateId;

            return parent;
        }
    }
}