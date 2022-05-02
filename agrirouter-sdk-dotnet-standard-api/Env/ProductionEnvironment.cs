// ReSharper disable InconsistentNaming

namespace Agrirouter.Api.Env
{
    /// <summary>
    ///     Specific QA environment.
    /// </summary>
    public class ProductionEnvironment : Environment
    {
        private const string API_PREFIX = "/api/v1.0";

        private const string REGISTRATION_SERVICE_URL =
            "https://onboard.my-agrirouter.com";

        private const string AUTHORIZATION_SERVICE_URL = "https://goto.my-agrirouter.com";

        private const string PUBLIC_KEY =
            "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwCxD31sYtzH9NTfZ6n8H\n+H/QgOaoTL9GAakplAsdwYSLjBpgYMZOHIgkdM9ksRP8WsITChtZtxrCnBjR8bap\nekPT/pM9zPZlNEPxUlylJNwwTWjzTJP03+Yr07Q8v8fTJ5VWzAHlHtGQ/sI7yXA8\npzruTNre1MzxO3lkljt2Q2e7CVXAp1b53BghgysppL9Bl7NK1R+vdWSs0B1Db/Gj\nalOkWUnhivTjRMX61RGDCQSVSEaX12EvJX7FooAsW3NFeZCgeZGWEa5ZMALIiBL4\nGNASOOHju7ewlYjkyGIRxxAoc3C0w5dg1qlLiAFWToYwgDOcUpLRjU/7bzGiGvp8\nRwIDAQAB\n-----END PUBLIC KEY-----";

        /// <summary>
        ///     API prefix for the service calls.
        /// </summary>
        /// <returns></returns>
        protected override string ApiPrefix()
        {
            return API_PREFIX;
        }

        /// <summary>
        ///     URL for the registration service which is used for the onboard process and revoke process.
        /// </summary>
        /// <returns>-</returns>
        protected override string RegistrationServiceUrl()
        {
            return REGISTRATION_SERVICE_URL;
        }

        /// <summary>
        ///     URL for the authorization service which is used to authorize the application to connect to the AR account.
        /// </summary>
        /// <returns>-</returns>
        protected override string AuthorizationServiceUrl()
        {
            return AUTHORIZATION_SERVICE_URL;
        }

        /// <summary>
        /// The public key, need for signature validation in example.
        /// </summary>
        /// <returns>The public key of the QA environment.</returns>
        public override string PublicKey()
        {
            return PUBLIC_KEY;
        }
    }
}