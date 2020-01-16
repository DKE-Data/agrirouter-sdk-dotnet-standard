namespace Agrirouter.Api.Env
{
    /// <summary>
    /// Specific QA environment.
    /// </summary>
    public class QualityAssuranceEnvironment : Environment
    {
        private static readonly string API_PREFIX = "/api/v1.0";

        private static readonly string REGISTRATION_SERVICE_URL =
            "https://agrirouter-registration-service-hubqa-eu10.cfapps.eu10.hana.ondemand.com";

        private static readonly string AUTHORIZATION_SERVICE_URL =
            "https://agrirouter-qa.cfapps.eu10.hana.ondemand.com";

        /// <summary>
        /// API prefix for the service calls.
        /// </summary>
        /// <returns></returns>
        protected override string ApiPrefix()
        {
            return API_PREFIX;
        }

        /// <summary>
        /// URL for the registration service which is used for the onboard process and revoke process.
        /// </summary>
        /// <returns>-</returns>
        protected override string RegistrationServiceUrl()
        {
            return REGISTRATION_SERVICE_URL;
        }

        /// <summary>
        /// URL for the authorization service which is used to authorize the application to connect to the AR account.
        /// </summary>
        /// <returns>-</returns>
        protected override string AuthorizationServiceUrl()
        {
            return AUTHORIZATION_SERVICE_URL;
        }
    }
}