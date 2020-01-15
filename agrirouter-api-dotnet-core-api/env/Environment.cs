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
        protected abstract string ApiPrefix();

        /**
         * URL for the registration service.
         *
         * @return -
         */
        protected abstract string RegistrationServiceUrl();

        /**
         * URL for the onboarding request.
         *
         * @return -
         */
        public string SecuredOnboardingUrl()
        {
            return RegistrationServiceUrl() + ApiPrefix() + "/registration/onboard/request";
        }

        /**
         * URL for the revoking request.
         *
         * @return -
         */
        public string RevokeUrl()
        {
            return RegistrationServiceUrl() + ApiPrefix() + "/registration/onboard/revoke";
        }

        /**
         * URL for the onboarding request.
         *
         * @return -
         */
        public string OnboardingUrl()
        {
            return RegistrationServiceUrl() + ApiPrefix() + "/registration/onboard";
        }

        /**
         * URL for the onboarding request.
         */
        public string VerificationUrl()
        {
            return RegistrationServiceUrl() + ApiPrefix() + "/registration/onboard/verify";
        }

        /**
         * URL for the authorization process.
         */
        public string AuthorizationUrl(string applicationId)
        {
            return AuthorizationServiceUrl() + "/application/" + applicationId + "/authorize";
        }

        /**
         * URL for the authorization process.
         */
        protected abstract string AuthorizationServiceUrl();
    }
}