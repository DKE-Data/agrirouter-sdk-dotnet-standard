// ReSharper disable InconsistentNaming

namespace Agrirouter.Api.Env
{
    /// <summary>
    ///     Specific QA environment.
    /// </summary>
    public class Ar2QualityAssuranceEnvironment : Environment
    {
        private const string API_PREFIX = "/api/v1.0";

        private const string REGISTRATION_SERVICE_URL =
            "https://endpoint-service.qa.agrirouter.farm/";

        private const string AUTHORIZATION_SERVICE_URL = "https://app.qa.agrirouter.farm";

        private const string PUBLIC_KEY =
            "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAw4DStz1cikiCqTd70p9i\nRBBx4vdTEtZAaWtvswu/IdMNoXP30+1ExVc3oJ0wHn3DMWItMLtn0gUSjj+XzDN5\nyrmwUSS6qqyAFinLBUio88EyEQAocZo270bDk9gSndftIvvQ82Iu6p4gRg1zbPNF\nCoBdLCQx7MN2zbl+/kmuZXzeEXZwAT94O8IbbTTAz9Wy5MUrAlJwNVaZir9bY6AZ\nCvgUPNRL2Jq9yz8IeoawhLNOo6ae47Jcf88x+7t/eN8QSrGu50WD1qpZbTReH7FA\nju9qUVOmP1P9rSYkuhrkWg16Qrw1t8hEqMiRDNYUUTkqEit+H1CNEBgr6t3RIC5t\nfQIDAQAB\n-----END PUBLIC KEY-----";

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