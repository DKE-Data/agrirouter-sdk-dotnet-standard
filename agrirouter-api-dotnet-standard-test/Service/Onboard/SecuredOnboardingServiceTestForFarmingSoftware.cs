using System;
using System.Net.Http;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Test.Helper;
using Agrirouter.Impl.Service.onboard;
using Xunit;

namespace Agrirouter.Api.Test.Service.Onboard
{
    /// <summary>
    ///     Functional tests.
    /// </summary>
    [Collection("Integrationtest")]
    public class SecuredOnboardingServiceTestForFarmingSoftware : AbstractSecuredIntegrationTestForFarmingSoftware
    {
        private static readonly HttpClient HttpClient = HttpClientFactory.HttpClient();

        [Fact]
        public void GivenInvalidRequestTokenWhenOnboardingThenThereShouldBeAValidResponse()
        {
            var onboardingService =
                new SecuredOnboardingService(Environment, HttpClient);

            var parameters = new OnboardParameters
            {
                Uuid = GetType().FullName,
                ApplicationId = ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.P12,
                GatewayId = "3",
                RegistrationCode = "XXXXXXXX",
                CertificationVersionId = CertificationVersionId
            };


            Assert.Throws<OnboardException>(() => onboardingService.Onboard(parameters, PrivateKey));
        }

        [Fact(Skip = "Can be run to generate the authorization URL.")]
        public void GivenValidApplicationIdWhenCreatingAuthorizationUrlThenTheUrlShouldBeFineDuringManualTesting()
        {
            var authorizationService = new AuthorizationService(Environment);
            var authorizationUrlResult = authorizationService.AuthorizationUrl(ApplicationId);
            Assert.NotEmpty(authorizationUrlResult.State);
            Assert.NotEmpty(authorizationUrlResult.AuthorizationUrl);
        }

        [Fact(Skip = "Will not run successfully without changing the registration code.")]
        public void GivenValidRequestTokenWhenOnboardingThenThereShouldBeAValidResponse()
        {
            var onboardingService =
                new SecuredOnboardingService(Environment, HttpClient);

            var parameters = new OnboardParameters
            {
                Uuid = Guid.NewGuid().ToString(),
                ApplicationId = ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.P12,
                GatewayId = "3",
                RegistrationCode = "65220a7655",
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
    }
}