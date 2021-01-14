using Agrirouter.Api.Env;
using Serilog;

namespace Agrirouter.Api.Test.Service
{
    /// <summary>
    ///     Abstract integration test class.
    /// </summary>
    public class AbstractIntegrationTestForCommunicationUnits
    {

        protected AbstractIntegrationTestForCommunicationUnits()
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