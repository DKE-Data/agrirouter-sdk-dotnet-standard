using System.Collections.Generic;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Xunit;

namespace Agrirouter.Test.Service.Messaging.Http
{
    /// <summary>
    /// Functional tests.
    /// </summary>
    [Collection("Integrationtest")]
    public class PemAndP12MessagingTest : AbstractIntegrationTestForCommunicationUnits
    {
        [Fact]
        public void
            GivenValidCapabilitiesWhenSendingCapabilitiesMessageWithP12CertificateThenTheAgrirouterShouldSetTheCapabilities()
        {
            RunWith(OnboardResponseWithP12Certificate);
        }

        [Fact]
        public void
            GivenValidCapabilitiesWhenSendingCapabilitiesMessageWithPemCertificateThenTheAgrirouterShouldSetTheCapabilities()
        {
            RunWith(OnboardResponseWithPemCertificate);
        }

        private static void RunWith(OnboardResponse onboardResponse)
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

            var capabilitiesParameter = new CapabilityParameter
            {
                Direction = CapabilitySpecification.Types.Direction.SendReceive,
                TechnicalMessageType = TechnicalMessageTypes.Iso11783TaskdataZip
            };

            capabilitiesParameters.CapabilityParameters.Add(capabilitiesParameter);
            capabilitiesServices.Send(capabilitiesParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(httpClient);
            var fetch = fetchMessageService.Fetch(onboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }

        private static OnboardResponse OnboardResponseWithP12Certificate =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit
                .SingleEndpointWithP12Certificate);

        private static OnboardResponse OnboardResponseWithPemCertificate =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit
                .SingleEndpointWithPemCertificate);
    }
}