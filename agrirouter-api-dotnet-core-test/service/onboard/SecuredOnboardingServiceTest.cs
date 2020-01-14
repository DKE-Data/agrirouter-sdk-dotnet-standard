using System.Net.Http;
using com.dke.data.agrirouter.api.definitions;
using com.dke.data.agrirouter.api.enums;
using com.dke.data.agrirouter.api.exception;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.api.test.helper;
using com.dke.data.agrirouter.impl.service.common;
using com.dke.data.agrirouter.impl.service.onboard;
using Xunit;

namespace com.dke.data.agrirouter.api.test.service.onboard
{
    public class SecuredOnboardingServiceTest : AbstractSecuredIntegrationTest
    {
        private static readonly UtcDataService UtcDataService = new UtcDataService();
        private static readonly SignatureService SignatureService = new SignatureService();
        private static readonly HttpClient HttpClient = HttpClientFactory.HttpClient();

        [Fact(Skip = "Will not run successfully without changing the registration code.")]
        public void GivenValidRequestTokenWhenOnboardingThenThereShouldBeAValidResponse()
        {
            var onboardingService =
                new SecuredOnboardingService(Environment, UtcDataService, SignatureService, HttpClient);

            var parameters = new OnboardingParameters
            {
                Uuid = GetType().FullName,
                ApplicationId = ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.P12,
                GatewayId = "3",
                RegistrationCode = "47a726bf20",
                CertificationVersionId = CertificationVersionId
            };


            var onboardingResponse = onboardingService.Onboard(parameters, PrivateKey);

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
            var onboardingService =
                new SecuredOnboardingService(Environment, UtcDataService, SignatureService, HttpClient);

            var parameters = new OnboardingParameters
            {
                Uuid = GetType().FullName,
                ApplicationId = ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.P12,
                GatewayId = "3",
                RegistrationCode = "XXXXXXXX",
                CertificationVersionId = CertificationVersionId
            };


            Assert.Throws<OnboardingException>(() => onboardingService.Onboard(parameters, PrivateKey));
        }
    }
}