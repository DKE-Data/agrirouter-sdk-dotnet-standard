using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using com.dke.data.agrirouter.api.dto.onboard;
using com.dke.data.agrirouter.api.logging;

namespace com.dke.data.agrirouter.api.test.helper
{
    public class HttpClientFactory
    {
        public static HttpClient HttpClient()
        {
            var httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()));
            return httpClient;
        }

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