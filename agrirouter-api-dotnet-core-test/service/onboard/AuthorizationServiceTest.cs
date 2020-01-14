using com.dke.data.agrirouter.api.env;
using com.dke.data.agrirouter.impl.service.onboard;
using Xunit;

namespace com.dke.data.agrirouter.api.test.service.onboard
{
    public class AuthorizationServiceTest
    {
        [Fact]
        public void GivenValidApplicationIdWhenCreatingAuthorizationUrlThenTheUrlShouldBeFineDuringManualTesting()
        {
            var authorizationService = new AuthorizationService(new QualityAssuranceEnvironment());
            var authorizationUrlResult = authorizationService.AuthorizationUrl(ApplicationId);
            Assert.Equal(
                $"https://agrirouter-qa.cfapps.eu10.hana.ondemand.com/application/16b1c3ab-55ef-412c-952b-f280424272e1/authorize?response_type=onboard&state={authorizationUrlResult.State}",
                authorizationUrlResult.AuthorizationUrl);
        }        
        
        [Fact]
        public void GivenValidApplicationIdAndRedirectUriWhenCreatingAuthorizationUrlThenTheUrlShouldBeFineDuringManualTesting()
        {
            var authorizationService = new AuthorizationService(new QualityAssuranceEnvironment());
            var authorizationUrlResult = authorizationService.AuthorizationUrl(ApplicationId, "https://www.saschadoemer.de");
            Assert.Equal(
                $"https://agrirouter-qa.cfapps.eu10.hana.ondemand.com/application/16b1c3ab-55ef-412c-952b-f280424272e1/authorize?response_type=onboard&state={authorizationUrlResult.State}&redirect_uri=https://www.saschadoemer.de",
                authorizationUrlResult.AuthorizationUrl);
        }

        public string ApplicationId => "16b1c3ab-55ef-412c-952b-f280424272e1";
    }
}