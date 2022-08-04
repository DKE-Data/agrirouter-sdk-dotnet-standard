using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Emit;
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
    public class SecuredOnboardingService
    {
        private readonly Environment _environment;
        private readonly HttpClient _httpClient;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="environment">The current environment.</param>
        /// <param name="utcDataService">The UTC data service.</param>
        /// <param name="signatureService">The signature service.</param>
        /// <param name="httpClient">The current HTTP client.</param>
        public SecuredOnboardingService(Environment environment, HttpClient httpClient)
        {
            _environment = environment;
            _httpClient = httpClient;
        }

        /// <summary>
        ///     Onboard an endpoint using the simple onboarding procedure and the given parameters.
        /// </summary>
        /// <param name="onboardParameters">The onboarding parameters.</param>
        /// <param name="privateKey">The private key.</param>
        /// <returns>-</returns>
        /// <exception cref="OnboardException">Will be thrown if the onboarding was not successful.</exception>
        public OnboardResponse Onboard(OnboardParameters onboardParameters, string privateKey)
        {
            var onboardingRequest = new OnboardRequest
            {
                ExternalId = onboardParameters.Uuid,
                ApplicationId = onboardParameters.ApplicationId,
                CertificationVersionId = onboardParameters.CertificationVersionId,
                GatewayId = onboardParameters.GatewayId,
                CertificateType = onboardParameters.CertificationType,
                TimeZone = UtcDataService.TimeZone,
                UtcTimestamp = UtcDataService.SecondsInThePastFromNow(10)
            };

            var requestBody = JsonConvert.SerializeObject(onboardingRequest);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_environment.SecuredOnboardingUrl()),
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", onboardParameters.RegistrationCode);
            httpRequestMessage.Headers.Add("X-Agrirouter-ApplicationId", onboardParameters.ApplicationId);
            httpRequestMessage.Headers.Add("X-Agrirouter-Signature",
                SignatureService.XAgrirouterSignature(requestBody, privateKey));

            var httpResponseMessage = _httpClient.SendAsync(httpRequestMessage).Result;
            var result = httpResponseMessage.Content.ReadAsStringAsync().Result;

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
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
        /// <param name="privateKey">The private key.</param>
        /// <returns>-</returns>
        /// <exception cref="OnboardException">Will be thrown if the onboarding was not successful.</exception>
        public async Task<OnboardResponse> OnboardAsync(OnboardParameters onboardParameters, string privateKey)
        {
            var onboardingRequest = new OnboardRequest
            {
                ExternalId = onboardParameters.Uuid,
                ApplicationId = onboardParameters.ApplicationId,
                CertificationVersionId = onboardParameters.CertificationVersionId,
                GatewayId = onboardParameters.GatewayId,
                CertificateType = onboardParameters.CertificationType,
                TimeZone = UtcDataService.TimeZone,
                UtcTimestamp = UtcDataService.SecondsInThePastFromNow(10)
            };

            var requestBody = JsonConvert.SerializeObject(onboardingRequest);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_environment.SecuredOnboardingUrl()),
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", onboardParameters.RegistrationCode);
            httpRequestMessage.Headers.Add("X-Agrirouter-ApplicationId", onboardParameters.ApplicationId);
            httpRequestMessage.Headers.Add("X-Agrirouter-Signature",
                SignatureService.XAgrirouterSignature(requestBody, privateKey));

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);
            var result = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var onboardErrorResponse = JsonConvert.DeserializeObject<OnboardErrorResponse>(result);
                throw new OnboardException(httpResponseMessage.StatusCode, onboardErrorResponse.OnboardError);
            }

            var onboardingResponse = JsonConvert.DeserializeObject<OnboardResponse>(result);

            return onboardingResponse;
        }

        /// <summary>
        /// Verify the onboard process of an endpoint.
        /// </summary>
        /// <param name="verificationParameters">Parameters</param>
        /// <param name="privateKey">The private key</param>
        /// <returns>The account ID for the onboard process.</returns>
        /// <exception cref="VerificationException">In case the verification process failed.</exception>
        public VerificationResponse Verify(VerificationParameters verificationParameters, string privateKey)
        {
            var verificationRequest = new VerificationRequest
            {
                ExternalId = verificationParameters.ExternalId,
                ApplicationId = verificationParameters.ApplicationId,
                CertificationVersionId = verificationParameters.CertificationVersionId,
                GatewayId = verificationParameters.GatewayId,
                CertificateType = verificationParameters.CertificationType,
                TimeZone = UtcDataService.TimeZone,
                UtcTimestamp = UtcDataService.SecondsInThePastFromNow(10)
            };

            var requestBody = JsonConvert.SerializeObject(verificationRequest);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_environment.VerificationUrl()),
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", verificationParameters.RegistrationCode);
            httpRequestMessage.Headers.Add("X-Agrirouter-ApplicationId", verificationParameters.ApplicationId);
            httpRequestMessage.Headers.Add("X-Agrirouter-Signature",
                SignatureService.XAgrirouterSignature(requestBody, privateKey));

            var httpResponseMessage = _httpClient.SendAsync(httpRequestMessage).Result;
            var result = httpResponseMessage.Content.ReadAsStringAsync().Result;

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                switch (httpResponseMessage.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new VerificationException(
                            "There was an error within the request. Please check the request parameters.");
                    case HttpStatusCode.Unauthorized:
                        throw new VerificationException("The registration code is invalid.");
                    default:
                        throw new VerificationException("The request failed, the error is unknown. The error code is " +
                                                        httpResponseMessage.StatusCode + ".");
                }
            }

            var response = JsonConvert.DeserializeObject(result, typeof(VerificationResponse));
            return response as VerificationResponse;
        }

        /// <summary>
        /// Verify the onboard process of an endpoint.
        /// </summary>
        /// <param name="verificationParameters">Parameters</param>
        /// <param name="privateKey">The private key</param>
        /// <returns>The account ID for the onboard process.</returns>
        /// <exception cref="VerificationException">In case the verification process failed.</exception>
        public async Task<VerificationResponse> VerifyAsync(VerificationParameters verificationParameters,
            string privateKey)
        {
            var verificationRequest = new VerificationRequest
            {
                ExternalId = verificationParameters.ExternalId,
                ApplicationId = verificationParameters.ApplicationId,
                CertificationVersionId = verificationParameters.CertificationVersionId,
                GatewayId = verificationParameters.GatewayId,
                CertificateType = verificationParameters.CertificationType,
                TimeZone = UtcDataService.TimeZone,
                UtcTimestamp = UtcDataService.SecondsInThePastFromNow(10)
            };

            var requestBody = JsonConvert.SerializeObject(verificationRequest);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_environment.VerificationUrl()),
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", verificationParameters.RegistrationCode);
            httpRequestMessage.Headers.Add("X-Agrirouter-ApplicationId", verificationParameters.ApplicationId);
            httpRequestMessage.Headers.Add("X-Agrirouter-Signature",
                SignatureService.XAgrirouterSignature(requestBody, privateKey));

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);
            var result = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                switch (httpResponseMessage.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new VerificationException(
                            "There was an error within the request. Please check the request parameters.");
                    case HttpStatusCode.Unauthorized:
                        throw new VerificationException("The registration code is invalid.");
                    default:
                        throw new VerificationException("The request failed, the error is unknown. The error code is " +
                                                        httpResponseMessage.StatusCode + ".");
                }
            }

            var response = JsonConvert.DeserializeObject<VerificationResponse>(result);
            return response;
        }
    }
}