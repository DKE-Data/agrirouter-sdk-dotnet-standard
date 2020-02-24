using System;
using System.Net.Http;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.test.helper;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.onboard;
using Xunit;

namespace Agrirouter.Api.Test.Service.Onboard
{
    /// <summary>
    /// Functional tests.
    /// </summary>
    public class OnboardServiceTest : AbstractIntegrationTest
    {
        private static readonly UtcDataService UtcDataService = new UtcDataService();
        private static readonly HttpClient HttpClient = HttpClientFactory.HttpClient();

        [Fact(Skip = "Will not run successfully without changing the registration code.")]
        public void GivenValidRequestTokenWhenOnboardingForP12ThenThereShouldBeAValidResponse()
        {
            var onboardingService = new OnboardService(Environment, UtcDataService, HttpClient);

            var parameters = new OnboardParameters
            {
                Uuid = Guid.NewGuid().ToString(),
                ApplicationId = ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.Pem,
                GatewayId = "3",
                RegistrationCode = "414fa598a3",
                CertificationVersionId = CertificationVersionId
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
                ApplicationId = ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.Pem,
                GatewayId = "3",
                RegistrationCode = "f70470a755",
                CertificationVersionId = CertificationVersionId
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

        [Fact]
        public void GivenInvalidRequestTokenWhenOnboardingThenThereShouldBeAValidResponse()
        {
            var onboardingService = new OnboardService(Environment, UtcDataService, HttpClient);

            var parameters = new OnboardParameters
            {
                Uuid = Guid.NewGuid().ToString(),
                ApplicationId = ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.P12,
                GatewayId = "3",
                RegistrationCode = "XXXXXXXX",
                CertificationVersionId = CertificationVersionId
            };


            Assert.Throws<OnboardException>(() => onboardingService.Onboard(parameters));
        }
    }
}