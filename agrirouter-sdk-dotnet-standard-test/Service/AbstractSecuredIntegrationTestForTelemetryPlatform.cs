using Agrirouter.Api.Env;
using Serilog;

namespace Agrirouter.Test.Service
{
    /// <summary>
    ///     Abstract integration test class.
    /// </summary>
    public class AbstractSecuredIntegrationTestForTelemetryPlatform
    {
        protected AbstractSecuredIntegrationTestForTelemetryPlatform()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.Console()
                .WriteTo.Debug()
                .CreateLogger();
        }


        protected static Environment Environment => new QualityAssuranceEnvironment();
    }
}