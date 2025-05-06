using System;
using System.Net.Http;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Onboard;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Agrirouter.Test.Service;
using Xunit;

namespace Agrirouter.Test.Documentation;

public class OnboardingForTelemetryConnectionTest : AbstractIntegrationTestForCommunicationUnits
{
    private static readonly UtcDataService UtcDataService = new();
    private static readonly HttpClient HttpClient = HttpClientFactory.HttpClient();

    [Fact(Skip = "Will not run successfully without changing the registration code.")]
    public void GivenValidRequestTokenWhenOnboardingThenThereShouldBeAValidResponse()
    {
        // [1] Create onboarding parameters
        // ============================================================
        // Define the parameters used for onboarding, please be aware that the registration code needs
        // to be replaced with an actual code to run the test case.
        var parameters = new OnboardParameters
        {
            Uuid = Guid.NewGuid().ToString(),
            ApplicationId = Applications.CommunicationUnit.ApplicationId,
            ApplicationType = ApplicationTypeDefinitions.Application,
            CertificationType = CertificationTypeDefinition.P12,
            GatewayId = GatewayTypeDefinition.Mqtt,
            RegistrationCode = "REPLACE_ME_WITH_ACTUAL_CODE",
            CertificationVersionId = Applications.CommunicationUnit.CertificationVersionId
        };

        // [2] Onboard the new telemetry connection
        // ============================================================
        // Use the onboarding service to onboard the new telemetry connection.
        var onboardService = new OnboardService(Environment, UtcDataService, HttpClient);
        var onboardResponse = onboardService.Onboard(parameters);

        // [3] Validate the onboarding response
        // ============================================================
        // Check if the onboarding response contains all necessary information. This is just necessary within the tests, but not
        // needed in production code.
        Assert.NotEmpty(onboardResponse.DeviceAlternateId);
        Assert.NotEmpty(onboardResponse.SensorAlternateId);
        Assert.NotEmpty(onboardResponse.CapabilityAlternateId);
        Assert.NotEmpty(onboardResponse.Authentication.Certificate);
        Assert.NotEmpty(onboardResponse.Authentication.Secret);
        Assert.NotEmpty(onboardResponse.Authentication.Type);
        Assert.NotEmpty(onboardResponse.ConnectionCriteria.Commands);
        Assert.NotEmpty(onboardResponse.ConnectionCriteria.Measures);
    }
}