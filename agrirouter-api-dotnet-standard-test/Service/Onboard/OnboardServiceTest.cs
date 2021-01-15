using System;
using System.Net.Http;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Test.Data;
using Agrirouter.Api.Test.Helper;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.onboard;
using Xunit;

namespace Agrirouter.Api.Test.Service.Onboard
{
    /// <summary>
    ///     Functional tests.
    /// </summary>
    [Collection("Integrationtest")]
    public class OnboardServiceTest : AbstractIntegrationTestForCommunicationUnits
    {
        private static readonly UtcDataService UtcDataService = new UtcDataService();
        private static readonly HttpClient HttpClient = HttpClientFactory.HttpClient();

        [Fact]
        public void GivenInvalidRequestTokenWhenOnboardingThenThereShouldBeAnException()
        {
            var onboardingService = new OnboardService(Environment, UtcDataService, HttpClient);

            var parameters = new OnboardParameters
            {
                Uuid = Guid.NewGuid().ToString(),
                ApplicationId = Applications.CommunicationUnit.ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.P12,
                GatewayId = "3",
                RegistrationCode = "XXXXXXXX",
                CertificationVersionId = Applications.CommunicationUnit.CertificationVersionId
            };


            Assert.Throws<OnboardException>(() => onboardingService.Onboard(parameters));
        }

        [Fact(Skip = "Will not run successfully without changing the registration code.")]
        public void GivenValidRequestTokenWhenOnboardingForP12ThenThereShouldBeAValidResponse()
        {
            var onboardingService = new OnboardService(Environment, UtcDataService, HttpClient);

            var parameters = new OnboardParameters
            {
                Uuid = Guid.NewGuid().ToString(),
                ApplicationId = Applications.CommunicationUnit.ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.P12,
                GatewayId = GatewayTypeDefinition.Mqtt,
                RegistrationCode = "7bf3aa13ee",
                CertificationVersionId = Applications.CommunicationUnit.CertificationVersionId
            };


            var onboardResponse = onboardingService.Onboard(parameters);

            Assert.NotEmpty(onboardResponse.DeviceAlternateId);
            Assert.NotEmpty(onboardResponse.SensorAlternateId);
            Assert.NotEmpty(onboardResponse.CapabilityAlternateId);

            Assert.NotEmpty(onboardResponse.Authentication.Certificate);
            Assert.NotEmpty(onboardResponse.Authentication.Secret);
            Assert.NotEmpty(onboardResponse.Authentication.Type);

            Assert.NotEmpty(onboardResponse.ConnectionCriteria.Commands);
            Assert.NotEmpty(onboardResponse.ConnectionCriteria.Measures);
        }

        [Fact(Skip = "Will not run successfully without changing the registration code.")]
        public void GivenValidRequestTokenWhenOnboardingForPEMThenThereShouldBeAValidResponse()
        {
            var onboardingService = new OnboardService(Environment, UtcDataService, HttpClient);

            var parameters = new OnboardParameters
            {
                Uuid = Guid.NewGuid().ToString(),
                ApplicationId = Applications.CommunicationUnit.ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.Pem,
                GatewayId = GatewayTypeDefinition.Mqtt,
                RegistrationCode = "f70470a755",
                CertificationVersionId = Applications.CommunicationUnit.CertificationVersionId
            };


            var onboardResponse = onboardingService.Onboard(parameters);

            Assert.NotEmpty(onboardResponse.DeviceAlternateId);
            Assert.NotEmpty(onboardResponse.SensorAlternateId);
            Assert.NotEmpty(onboardResponse.CapabilityAlternateId);

            Assert.NotEmpty(onboardResponse.Authentication.Certificate);
            Assert.NotEmpty(onboardResponse.Authentication.Secret);
            Assert.NotEmpty(onboardResponse.Authentication.Type);

            Assert.NotEmpty(onboardResponse.ConnectionCriteria.Commands);
            Assert.NotEmpty(onboardResponse.ConnectionCriteria.Measures);
        }
    }
}