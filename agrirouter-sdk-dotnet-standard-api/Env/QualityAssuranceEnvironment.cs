// ReSharper disable InconsistentNaming

namespace Agrirouter.Api.Env
{
    /// <summary>
    ///     Specific QA environment.
    /// </summary>
    public class QualityAssuranceEnvironment : Environment
    {
        private const string API_PREFIX = "/api/v1.0";

        private const string REGISTRATION_SERVICE_URL =
            "https://agrirouter-registration-service-hubqa-eu10.cfapps.eu10.hana.ondemand.com";

        private const string AUTHORIZATION_SERVICE_URL = "https://agrirouter-qa.cfapps.eu10.hana.ondemand.com";

        private const string PUBLIC_KEY =
            "-----BEGIN PUBLIC KEY-----\n\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAy8xF9661acn+iS+QS+9Y\n\n3HvTfUVcismzbuvxHgHA7YeoOUFxyj3lkaTnXm7hzQe4wDEDgwpJSGAzxIIYSUXe\n\n8EsWLorg5O0tRexx5SP3+kj1i83DATBJCXP7k+bAF4u2FVJphC1m2BfLxelGLjzx\n\nVAS/v6+EwvYaT1AI9FFqW/a2o92IsVPOh9oM9eds3lBOAbH/8XrmVIeHofw+XbTH\n\n1/7MLD6IE2+HbEeY0F96nioXArdQWXcjUQsTch+p0p9eqh23Ak4ef5oGcZhNd4yp\n\nY8M6ppvIMiXkgWSPJevCJjhxRJRmndY+ajYGx7CLePx7wNvxXWtkng3yh+7WiZ/Y\n\nqwIDAQAB\n\n-----END PUBLIC KEY-----";

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