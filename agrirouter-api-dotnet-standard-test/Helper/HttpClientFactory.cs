using System.Net.Http;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Logging;
using Agrirouter.Impl.Service.Common;

namespace Agrirouter.Api.Test.Helper
{
    /// <summary>
    ///     Factory to create HTTP client objects.
    /// </summary>
    public class HttpClientFactory
    {
        /// <summary>
        ///     Create a single HTTP client.
        /// </summary>
        /// <returns>-</returns>
        public static HttpClient HttpClient()
        {
            var httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()));
            return httpClient;
        }

        /// <summary>
        ///     Create a single HTTP client using the given onboarding response.
        /// </summary>
        /// <param name="onboardResponse">The current onboarding response.</param>
        /// <returns>-</returns>
        public static HttpClient AuthenticatedHttpClient(OnboardResponse onboardResponse)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ClientCertificates.Add(X509CertificateService.GetCertificate(onboardResponse));
            var httpClient = new HttpClient(new LoggingHandler(httpClientHandler));
            return httpClient;
        }

        /// <summary>
        ///     Create a single HTTP client using the given onboarding response.
        /// </summary>
        /// <param name="onboardResponse">The current onboarding response.</param>
        /// <returns>-</returns>
        public static HttpClient AuthenticatedNonLoggingHttpClient(OnboardResponse onboardResponse)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ClientCertificates.Add(X509CertificateService.GetCertificate(onboardResponse));
            var httpClient = new HttpClient(httpClientHandler);
            return httpClient;
        }
    }
}