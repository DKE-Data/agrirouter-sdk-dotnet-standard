using System;
using com.dke.data.agrirouter.api.dto.onboard;
using Environment = com.dke.data.agrirouter.api.env.Environment;

namespace com.dke.data.agrirouter.impl.service.onboard
{
    /**
     * Service for the authorization process.
     */
    public class AuthorizationService
    {
        private readonly Environment _environment;

        public AuthorizationService(Environment environment)
        {
            _environment = environment;
        }

        /**
         * Generates the authorization URL for the application used within the onboarding process..
         */
        public AuthorizationUrlResult AuthorizationUrl(string applicationId)
        {
            var state = Guid.NewGuid().ToString();
            return new AuthorizationUrlResult
            {
                AuthorizationUrl =
                    $"{_environment.AuthorizationUrl(applicationId)}?response_type=onboard&state={state}",
                State = state
            };
        }

        /**
         * Generates the authorization URL for the application used within the onboarding process and adds the redirect URI parameter.
         */
        public AuthorizationUrlResult AuthorizationUrl(string applicationId, String redirectUri)
        {
            var state = Guid.NewGuid().ToString();
            return new AuthorizationUrlResult
            {
                AuthorizationUrl =
                    $"{_environment.AuthorizationUrl(applicationId)}?response_type=onboard&state={state}&redirect_uri={redirectUri}",
                State = state
            };
        }
    }
}