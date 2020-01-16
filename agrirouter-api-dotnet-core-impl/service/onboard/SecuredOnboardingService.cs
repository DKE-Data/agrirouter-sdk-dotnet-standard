using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Newtonsoft.Json;
using Environment = Agrirouter.Api.Env.Environment;

namespace Agrirouter.Impl.Service.onboard
{
    /// <summary>
    /// Service for the onboarding.
    /// </summary>
    public class SecuredOnboardingService
    {
        private readonly Environment _environment;
        private readonly HttpClient _httpClient;
        private readonly UtcDataService _utcDataService;
        private readonly SignatureService _signatureService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="environment">The current environment.</param>
        /// <param name="utcDataService">The UTC data service.</param>
        /// <param name="signatureService">The signature service.</param>
        /// <param name="httpClient">The current HTTP client.</param>
        public SecuredOnboardingService(Environment environment, UtcDataService utcDataService,
            SignatureService signatureService, HttpClient httpClient)
        {
            _environment = environment;
            _httpClient = httpClient;
            _utcDataService = utcDataService;
            _signatureService = signatureService;
        }

        /// <summary>
        /// Onboard an endpoint using the simple onboarding procedure and the given parameters.
        /// </summary>
        /// <param name="onboardingParameters">The onboarding parameters.</param>
        /// <param name="privateKey">The private key.</param>
        /// <returns>-</returns>
        /// <exception cref="OnboardingException">Will be thrown if the onboarding was not successful.</exception>
        public OnboardingResponse Onboard(OnboardingParameters onboardingParameters, string privateKey)
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

            var requestBody = JsonConvert.SerializeObject(onboardingRequest);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_environment.SecuredOnboardingUrl()),
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", onboardingParameters.RegistrationCode);
            httpRequestMessage.Headers.Add("X-Agrirouter-ApplicationId", onboardingParameters.ApplicationId);
            httpRequestMessage.Headers.Add("X-Agrirouter-Signature",
                _signatureService.XAgrirouterSignature(requestBody, privateKey));

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