using com.dke.data.agrirouter.api.enums;
using com.dke.data.agrirouter.api.env;
using com.dke.data.agrirouter.api.service.onboard;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.impl.service.onboard;
using Xunit;

namespace com.dke.data.agrirouter.api.test.service.onboard
{
    public class OnboardingServiceTest : AbstractOnboardingTest
    {
        [Fact]
        public void GivenValidRequestTokenWhenOnboardingThenThereShouldBeAValidResponse()
        {
            IOnboardingService onboardingService = new OnboardingService(Environment);

            OnboardingParameters parameters = new OnboardingParameters();
            parameters.Uuid = GetType().FullName;
            parameters.ApplicationId = ApplicationId;
            parameters.ApplicationType = ApplicationType.APPLICATION;
            parameters.CertificationType = CertificationType.P12;
            parameters.GatewayId = "2";
            parameters.RegistrationCode = "1f11ce2dc9";
            parameters.CertificationVersionId = CertificationVersionId;
            

            onboardingService.Onboard(parameters);
        }

        private Environment Environment => new QA();
    }
}