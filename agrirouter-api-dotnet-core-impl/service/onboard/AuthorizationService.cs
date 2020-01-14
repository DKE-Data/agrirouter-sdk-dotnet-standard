using System;
using System.Collections.Generic;
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

        /**
         * Parsing the result which was attached as parameters to the URL.
         */
        public AuthorizationResult Parse(string authorizationResult)
        {
            var split = authorizationResult.Split('&');
            var parameters = new Dictionary<string, string>();
            if (split.Length == 3 || split.Length == 4)
            {
                foreach (var parameter in split)
                {
                    var parameterSplit = parameter.Split("=");
                    if (parameterSplit.Length != 2)
                    {
                        throw new ArgumentException($"Parameter '{parameter}' could not be parsed.");
                    }

                    parameters.Add(parameterSplit[0], parameterSplit[1]);
                }

                return new AuthorizationResult
                {
                    State = parameters.GetValueOrDefault("state"),
                    Signature = parameters.GetValueOrDefault("signature"),
                    Token = parameters.GetValueOrDefault("token"),
                    Error = parameters.GetValueOrDefault("error")
                };
            }

            throw new ArgumentException($"The input '{authorizationResult}' does not meet the specification");
        }

        public AuthorizationToken Parse(AuthorizationResult authorizationResult)
        {
            
        }
    }
}