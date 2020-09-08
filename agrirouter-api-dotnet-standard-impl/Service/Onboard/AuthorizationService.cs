using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Agrirouter.Api.Dto.Onboard;
using Newtonsoft.Json;
using Environment = Agrirouter.Api.Env.Environment;

namespace Agrirouter.Impl.Service.onboard
{
    /// <summary>
    ///     Service for the authorization process.
    /// </summary>
    public class AuthorizationService
    {
        private readonly Environment _environment;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="environment">The current environment.</param>
        public AuthorizationService(Environment environment)
        {
            _environment = environment;
        }

        /// <summary>
        ///     Generates the authorization URL for the application used within the onboarding process.
        /// </summary>
        /// <param name="applicationId">The application ID for the authorization.</param>
        /// <returns>-</returns>
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

        /// <summary>
        ///     Generates the authorization URL for the application used within the onboarding process and adds the redirect URI
        ///     parameter.
        /// </summary>
        /// <param name="applicationId">The application ID for the authorization.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        /// <returns>-</returns>
        public AuthorizationUrlResult AuthorizationUrl(string applicationId, string redirectUri)
        {
            var state = Guid.NewGuid().ToString();
            return new AuthorizationUrlResult
            {
                AuthorizationUrl =
                    $"{_environment.AuthorizationUrl(applicationId)}?response_type=onboard&state={state}&redirect_uri={redirectUri}",
                State = state
            };
        }

        /// <summary>
        ///     Parsing the result which was attached as parameters to the URL.
        /// </summary>
        /// <param name="authorizationResult">The result of the parsing.</param>
        /// <returns>-</returns>
        /// <exception cref="System.ArgumentException">Will be thrown if the input is not valid.</exception>
        public AuthorizationResult Parse(string authorizationResult)
        {
            var split = authorizationResult.Split('&');
            var parameters = new Dictionary<string, string>();

            if (split.Length != 2 && split.Length != 3 && split.Length != 4)
                throw new ArgumentException($"The input '{authorizationResult}' does not meet the specification");

            foreach (var parameter in split)
            {
                var parameterSplit = parameter.Split("=");
                if (parameterSplit.Length != 2)
                    throw new ArgumentException($"Parameter '{parameter}' could not be parsed.");

                parameters.Add(parameterSplit[0], HttpUtility.UrlDecode(parameterSplit[1]));
            }

            return new AuthorizationResult
            {
                State = parameters.GetValueOrDefault("state"),
                Signature = parameters.GetValueOrDefault("signature"),
                Token = parameters.GetValueOrDefault("token"),
                Error = parameters.GetValueOrDefault("error")
            };
        }

        /// <summary>
        ///     Parsing the token from the authorization result.
        /// </summary>
        /// <param name="authorizationResult">-</param>
        /// <returns>-</returns>
        public AuthorizationToken Parse(AuthorizationResult authorizationResult)
        {
            return
                (AuthorizationToken) JsonConvert.DeserializeObject(
                    Encoding.UTF8.GetString(Convert.FromBase64String(authorizationResult.Token)),
                    typeof(AuthorizationToken));
        }
    }
}