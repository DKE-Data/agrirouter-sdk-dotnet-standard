using System;

namespace com.dke.data.agrirouter.api.env
{
    /**
     * Abstract environment holding environment data.
     */
    public abstract class Environment
    {
        /**
         * Returning the API prefix for several AR URLs, like the onboarding URL for example.
         *
         * @return -
         */
        protected abstract String ApiPrefix();

        /**
         * URL for the registration service.
         *
         * @return -
         */
        protected abstract String RegistrationServiceUrl();

        /**
         * URL for the onboarding request.
         *
         * @return -
         */
        public String OnboardUrl()
        {
            return RegistrationServiceUrl() + ApiPrefix() + "/registration/onboard";
        }
    }
}