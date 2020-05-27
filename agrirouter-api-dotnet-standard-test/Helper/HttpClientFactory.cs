using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Logging;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Agrirouter.Api.Test.Helper
{
    /// <summary>
    /// Factory to create HTTP client objects.
    /// </summary>
    public class HttpClientFactory
    {
        /// <summary>
        /// Create a single HTTP client.
        /// </summary>
        /// <returns>-</returns>
        public static HttpClient HttpClient()
        {
            var httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()));
            return httpClient;
        }

        /// <summary>
        /// Create a single HTTP client using the given onboarding response.
        /// </summary>
        /// <param name="onboardResponse">The current onboarding response.</param>
        /// <returns>-</returns>
        public static HttpClient AuthenticatedHttpClient(OnboardResponse onboardResponse)
        {
            if (onboardResponse.Authentication.Type.Equals("P12"))
            {
                return CreateAuthenticatedHttpClientForP12(onboardResponse);
            }

            if (onboardResponse.Authentication.Type.Equals("PEM"))
            {
                return CreateAuthenticatedHttpClientForPem(onboardResponse);
            }

            throw new System.Exception("Could not create client, authentication type has to be PEM or P12.");
        }

        /// <summary>
        /// Creating a authenticated client for a P12 certificate.
        /// </summary>
        /// <param name="onboardResponse"></param>
        /// <returns></returns>
        private static HttpClient CreateAuthenticatedHttpClientForP12(OnboardResponse onboardResponse)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ClientCertificates.Add(new X509Certificate2(
                Convert.FromBase64String(onboardResponse.Authentication.Certificate),
                onboardResponse.Authentication.Secret));
            var httpClient = new HttpClient(new LoggingHandler(httpClientHandler));
            return httpClient;
        }

        /// <summary>
        /// Creating a authenticated client for a PEM certificate.
        /// </summary>
        /// <param name="onboardResponse"></param>
        /// <returns></returns>
        private static HttpClient CreateAuthenticatedHttpClientForPem(OnboardResponse onboardResponse)
        {
            var httpClientHandler = new HttpClientHandler();
            var certificateWithoutPrivateKey = new X509Certificate2(
                Encoding.UTF8.GetBytes(onboardResponse.Authentication.Certificate),
                onboardResponse.Authentication.Secret);
            var rsa = ExtractRsaFromPem(onboardResponse.Authentication.Certificate,
                onboardResponse.Authentication.Secret);
            var certificate = CombineCertificateAndRsaKey(certificateWithoutPrivateKey, rsa);
            httpClientHandler.ClientCertificates.Add(certificate);
            var httpClient = new HttpClient(new LoggingHandler(httpClientHandler));
            return httpClient;
        }

        /// <summary>
        /// Create a single HTTP client using the given onboarding response.
        /// </summary>
        /// <param name="onboardResponse">The current onboarding response.</param>
        /// <returns>-</returns>
        public static HttpClient AuthenticatedNonLoggingHttpClient(OnboardResponse onboardResponse)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ClientCertificates.Add(new X509Certificate2(
                Convert.FromBase64String(onboardResponse.Authentication.Certificate),
                onboardResponse.Authentication.Secret));
            var httpClient = new HttpClient(httpClientHandler);
            return httpClient;
        }

        /// <summary>
        /// Extract RSA from PEM to enhance the certificate.
        /// </summary>
        /// <param name="pem"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        private static RSA ExtractRsaFromPem(string pem, string secret)
        {
            var rsa = RSA.Create();

            RSA MakePublicRcsp(RSA rcsp, RsaKeyParameters rkp)
            {
                var rsaParameters = DotNetUtilities.ToRSAParameters(rkp);
                rcsp.ImportParameters(rsaParameters);
                return rsa;
            }

            RSA MakePrivateRcsp(RSA rcsp, RsaPrivateCrtKeyParameters rkp)
            {
                var rsaParameters = DotNetUtilities.ToRSAParameters(rkp);
                rcsp.ImportParameters(rsaParameters);
                return rsa;
            }

            var pemReader = new PemReader(new StringReader(pem), new PasswordFinder(secret));
            var kp = pemReader.ReadObject();

            var hasPrivate = kp.GetType().GetProperty("Private") != null;
            var isPrivate = kp is RsaPrivateCrtKeyParameters;
            return isPrivate
                ? MakePrivateRcsp(rsa, (RsaPrivateCrtKeyParameters) kp)
                : hasPrivate
                    ? MakePrivateRcsp(rsa, (RsaPrivateCrtKeyParameters) (((AsymmetricCipherKeyPair) kp).Private))
                    : MakePublicRcsp(rsa, (RsaKeyParameters) kp);
        }

        private static X509Certificate2 CombineCertificateAndRsaKey(X509Certificate2 certificate, RSA rsa)
        {
            return certificate.CopyWithPrivateKey(rsa);
        }

        /// <summary>
        /// Internal implementation of the password finder to pass the secret to the PEM reader.
        /// </summary>
        private class PasswordFinder : IPasswordFinder
        {
            private readonly string _password;

            public PasswordFinder(string password)
            {
                this._password = password;
            }

            public char[] GetPassword()
            {
                return _password.ToCharArray();
            }
        }
    }
}