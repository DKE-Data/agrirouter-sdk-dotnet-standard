using System;
using System.Collections.Generic;
using System.Net.Http;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Cloud.Registration;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Impl.Service.Messaging.Vcu;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Agrirouter.Test.Service;
using Xunit;

namespace Agrirouter.Test.Integration
{
    public class
        SendCapabilitiesMessageForVirtualCommunicationUnitTest : AbstractIntegrationTestForCommunicationUnits
    {
        private static readonly HttpClient HttpClient = HttpClientFactory.AuthenticatedHttpClient(OnboardResponse);

        private static OnboardResponse OnboardResponse =>
            OnboardResponseIntegrationService.Read(Identifier.Http.TelemtryPlatform.Sender);

        /// <summary>
        /// Onboard process for a virtual communication unit and sending the capabilities message.
        /// </summary>
        [Fact]
        public void
            GivenExistingEndpointWhenVirtualCommunicationUnitIsOnboardedThenTheEndpointShouldBeAbleToSendCapabilities()
        {
            var onboardVcuService = new OnboardVcuService(new HttpMessagingService(HttpClient));
            var onboardVcuParameters = new OnboardVcuParameters
            {
                OnboardResponse = OnboardResponse,
                ApplicationMessageId = MessageIdService.ApplicationMessageId(),
                OnboardingRequests = new List<OnboardingRequest.Types.EndpointRegistrationDetails>
                {
                    new OnboardingRequest.Types.EndpointRegistrationDetails
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Integration Test | Sending Capabilities For Virtual Communication Unit",
                    }
                }
            };
            onboardVcuService.Send(onboardVcuParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);

            var onboardingResponse = onboardVcuService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(onboardingResponse);
            Assert.Single(onboardingResponse.OnboardedEndpoints);
            var onboardResponseForVirtualCommunicationUnit = onboardingResponse.OnboardedEndpoints[0];

            var fullyUsableOnboardResponseForVirtualCommunicationUnit =
                onboardVcuService.EnhanceVirtualCommunicationToFullyUsableOnboardResponse(OnboardResponse,
                    onboardResponseForVirtualCommunicationUnit);

            var httpClient =
                HttpClientFactory.AuthenticatedHttpClient(fullyUsableOnboardResponseForVirtualCommunicationUnit);
            var capabilitiesServices =
                new CapabilitiesService(new HttpMessagingService(httpClient));
            var capabilitiesParameters = new CapabilitiesParameters
            {
                OnboardResponse = fullyUsableOnboardResponseForVirtualCommunicationUnit,
                ApplicationId = Applications.TelemetryPlatform.ApplicationId,
                CertificationVersionId = Applications.TelemetryPlatform.CertificationVersionId,
                EnablePushNotifications = CapabilitySpecification.Types.PushNotification.Disabled,
                CapabilityParameters = new List<CapabilityParameter>()
            };

            capabilitiesParameters.CapabilityParameters.AddRange(CapabilitiesHelper.AllCapabilities);
            capabilitiesServices.Send(capabilitiesParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageServiceForVirtualCommunicationUnit = new FetchMessageService(httpClient);
            fetch = fetchMessageServiceForVirtualCommunicationUnit.Fetch(
                fullyUsableOnboardResponseForVirtualCommunicationUnit);
            Assert.Single(fetch);

            decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }
    }
}