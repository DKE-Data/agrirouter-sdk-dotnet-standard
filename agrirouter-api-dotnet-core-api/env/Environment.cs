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
         */
        protected abstract String ApiPrefix();

        /**
         * URL for the registration service.
         */
        protected abstract String RegistrationServiceUrl();

        /**
         * URL for the onboarding request.
         */
        public String OnboardUrl()
        {
            return RegistrationServiceUrl() + ApiPrefix() + "/registration/onboard";
        }
        
        /**
         * URL for the onboarding request.
         */
        public String VerificationUrl()
        {
            return RegistrationServiceUrl() + ApiPrefix() + "/registration/onboard/verify";
        }

        /**
         * Public key for secured communication.
         */
        public String PublicKey()
        {
            throw new NotImplementedException("Public key is only necessary if the application does support secured communication.");
        }
        
        /**
         * Public key for secured communication.
         */
        public String PrivateKey()
        {
            throw new NotImplementedException("Private key is only necessary if the application does support secured communication.");
        }
    }
}