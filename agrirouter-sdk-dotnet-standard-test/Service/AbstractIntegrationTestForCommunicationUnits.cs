using Agrirouter.Api.Env;
using Serilog;

namespace Agrirouter.Test.Service
{
    /// <summary>
    ///     Abstract integration test class.
    /// </summary>
    public class AbstractIntegrationTestForCommunicationUnits
    {
        protected AbstractIntegrationTestForCommunicationUnits()
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