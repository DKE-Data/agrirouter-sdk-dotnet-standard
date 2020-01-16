namespace com.dke.data.agrirouter.api.env
{
    /// <summary>
    /// Abstract environment holding environment data.
    /// </summary>
    public abstract class Environment
    {
        /// <summary>
        /// Returning the API prefix for several AR URLs, like the onboarding URL for example.
        /// </summary>
        /// <returns>-</returns>
        protected abstract string ApiPrefix();

        /// <summary>
        /// URL for the registration service.
        /// </summary>
        /// <returns>-</returns>
        protected abstract string RegistrationServiceUrl();

        /// <summary>
        /// URL for the onboarding request.
        /// </summary>
        /// <returns>-</returns>
        public string SecuredOnboardingUrl()
        {
            return RegistrationServiceUrl() + ApiPrefix() + "/registration/onboard/request";
        }

        /// <summary>
        /// URL for the revoking request.
        /// </summary>
        /// <returns>-</returns>
        public string RevokeUrl()
        {
            return RegistrationServiceUrl() + ApiPrefix() + "/registration/onboard/revoke";
        }

        /// <summary>
        /// URL for the onboarding request.
        /// </summary>
        /// <returns>-</returns>
        public string OnboardingUrl()
        {
            return RegistrationServiceUrl() + ApiPrefix() + "/registration/onboard";
        }

        /// <summary>
        /// URL for the onboarding request.
        /// </summary>
        public string VerificationUrl()
        {
            return RegistrationServiceUrl() + ApiPrefix() + "/registration/onboard/verify";
        }

        /// <summary>
        /// URL for the authorization process.
        /// </summary>
        public string AuthorizationUrl(string applicationId)
        {
            return AuthorizationServiceUrl() + "/application/" + applicationId + "/authorize";
        }

        /// <summary>
        /// URL for the authorization process.
        /// </summary>
        protected abstract string AuthorizationServiceUrl();
    }
}