using System;

namespace com.dke.data.agrirouter.api.env
{
    /**
     * Specific QA environment.
     */
    public class QA : Environment
    {
        private static String API_PREFIX = "/api/v1.0";

        private static String REGISTRATION_SERVICE_URL =
            "https://agrirouter-registration-service-hubqa-eu10.cfapps.eu10.hana.ondemand.com";


        protected override string ApiPrefix()
        {
            return API_PREFIX;
        }

        protected override string RegistrationServiceUrl()
        {
            return REGISTRATION_SERVICE_URL;
        }
    }
}