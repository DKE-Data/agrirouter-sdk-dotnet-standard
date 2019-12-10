using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using com.dke.data.agrirouter.api.dto.onboard;
using com.dke.data.agrirouter.api.env;
using com.dke.data.agrirouter.api.exception;
using com.dke.data.agrirouter.api.logging;
using com.dke.data.agrirouter.api.service.onboard;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.impl.service.common;
using Newtonsoft.Json;

namespace com.dke.data.agrirouter.impl.service.onboard
{
    /**
     * Implementation.
     */
    public class OnboardingService : IOnboardingService
    {
        private readonly Environment _environment;
        private readonly UtcDataService _utcDataService;

        public OnboardingService(Environment environment)
        {
            _environment = environment;
            _utcDataService = new UtcDataService();
        }

        public OnboardingResponse Onboard(OnboardingParameters parameters)
        {
            var onboardingRequest = new OnboardingRequest
            {
                Id = parameters.Uuid,
                ApplicationId = parameters.ApplicationId,
                CertificationVersionId = parameters.CertificationVersionId,
                GatewayId = parameters.GatewayId,
                CertificateType = parameters.CertificationType,
                TimeZone = _utcDataService.TimeZone,
                UTCTimestamp = _utcDataService.Now
            };


            var jsonContent = JsonConvert.SerializeObject(onboardingRequest);
            var requestBody = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()));
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", parameters.RegistrationCode);

            var httpResponseMessage = httpClient.PostAsync(_environment.OnboardUrl(), requestBody).Result;

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