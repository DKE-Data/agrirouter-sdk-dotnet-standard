using com.dke.data.agrirouter.api.dto.onboard;
using com.dke.data.agrirouter.api.enums;
using com.dke.data.agrirouter.api.env;
using com.dke.data.agrirouter.api.exception;
using com.dke.data.agrirouter.api.service.onboard;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.impl.service.onboard;
using Xunit;

namespace com.dke.data.agrirouter.api.test.service.onboard
{
    public class OnboardingServiceTest : AbstractOnboardingTest
    {
        [Fact(Skip = "Will only be successful if there is a valid registration code.")]
        public void GivenValidRequestTokenWhenOnboardingThenThereShouldBeAValidResponse()
        {
            IOnboardingService onboardingService = new OnboardingService(Environment);

            OnboardingParameters parameters = new OnboardingParameters
            {
                Uuid = GetType().FullName,
                ApplicationId = ApplicationId,
                ApplicationType = ApplicationType.APPLICATION,
                CertificationType = CertificationType.P12,
                GatewayId = "3",
                RegistrationCode = "6dae10384d",
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
            IOnboardingService onboardingService = new OnboardingService(Environment);

            OnboardingParameters parameters = new OnboardingParameters
            {
                Uuid = GetType().FullName,
                ApplicationId = ApplicationId,
                ApplicationType = ApplicationType.APPLICATION,
                CertificationType = CertificationType.P12,
                GatewayId = "3",
                RegistrationCode = "XXXXXXXX",
                CertificationVersionId = CertificationVersionId
            };


            Assert.Throws<OnboardingException>(() => onboardingService.Onboard(parameters));
        }

        private Environment Environment => new QA();
    }
}