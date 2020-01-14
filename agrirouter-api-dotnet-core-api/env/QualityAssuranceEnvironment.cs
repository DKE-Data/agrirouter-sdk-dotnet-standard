namespace com.dke.data.agrirouter.api.env
{
    /**
     * Specific QA environment.
     */
    public class QualityAssuranceEnvironment : Environment
    {
        private static readonly string API_PREFIX = "/api/v1.0";

        private static readonly string REGISTRATION_SERVICE_URL =
            "https://agrirouter-registration-service-hubqa-eu10.cfapps.eu10.hana.ondemand.com";

        private static readonly string AUTHORIZATION_SERVICE_URL = "https://agrirouter-qa.cfapps.eu10.hana.ondemand.com";


        protected override string ApiPrefix()
        {
            return API_PREFIX;
        }

        protected override string RegistrationServiceUrl()
        {
            return REGISTRATION_SERVICE_URL;
        }

        protected override string AuthorizationServiceUrl()
        {
            return AUTHORIZATION_SERVICE_URL;
        }
    }
}