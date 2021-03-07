using Agrirouter.Api.Env;
using Serilog;

namespace Agrirouter.Test.Service
{
    /// <summary>
    ///     Abstract integration test class.
    /// </summary>
    public class AbstractSecuredIntegrationTestForFarmingSoftware
    {
        protected AbstractSecuredIntegrationTestForFarmingSoftware()
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