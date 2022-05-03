using System;
using System.Net.Http;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Onboard;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Xunit;

namespace Agrirouter.Test.Service.Onboard
{
    /// <summary>
    ///     Functional tests.
    /// </summary>
    [Collection("Integrationtest")]
    public class SecuredOnboardingServiceTestForFarmingSoftware : AbstractSecuredIntegrationTestForFarmingSoftware
    {
        private static readonly HttpClient HttpClient = HttpClientFactory.HttpClient();

        [Fact]
        public void GivenInvalidRequestTokenWhenOnboardingThenThereShouldBeAnException()
        {
            var onboardingService =
                new SecuredOnboardingService(Environment, HttpClient);

            var parameters = new OnboardParameters
            {
                Uuid = GetType().FullName,
                ApplicationId = Applications.FarmingSoftware.ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.P12,
                GatewayId = GatewayTypeDefinition.Http,
                RegistrationCode = "XXXXXXXX",
                CertificationVersionId = Applications.FarmingSoftware.CertificationVersionId
            };


            Assert.Throws<OnboardException>(() =>
                onboardingService.Onboard(parameters, Applications.FarmingSoftware.PrivateKey));
        }

        [Fact]
        public void GivenValidApplicationIdWhenCreatingAuthorizationUrlThenTheUrlShouldBeFineDuringManualTesting()
        {
            var authorizationService = new AuthorizationService(Environment);
            var authorizationUrlResult =
                authorizationService.AuthorizationUrl(Applications.FarmingSoftware.ApplicationId);
            Assert.NotEmpty(authorizationUrlResult.State);
            Assert.NotEmpty(authorizationUrlResult.AuthorizationUrl);
        }


        [Fact(Skip = "Will not work without changing the registration code.")]
        public void GivenValidRequestTokenWhenVerifyingTheAccountThenThereShouldBeAValidResponse()
        {
            var onboardingService =
                new SecuredOnboardingService(Environment, HttpClient);

            var parameters = new VerificationParameters()
            {
                ExternalId = Guid.NewGuid().ToString(),
                ApplicationId = Applications.FarmingSoftware.ApplicationId,
                CertificationType = CertificationTypeDefinition.P12,
                GatewayId = GatewayTypeDefinition.Http,
                RegistrationCode = "09e0e95c7b",
                CertificationVersionId = Applications.FarmingSoftware.CertificationVersionId
            };

            var verificationResponse = onboardingService.Verify(parameters, Applications.FarmingSoftware.PrivateKey);
            Assert.NotEmpty(verificationResponse.AccountId);
        }

        [Fact(Skip = "Will not work without changing the registration code.")]
        public void GivenValidRequestTokenWhenOnboardingThenThereShouldBeAValidResponse()
        {
            var onboardingService =
                new SecuredOnboardingService(Environment, HttpClient);

            var parameters = new OnboardParameters
            {
                Uuid = Guid.NewGuid().ToString(),
                ApplicationId = Applications.FarmingSoftware.ApplicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.P12,
                GatewayId = GatewayTypeDefinition.Http,
                RegistrationCode = "8e266f7b8c",
                CertificationVersionId = Applications.FarmingSoftware.CertificationVersionId
            };


            var onboardingResponse = onboardingService.Onboard(parameters, Applications.FarmingSoftware.PrivateKey);

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