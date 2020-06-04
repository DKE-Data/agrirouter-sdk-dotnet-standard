using Agrirouter.Api.Env;
using Serilog;

namespace Agrirouter.Api.Test.Service
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

        protected static string PrivateKey => "-----BEGIN PRIVATE KEY-----\n" +
                                              "MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQC8WSEtHYyiiFDM\n" +
                                              "cAp2/uLRhNK7GLBEfBdzBA1kRh39+LejS4ojoDv/kJMTQ3IMsAGZhRnFFswgRuRU\n" +
                                              "fK8j7ZCke/iX4vR020H/QiiC3cx91v39hwPGtbBY8KeIKms3LJt6zXq6nqX3Adhz\n" +
                                              "jaK+Uf3j63zjz4cMHA4oc5RxzBgdZi+KiujZFH7njc1obrmNAGH7FI8BG9j8V00R\n" +
                                              "Xyhkcz/igoEjQPnUDAipvdaGKVclQdFosy+if5wbanh0jPfYdqv2nO9sJ+iUQs9+\n" +
                                              "WpvfGdwunRwDSiyPBL2YtbDUEM+jbCtbVxxt0mZZfipb4gXXpJfUIKH+5AiTQvtD\n" +
                                              "Kkbggjs/AgMBAAECggEAFnrU2NukA2odqqjctuMpEzS1KhuCPkpJSjL7njI4Ni3P\n" +
                                              "TTAg5qujPZd11mDAZf9D0ZzvPHTCHEAZ1HOlO7aJAvjA7O/rmV7p+5tb1nK+i25z\n" +
                                              "liTdMIRS7eqbNEdL0KQEUp3fvhJdYKPZv8Dx4aFLmj5rA42rYqiej/lriEGBx1BM\n" +
                                              "Uq2ATRiuaW0Nh2Pz2F83nwZ8TRYqEcY41AycNKiOGNe2sPEKC0DmnRh4t4f2+MZw\n" +
                                              "iapURj54BlXcpYlz0sAvMedLNgP94buUnUgDaGRXyqFtkouzQWMOFsCciNm7HVyz\n" +
                                              "07wgrYX4AHV1GZibVQOz2V6J/3SRouIdNcIvdTCZ8QKBgQDvlEeigu+Oxbnir5/4\n" +
                                              "ci3ONvR3GwrplQ+/UIBjQ+fYz3Iyll7ni/zoOdAt63UR5OgwJYdq2WwNpc7Osb09\n" +
                                              "VnbDGQjFCrKunLFaHvXnvfwNBPxbJakrEadMZBVXN9S/UxGBggeaW0xQZasIJ7C4\n" +
                                              "9OpTX3yN7RJMZRwPSfOOBjHyRQKBgQDJQe+npo7c0alGBOKMExBeR8E0H9lTKTeg\n" +
                                              "ytaDoRN2NvCkJVcCfzU6B+Hys2ylDioQy/JMyPySjoz4AEWSLhWT1bVAAQBTHIDS\n" +
                                              "OiDNg1/qCIhU3RBJyJBzLPHum08+XALpuFJwrCD4nS2b2lSdzVregULldv8PhjL2\n" +
                                              "c2AM/BZRswKBgQDc9glLnS+Molha9le4MLaGZrD52Psri8tGS9zdsJd4o3tTpNRL\n" +
                                              "AhVzqT6T24HRyylKnpJSvcymmbIveEZs9/342nH4KXG7EdGQqNVrsxFwgJSvDAEG\n" +
                                              "M/X0wqncEyYof1i59U9F0caEsAAaOWEIdPpZgsvBqlEiHG7QuwfiiVwvcQKBgCA1\n" +
                                              "7l5duPW8lKQBOiUDFBaS02g9RLIboayZajXM/Olpp6AN3dwnceRkyJPohZXxK2he\n" +
                                              "y9vgOxRVvlge6wOXXpq3lHe28U9b+34qEX/y69HwJam3a9jzQbM9WUdAEjG/1jOD\n" +
                                              "7aXc1rYTqe4MxogvCsEb3RIOtonMh7GMC65oVkYjAoGAK7wjt5s/vHf+fGnnAsRp\n" +
                                              "tZTaNR/JnAuI8Uv2+V4dyXFBoQWDja2j5cD5wtJCHpDm4oud3Jkvm9/Pw1ZyQTCU\n" +
                                              "tvpK8kR8MPQcct0LSYyHOTf1wjNcqtuhlCWMVzNklSIhkKn3xkGSOLeVswP81g1h\n" +
                                              "e5E53fgi7iWqO0AEDUY8XcY=\n" +
                                              "-----END PRIVATE KEY-----";

        protected static string ApplicationId => "8ce2641e-407d-4ba0-9735-bf81b7ba07a9";

        protected static string CertificationVersionId => "95e8eb40-dc4b-4e0a-b6b2-647b2e8c3a37";

        protected static Environment Environment => new QualityAssuranceEnvironment();
    }
}