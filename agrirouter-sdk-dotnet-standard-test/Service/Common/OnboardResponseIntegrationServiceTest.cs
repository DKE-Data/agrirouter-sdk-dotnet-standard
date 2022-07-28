using System;
using System.Net.Http;
using System.Threading;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Xunit;

namespace Agrirouter.Test.Service.Common
{
    /// <summary>
    ///     Functional tests.
    /// </summary>
    [Collection("Integrationtest")]
    public class OnboardResponseIntegrationServiceTest : AbstractIntegrationTestForCommunicationUnits
    {
        private static readonly UtcDataService UtcDataService = new UtcDataService();
        private static readonly HttpClient HttpClient = HttpClientFactory.HttpClient();

        [Fact]
        public void GivenExistingOnboardingResponseForTheIdentifierTheServiceShouldReadTheOnboardingResponseFromFile()
        {
            var onboardingResponse =
                OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.SingleEndpointWithoutRoute);
            Assert.NotNull(onboardingResponse);

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
        public void OffsetTimeDiffersFromActualTime()
        {
            int testOffset = 23;
            string firstTime = UtcDataService.Now;
            string comparison = UtcDataService.SecondsInThePastFromNow(testOffset);
            double difference = (DateTime.Parse(firstTime) - DateTime.Parse(comparison)).TotalSeconds - testOffset;
            Assert.True(difference>-0.2 && difference < 0.2); //We need a small time difference to respect the processing time.
        }
    }
}