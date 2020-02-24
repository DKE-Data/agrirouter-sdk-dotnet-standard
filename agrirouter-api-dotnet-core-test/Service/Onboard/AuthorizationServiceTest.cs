using Agrirouter.Api.Env;
using Agrirouter.Impl.Service.onboard;
using Xunit;

namespace Agrirouter.Api.Test.Service.Onboard
{
    /// <summary>
    /// Functional tests.
    /// </summary>
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
        public void
            GivenValidApplicationIdAndRedirectUriWhenCreatingAuthorizationUrlThenTheUrlShouldBeFineDuringManualTesting()
        {
            var authorizationService = new AuthorizationService(new QualityAssuranceEnvironment());
            var authorizationUrlResult =
                authorizationService.AuthorizationUrl(ApplicationId, "https://www.saschadoemer.de");
            Assert.Equal(
                $"https://agrirouter-qa.cfapps.eu10.hana.ondemand.com/application/16b1c3ab-55ef-412c-952b-f280424272e1/authorize?response_type=onboard&state={authorizationUrlResult.State}&redirect_uri=https://www.saschadoemer.de",
                authorizationUrlResult.AuthorizationUrl);
        }

        [Fact]
        public void GivenValidResponseWhenParsingTheResultThenTheAuthorizationRequestObjectWShouldBeFilled()
        {
            const string input = "state=6eab2086-0ef2-4b64-94b0-2ce620e66ece&token=eyJhY2NvdW50IjoiNWQ0N2E1MzctOTQ1NS00MTBkLWFhNmQtZmJkNjlhNWNmOTkwIiwicmVnY29kZSI6IjI2NGQwNjgzYzkiLCJleHBpcmVzIjoiMjAyMC0wMS0xNFQxMDowOTo1OS4zMTlaIn0%3D&signature=AJOFQmO4Y%2FT8DlNOcTAfpymMFiZQBpJHr4%2FUOfrHuGpzst6UA4kQraJYJtUEKSeEaQ%2FHCf4rJlUcK14ygyGAUtGkca1Y1sUAC1lVggVnECFMnVQAyTQzSnd1DEXjqI8n4Ud4LujSF6oSbiK0DWg1U8U9swwAEQ73Z0SDna7M3OEirY8zPUhGFcRij%2FrJOEFujq2rW%2Bs267z1pnp6FNq%2BoK5nbPBuH0hvCZ57Fz3HI1VadyE77o6rOAZ1HXniGqCGr%2F6v4TqAQ22MY9xhMAfUihtwQ3VLtdHsGSu1OH%2Fs71IQczOzBgeIlMAl4mchRo3l16qSU4k4awufLq7LzDSf5Q%3D%3D";
            var authorizationService = new AuthorizationService(new QualityAssuranceEnvironment());
            var authorizationResult = AuthorizationService.Parse(input);
            Assert.NotNull(authorizationResult.State);
            Assert.NotNull(authorizationResult.Token);
            Assert.NotNull(authorizationResult.Signature);
            Assert.Null(authorizationResult.Error);
        }

        [Fact]
        public void GivenValidResponseWhenParsingTheTokenThenTheAuthorizationTokenObjectWShouldBeFilled()
        {
            const string input = "state=6eab2086-0ef2-4b64-94b0-2ce620e66ece&token=eyJhY2NvdW50IjoiNWQ0N2E1MzctOTQ1NS00MTBkLWFhNmQtZmJkNjlhNWNmOTkwIiwicmVnY29kZSI6IjI2NGQwNjgzYzkiLCJleHBpcmVzIjoiMjAyMC0wMS0xNFQxMDowOTo1OS4zMTlaIn0%3D&signature=AJOFQmO4Y%2FT8DlNOcTAfpymMFiZQBpJHr4%2FUOfrHuGpzst6UA4kQraJYJtUEKSeEaQ%2FHCf4rJlUcK14ygyGAUtGkca1Y1sUAC1lVggVnECFMnVQAyTQzSnd1DEXjqI8n4Ud4LujSF6oSbiK0DWg1U8U9swwAEQ73Z0SDna7M3OEirY8zPUhGFcRij%2FrJOEFujq2rW%2Bs267z1pnp6FNq%2BoK5nbPBuH0hvCZ57Fz3HI1VadyE77o6rOAZ1HXniGqCGr%2F6v4TqAQ22MY9xhMAfUihtwQ3VLtdHsGSu1OH%2Fs71IQczOzBgeIlMAl4mchRo3l16qSU4k4awufLq7LzDSf5Q%3D%3D";
            var authorizationService = new AuthorizationService(new QualityAssuranceEnvironment());
            var authorizationResult = AuthorizationService.Parse(input);
            Assert.NotNull(authorizationResult.State);
            Assert.NotNull(authorizationResult.Token);
            Assert.NotNull(authorizationResult.Signature);
            Assert.Null(authorizationResult.Error);
            var authorizationToken = AuthorizationService.Parse(authorizationResult);
            Assert.NotNull(authorizationToken.Account);
            Assert.NotNull(authorizationToken.RegistrationCode);
            Assert.NotNull(authorizationToken.Expires);
        }

        private static string ApplicationId => "16b1c3ab-55ef-412c-952b-f280424272e1";
    }
}