using System;
using System.Collections.Generic;
using System.Threading;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Test.Helper;
using Xunit;
using Timer = Agrirouter.Test.Helper.Timer;

namespace Agrirouter.Test.Data.OnboardingResponses.Http
{
    public class EnableAllCapabilitiesForOnboardingResponsesTest
    {
        [Fact]
        public void
            GivenValidCapabilitiesWhenSendingTheCapabilitiesThenTheAgrirouterShouldSetTheCapabilitiesForAllCommunicationUnits()
        {
            OnboardResponseIntegrationService.AllCommunicationUnits().ForEach(onboardResponse =>
            {
                var httpClient = HttpClientFactory.AuthenticatedHttpClient(onboardResponse);
                var capabilitiesServices =
                    new CapabilitiesService(new HttpMessagingService(httpClient));
                var capabilitiesParameters = new CapabilitiesParameters
                {
                    OnboardResponse = onboardResponse,
                    ApplicationId = Applications.CommunicationUnit.ApplicationId,
                    CertificationVersionId = Applications.CommunicationUnit.CertificationVersionId,
                    EnablePushNotifications = CapabilitySpecification.Types.PushNotification.Disabled,
                    CapabilityParameters = new List<CapabilityParameter>()
                };

                capabilitiesParameters.CapabilityParameters.AddRange(CapabilitiesHelper.AllCapabilities);
                capabilitiesServices.Send(capabilitiesParameters);

                Timer.WaitForTheAgrirouterToProcessTheMessage();

                var fetchMessageService = new FetchMessageService(httpClient);
                var fetch = fetchMessageService.Fetch(onboardResponse);
                Assert.Single(fetch);

                var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
                Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
            });
        }
    }
}