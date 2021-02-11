using Agrirouter.Sdk.Api.Env;
using Serilog;

namespace Agrirouter.Sdk.Test.Service
{
    /// <summary>
    ///     Abstract integration test class.
    /// </summary>
    public class AbstractSecuredIntegrationTestForTelemetryPlatform
    {
        protected AbstractSecuredIntegrationTestForTelemetryPlatform()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.Debug()
                .CreateLogger();
        }


        protected static Environment Environment => new QualityAssuranceEnvironment();
    }
}