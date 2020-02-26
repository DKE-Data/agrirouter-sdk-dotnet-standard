using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Env;
using Serilog;

namespace Agrirouter.Api.Test.Service
{
    /// <summary>
    /// Abstract integration test class.
    /// </summary>
    public class AbstractIntegrationTest
    {
        private int _testStep = 1;

        private static string AccountId => "5d47a537-9455-410d-aa6d-fbd69a5cf990";

        protected static string ApplicationId => "39d18ae2-04e3-42de-8a42-935565a6b261";

        protected static string CertificationVersionId => "719afec8-d2ff-4cf8-8194-e688ae56b3b5";

        protected static Environment Environment => new QualityAssuranceEnvironment();

        protected AbstractIntegrationTest()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.Debug()
                .CreateLogger();
        }

        protected void LogTestStep(string message)
        {
            Log.Debug("******************************************************************************");
            Log.Debug($"* [{++_testStep}]: {message}");
            Log.Debug("******************************************************************************");
        }

        protected void LogDebugInformation(OnboardResponse onboardResponse)
        {
            Log.Debug("******************************************************************************");
            Log.Debug($"* [ACCOUNT_ID]: {AccountId}");
            Log.Debug($"* [APPLICATION_ID]: {ApplicationId}");
            Log.Debug($"* [CERTIFICATION_VERSION_ID]: {ApplicationId}");
            Log.Debug($"* [SENSOR_ALTERNATE_ID]: {onboardResponse.SensorAlternateId}");
            Log.Debug($"* [DEVICE_ALTERNATE_ID]: {onboardResponse.DeviceAlternateId}");
            Log.Debug("******************************************************************************");
        }
    }
}