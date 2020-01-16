using System;
using System.Net.Http;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.test.helper;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.onboard;
using Xunit;

namespace Agrirouter.Api.test.service.onboard
{
    /// <summary>
    /// Functional tests.
    /// </summary>
    public class OnboardingServiceTest : AbstractIntegrationTest
    {
        private static readonly UtcDataService UtcDataService = new UtcDataService();
        private static readonly HttpClient HttpClient = HttpClientFactory.HttpClient();

        //[Fact(Skip = "Will not run successfully without changing the registration code.")]
        [Fact]
        public void GivenValidRequestTokenWhenOnboardingForP12ThenThereShouldBeAValidResponse()
        {
            var onboardingService = new OnboardingService(Environment, UtcDataService, HttpClient);

            OnboardingParameters parameters = new OnboardingParameters
            {
                Uuid = Guid.NewGuid().ToString(),
                ApplicationId = ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.PEM,
                GatewayId = "3",
                RegistrationCode = "414fa598a3",
                CertificationVersionId = CertificationVersionId
            };


            OnboardingResponse onboardingResponse = onboardingService.Onboard(parameters);

            Assert.NotEmpty(onboardingResponse.DeviceAlternateId);
            Assert.NotEmpty(onboardingResponse.SensorAlternateId);
            Assert.NotEmpty(onboardingResponse.CapabilityAlternateId);

            Assert.NotEmpty(onboardingResponse.Authentication.Certificate);
            Assert.NotEmpty(onboardingResponse.Authentication.Secret);
            Assert.NotEmpty(onboardingResponse.Authentication.Type);

            Assert.NotEmpty(onboardingResponse.ConnectionCriteria.Commands);
            Assert.NotEmpty(onboardingResponse.ConnectionCriteria.Measures);
        }

        [Fact(Skip = "Will not run successfully without changing the registration code.")]
        public void GivenValidRequestTokenWhenOnboardingForPEMThenThereShouldBeAValidResponse()
        {
            var onboardingService = new OnboardingService(Environment, UtcDataService, HttpClient);

            OnboardingParameters parameters = new OnboardingParameters
            {
                Uuid = Guid.NewGuid().ToString(),
                ApplicationId = ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.PEM,
                GatewayId = "3",
                RegistrationCode = "f70470a755",
                CertificationVersionId = CertificationVersionId
            };


            OnboardingResponse onboardingResponse = onboardingService.Onboard(parameters);

            Assert.NotEmpty(onboardingResponse.DeviceAlternateId);
            Assert.NotEmpty(onboardingResponse.SensorAlternateId);
            Assert.NotEmpty(onboardingResponse.CapabilityAlternateId);

            Assert.NotEmpty(onboardingResponse.Authentication.Certificate);
            Assert.NotEmpty(onboardingResponse.Authentication.Secret);
            Assert.NotEmpty(onboardingResponse.Authentication.Type);

            Assert.NotEmpty(onboardingResponse.ConnectionCriteria.Commands);
            Assert.NotEmpty(onboardingResponse.ConnectionCriteria.Measures);
        }

        [Fact]
        public void GivenInvalidRequestTokenWhenOnboardingThenThereShouldBeAValidResponse()
        {
            var onboardingService = new OnboardingService(Environment, UtcDataService, HttpClient);

            OnboardingParameters parameters = new OnboardingParameters
            {
                Uuid = Guid.NewGuid().ToString(),
                ApplicationId = ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.P12,
                GatewayId = "3",
                RegistrationCode = "XXXXXXXX",
                CertificationVersionId = CertificationVersionId
            };


            Assert.Throws<OnboardingException>(() => onboardingService.Onboard(parameters));
        }
    }
}