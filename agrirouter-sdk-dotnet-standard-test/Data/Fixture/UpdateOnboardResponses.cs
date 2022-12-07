using System.Collections.Generic;
using System.Net.Http;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Env;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Impl.Service.Onboard;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Test.Helper;
using Xunit;

namespace Agrirouter.Test.Data.Fixture
{
    /// <summary>
    /// Fixture creator to update the onboard responses.
    /// </summary>
    [Collection("fixture")]
    public class UpdateOnboardResponses
    {
        private static readonly UtcDataService UtcDataService = new UtcDataService();
        private static readonly HttpClient HttpClient = HttpClientFactory.HttpClient();
        private static readonly Environment Environment = new QualityAssuranceEnvironment();

        [Fact]
        public void Recipient()
        {
            var onboardResponse = Onboard("97cce58c-7eb0-4284-993b-f0069fdee961", "a1e3a22433");
            ValidateConnection(onboardResponse);
            EnableAllCapabilitiesViaHttp(onboardResponse);
            OnboardResponseIntegrationService.Save(Identifier.Http.CommunicationUnit.Recipient, onboardResponse);
        }

        [Fact(Skip = "Will fail unless the token is changed.")]
        public void RecipientWithEnabledPushMessages()
        {
            var onboardResponse = Onboard("97cce58c-7eb0-4284-993b-f0069fdee962", "CHANGE_ME");
            ValidateConnection(onboardResponse);
            EnableAllCapabilitiesViaHttp(onboardResponse);
            OnboardResponseIntegrationService.Save(Identifier.Http.CommunicationUnit.RecipientWithEnabledPushMessages,
                onboardResponse);
        }

        [Fact(Skip = "Will fail unless the token is changed.")]
        public void Sender()
        {
            var onboardResponse = Onboard("97cce58c-7eb0-4284-993b-f0069fdee963", "CHANGE_ME");
            ValidateConnection(onboardResponse);
            EnableAllCapabilitiesViaHttp(onboardResponse);
            OnboardResponseIntegrationService.Save(Identifier.Http.CommunicationUnit.Sender, onboardResponse);
        }

        [Fact(Skip = "Will fail unless the token is changed.")]
        public void SenderWithMultipleRecipients()
        {
            var onboardResponse = Onboard("97cce58c-7eb0-4284-993b-f0069fdee964", "CHANGE_ME");
            ValidateConnection(onboardResponse);
            EnableAllCapabilitiesViaHttp(onboardResponse);
            OnboardResponseIntegrationService.Save(Identifier.Http.CommunicationUnit.SenderWithMultipleRecipients,
                onboardResponse);
        }

        [Fact(Skip = "Will fail unless the token is changed.")]
        public void SingleEndpointWithoutRoute()
        {
            var onboardResponse = Onboard("97cce58c-7eb0-4284-993b-f0069fdee965", "CHANGE_ME");
            ValidateConnection(onboardResponse);
            EnableAllCapabilitiesViaHttp(onboardResponse);
            OnboardResponseIntegrationService.Save(Identifier.Http.CommunicationUnit.SingleEndpointWithoutRoute,
                onboardResponse);
        }

        [Fact(Skip = "Will fail unless the token is changed.")]
        public void SingleEndpointWithP12Certificate()
        {
            var onboardResponse = Onboard("97cce58c-7eb0-4284-993b-f0069fdee966", "CHANGE_ME");
            ValidateConnection(onboardResponse);
            EnableAllCapabilitiesViaHttp(onboardResponse);
            OnboardResponseIntegrationService.Save(Identifier.Http.CommunicationUnit.SingleEndpointWithP12Certificate,
                onboardResponse);
        }

        [Fact(Skip = "Will fail unless the token is changed.")]
        public void SingleEndpointWithPemCertificate()
        {
            var onboardResponse = Onboard("97cce58c-7eb0-4284-993b-f0069fdee967", "CHANGE_ME",
                CertificationTypeDefinition.Pem);
            ValidateConnection(onboardResponse);
            EnableAllCapabilitiesViaHttp(onboardResponse);
            OnboardResponseIntegrationService.Save(Identifier.Http.CommunicationUnit.SingleEndpointWithPemCertificate,
                onboardResponse);
        }

        [Fact(Skip = "Will fail unless the token is changed.")]
        public void SingleMqttEndpointWithoutRoute()
        {
            var onboardResponse = Onboard("97cce58c-7eb0-4284-993b-f0069fdee968", "CHANGE_ME",
                gatewayId: GatewayTypeDefinition.Mqtt);
            OnboardResponseIntegrationService.Save(Identifier.Mqtt.CommunicationUnit.SingleEndpointWithoutRoute,
                onboardResponse);
        }

        [Fact(Skip = "Will fail unless the token is changed.")]
        public void SingleMqttEndpointWithP12Certificate()
        {
            var onboardResponse = Onboard("97cce58c-7eb0-4284-993b-f0069fdee969", "CHANGE_ME",
                gatewayId: GatewayTypeDefinition.Mqtt);
            OnboardResponseIntegrationService.Save(Identifier.Mqtt.CommunicationUnit.SingleEndpointWithP12Certificate,
                onboardResponse);
        }

        [Fact(Skip = "Will fail unless the token is changed.")]
        public void SingleMqttEndpointWithPemCertificate()
        {
            var onboardResponse = Onboard("97cce58c-7eb0-4284-993b-f0069fdee970", "CHANGE_ME",
                gatewayId: GatewayTypeDefinition.Mqtt);
            OnboardResponseIntegrationService.Save(Identifier.Mqtt.CommunicationUnit.SingleEndpointWithPemCertificate,
                onboardResponse);
        }

        private OnboardResponse Onboard(string uuid, string registrationCode,
            string certificationTypeDefinition = "P12", string gatewayId = "3")
        {
            var onboardService = new OnboardService(Environment, UtcDataService, HttpClient);

            var parameters = new OnboardParameters
            {
                Uuid = uuid,
                ApplicationId = Applications.CommunicationUnit.ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = certificationTypeDefinition,
                GatewayId = gatewayId,
                RegistrationCode = registrationCode,
                CertificationVersionId = Applications.CommunicationUnit.CertificationVersionId
            };

            var onboardResponse = onboardService.Onboard(parameters);

            Assert.NotEmpty(onboardResponse.DeviceAlternateId);
            Assert.NotEmpty(onboardResponse.SensorAlternateId);
            Assert.NotEmpty(onboardResponse.CapabilityAlternateId);

            Assert.NotEmpty(onboardResponse.Authentication.Certificate);
            Assert.NotEmpty(onboardResponse.Authentication.Secret);
            Assert.NotEmpty(onboardResponse.Authentication.Type);

            Assert.NotEmpty(onboardResponse.ConnectionCriteria.Commands);
            Assert.NotEmpty(onboardResponse.ConnectionCriteria.Measures);
            return onboardResponse;
        }

        private void ValidateConnection(OnboardResponse onboardResponse)
        {
            var authenticatedHttpClient = HttpClientFactory.AuthenticatedHttpClient(onboardResponse);
            var fetchMessageService = new FetchMessageService(authenticatedHttpClient);
            var fetch = fetchMessageService.Fetch(onboardResponse);
            Assert.Empty(fetch);
        }

        private void EnableAllCapabilitiesViaHttp(OnboardResponse onboardResponse)
        {
            var authenticatedHttpClient = HttpClientFactory.AuthenticatedHttpClient(onboardResponse);
            var capabilitiesServices =
                new CapabilitiesService(new HttpMessagingService(authenticatedHttpClient));
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

            var fetchMessageService = new FetchMessageService(authenticatedHttpClient);
            var fetch = fetchMessageService.Fetch(onboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }
    }
}