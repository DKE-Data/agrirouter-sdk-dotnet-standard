using Agrirouter.Api.Dto.Onboard;
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

        protected static string ApplicationId => "434989e2-b4be-4cfd-8e40-f5b89d83458d";

        protected static string CertificationVersionId => "f491d487-f913-4732-8be4-c2eacff21816";

        protected static Environment Environment => new QualityAssuranceEnvironment();

    }
}