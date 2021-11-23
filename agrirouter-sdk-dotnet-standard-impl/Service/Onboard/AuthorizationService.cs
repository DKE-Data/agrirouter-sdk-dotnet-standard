using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Exception;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math.EC.Rfc8032;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using Environment = Agrirouter.Api.Env.Environment;

namespace Agrirouter.Impl.Service.Onboard
{
    /// <summary>
    ///     Service for the authorization process.
    /// </summary>
    public class AuthorizationService
    {
        private static string Algorithm => "SHA-256withRSA";

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
        /// <param name="authorizationResult">The parameter string which has been returned from the redirect (everything after the initial '?')</param>
        /// <returns>The result of the parsing.</returns>
        /// <exception cref="System.ArgumentException">Will be thrown if the input is not valid.</exception>
        public AuthorizationResult Parse(string authorizationResult)
        {
            var parameters = HttpUtility.ParseQueryString(authorizationResult);
            if (parameters.Count < 2 || parameters.Count > 4)
            {
                throw new ArgumentException($"The input '{authorizationResult}' does not meet the specification");
            }
            return new AuthorizationResult
            {
                State = parameters.Get("state"),
                Signature = parameters.Get("signature"),
                Token = parameters.Get("token"),
                Error = parameters.Get("error")
            };
        }

        /// <summary>
        ///     Parsing a callback URI for parameters
        /// </summary>
        /// <param name="callbackUri">The complete URI of the callback request.</param>
        /// <returns>The result of the parsing.</returns>
        /// <exception cref="System.ArgumentException">Will be thrown if the input is not valid.</exception>
        public AuthorizationResult Parse(Uri callbackUri)
        {
            return Parse(callbackUri.Query); 
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

        /// <summary>
        /// Verify the result from the AR during the onboard process.
        /// </summary>
        /// <param name="state">State, returned from the AR.</param>
        /// <param name="token">Token, returned from the AR.</param>
        /// <param name="signature">Signature, returned from the AR.</param>
        public bool Verify(string state, string token, string signature)
        {
            var concatenatedValues = $"{state}{token}";
            try
            {
                var signer = SignerUtilities.GetSigner(Algorithm);
                signer.Init(false,
                    (RsaKeyParameters) new PemReader(new StringReader(_environment.PublicKey())).ReadObject());
                signer.BlockUpdate(Encoding.UTF8.GetBytes(concatenatedValues), 0, concatenatedValues.Length);
                return signer.VerifySignature(Base64.Decode(signature));
            }
            catch (Exception e)
            {
                throw new CouldNotVerifySignatureException("Could not verify signature.", e);
            }
        }
    }
}