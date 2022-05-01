using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Newtonsoft.Json;
using Environment = Agrirouter.Api.Env.Environment;

namespace Agrirouter.Impl.Service.Onboard
{
    /// <summary>
    ///     Service for the onboarding.
    /// </summary>
    public class OnboardService
    {
        private readonly Environment _environment;
        private readonly HttpClient _httpClient;
        private readonly UtcDataService _utcDataService;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="environment">The current environment.</param>
        /// <param name="utcDataService">The UTC data service.</param>
        /// <param name="httpClient">The current HTTP client.</param>
        public OnboardService(Environment environment, UtcDataService utcDataService, HttpClient httpClient)
        {
            _environment = environment;
            _httpClient = httpClient;
            _utcDataService = utcDataService;
        }

        /// <summary>
        ///     Onboard an endpoint using the simple onboarding procedure and the given parameters.
        /// </summary>
        /// <param name="onboardParameters">The onboarding parameters.</param>
        /// <returns>-</returns>
        /// <exception cref="OnboardException">Will be thrown if the onboarding was not successful.</exception>
        public OnboardResponse Onboard(OnboardParameters onboardParameters)
        {
            var onboardingRequest = new OnboardRequest
            {
                Uuid = onboardParameters.Uuid,
                ApplicationId = onboardParameters.ApplicationId,
                CertificationVersionId = onboardParameters.CertificationVersionId,
                GatewayId = onboardParameters.GatewayId,
                CertificateType = onboardParameters.CertificationType,
                TimeZone = UtcDataService.TimeZone,
                UtcTimestamp = UtcDataService.Now
            };

            var requestBody = JsonConvert.SerializeObject(onboardingRequest);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_environment.OnboardingUrl()),
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", onboardParameters.RegistrationCode);

            var httpResponseMessage = _httpClient.SendAsync(httpRequestMessage).Result;
            var result = httpResponseMessage.Content.ReadAsStringAsync().Result;

            if (!httpResponseMessage.IsSuccessStatusCode) {
                var onboardErrorResponse = JsonConvert.DeserializeObject<OnboardErrorResponse>(result);
                throw new OnboardException(httpResponseMessage.StatusCode, onboardErrorResponse.OnboardError);
            }

            var onboardingResponse = JsonConvert.DeserializeObject(result, typeof(OnboardResponse));
            return onboardingResponse as OnboardResponse;
        }

        /// <summary>
        ///     Onboard an endpoint using the simple onboarding procedure and the given parameters.
        /// </summary>
        /// <param name="onboardParameters">The onboarding parameters.</param>
        /// <returns>-</returns>
        /// <exception cref="OnboardException">Will be thrown if the onboarding was not successful.</exception>
        public async Task<OnboardResponse> OnboardAsync(OnboardParameters onboardParameters)
        {
            var onboardingRequest = new OnboardRequest
            {
                Uuid = onboardParameters.Uuid,
                ApplicationId = onboardParameters.ApplicationId,
                CertificationVersionId = onboardParameters.CertificationVersionId,
                GatewayId = onboardParameters.GatewayId,
                CertificateType = onboardParameters.CertificationType,
                TimeZone = UtcDataService.TimeZone,
                UtcTimestamp = UtcDataService.Now
            };

            var requestBody = JsonConvert.SerializeObject(onboardingRequest);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_environment.OnboardingUrl()),
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", onboardParameters.RegistrationCode);

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            var result = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode) {
                var onboardErrorResponse = JsonConvert.DeserializeObject<OnboardErrorResponse>(result);
                throw new OnboardException(httpResponseMessage.StatusCode, onboardErrorResponse.OnboardError);
            }

            var onboardingResponse = JsonConvert.DeserializeObject<OnboardResponse>(result);

            return onboardingResponse;
        }
    }
}