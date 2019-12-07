using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using com.dke.data.agrirouter.api.dto.onboard;
using com.dke.data.agrirouter.api.exception;
using com.dke.data.agrirouter.api.logging;
using com.dke.data.agrirouter.api.service.onboard;
using com.dke.data.agrirouter.api.service.parameters;
using Newtonsoft.Json;
using Environment = com.dke.data.agrirouter.api.env.Environment;

namespace com.dke.data.agrirouter.impl.service.onboard
{
    /**
     * Implementation.
     */
    public class OnboardingService : IOnboardingService
    {
        private readonly Environment _environment;

        public OnboardingService(Environment environment)
        {
            _environment = environment;
        }

        public OnboardingResponse Onboard(OnboardingParameters parameters)
        {
            OnboardingRequest onboardingRequest = new OnboardingRequest
            {
                Id = parameters.Uuid,
                ApplicationId = parameters.ApplicationId,
                CertificationVersionId = parameters.CertificationVersionId,
                GatewayId = parameters.GatewayId,
                CertificateType = parameters.CertificationType.ToString()
            };

            var timeZone = (TimeZoneInfo.Local.BaseUtcOffset < TimeSpan.Zero ? "-" : "+") +
                           TimeZoneInfo.Local.BaseUtcOffset.ToString("hh") + ":00";
            onboardingRequest.TimeZone = timeZone;
            onboardingRequest.UTCTimestamp = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");

            var jsonContent = JsonConvert.SerializeObject(onboardingRequest);
            var requestBody = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpClient httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()));
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", parameters.RegistrationCode);

            var httpResponseMessage = httpClient.PostAsync(_environment.OnboardUrl(), requestBody).Result;

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                var onboardingResponse = JsonConvert.DeserializeObject(result, typeof(OnboardingResponse));
                return onboardingResponse as OnboardingResponse;
            }
            else
            {
                throw new OnboardingException(httpResponseMessage.StatusCode, httpResponseMessage.Content.ReadAsStringAsync().Result);
            }
        }
    }
}