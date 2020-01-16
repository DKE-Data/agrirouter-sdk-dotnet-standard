using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Logging;

namespace Agrirouter.Api.test.helper
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
        /// <param name="onboardingResponse">The current onboarding response.</param>
        /// <returns>-</returns>
        public static HttpClient AuthenticatedHttpClient(OnboardingResponse onboardingResponse)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ClientCertificates.Add(new X509Certificate2(
                Convert.FromBase64String(onboardingResponse.Authentication.Certificate),
                onboardingResponse.Authentication.Secret));
            var httpClient = new HttpClient(new LoggingHandler(httpClientHandler));
            return httpClient;
        }
    }
}