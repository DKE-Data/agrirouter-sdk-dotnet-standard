using System;
using com.dke.data.agrirouter.api.dto.onboard;
using Environment = com.dke.data.agrirouter.api.env.Environment;

namespace com.dke.data.agrirouter.impl.service.onboard
{
    public class AuthorizationService
    {
        private readonly Environment _environment;

        public AuthorizationService(Environment environment)
        {
            _environment = environment;
        }

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