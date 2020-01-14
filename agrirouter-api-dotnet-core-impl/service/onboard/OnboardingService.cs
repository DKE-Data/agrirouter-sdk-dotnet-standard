using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using com.dke.data.agrirouter.api.dto.onboard;
using com.dke.data.agrirouter.api.exception;
using com.dke.data.agrirouter.api.service.onboard;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.impl.service.common;
using Newtonsoft.Json;
using Environment = com.dke.data.agrirouter.api.env.Environment;

namespace com.dke.data.agrirouter.impl.service.onboard
{
    /**
     * Service for the onboarding.
     */
    public class OnboardingService : IOnboardingService
    {
        private readonly Environment _environment;
        private readonly HttpClient _httpClient;
        private readonly UtcDataService _utcDataService;

        public OnboardingService(Environment environment, UtcDataService utcDataService, HttpClient httpClient)
        {
            _environment = environment;
            _httpClient = httpClient;
            _utcDataService = utcDataService;
        }

        /**
         * Onboard an endpoint using the simple onboarding procedure and the given parameters.
         */
        public OnboardingResponse Onboard(OnboardingParameters onboardingParameters)
        {
            var onboardingRequest = new OnboardingRequest
            {
                Id = onboardingParameters.Uuid,
                ApplicationId = onboardingParameters.ApplicationId,
                CertificationVersionId = onboardingParameters.CertificationVersionId,
                GatewayId = onboardingParameters.GatewayId,
                CertificateType = onboardingParameters.CertificationType,
                TimeZone = _utcDataService.TimeZone,
                UTCTimestamp = _utcDataService.Now
            };

            var jsonContent = JsonConvert.SerializeObject(onboardingRequest);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_environment.OnboardUrl()),
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", onboardingParameters.RegistrationCode);

            var httpResponseMessage = _httpClient.SendAsync(httpRequestMessage).Result;

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var onboardingResponse = JsonConvert.DeserializeObject(result, typeof(OnboardingResponse));
                return onboardingResponse as OnboardingResponse;
            }

            throw new OnboardingException(httpResponseMessage.StatusCode,
                httpResponseMessage.Content.ReadAsStringAsync().Result);
        }
    }
}