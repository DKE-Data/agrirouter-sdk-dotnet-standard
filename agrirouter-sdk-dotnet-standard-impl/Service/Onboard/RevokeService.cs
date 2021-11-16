using System;
using System.Net.Http;
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
    ///     Service for revoking endpoints.
    /// </summary>
    public class RevokeService
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
        public RevokeService(Environment environment, HttpClient httpClient)
        {
            _environment = environment;
            _httpClient = httpClient;
        }

        /// <summary>
        ///     Revoke an existing endpoint.
        /// </summary>
        /// <param name="revokeParameters">The parameters for the revoke process.</param>
        /// <param name="privateKey">The private key.</param>
        /// <exception cref="RevokeException">Will be thrown if the revoking was not successful.</exception>
        public void Revoke(RevokeParameters revokeParameters, string privateKey)
        {
            var revokeRequest = new RevokeRequest
            {
                AccountId = revokeParameters.AccountId,
                EndpointIds = revokeParameters.EndpointIds,
                TimeZone = UtcDataService.TimeZone,
                UtcTimestamp = UtcDataService.Now
            };

            var requestBody = JsonConvert.SerializeObject(revokeRequest);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(_environment.RevokeUrl()),
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Add("X-Agrirouter-ApplicationId", revokeParameters.ApplicationId);
            httpRequestMessage.Headers.Add("X-Agrirouter-Signature",
                SignatureService.XAgrirouterSignature(requestBody, privateKey));

            var httpResponseMessage = _httpClient.SendAsync(httpRequestMessage).Result;

            if (!httpResponseMessage.IsSuccessStatusCode) {
                var onboardErrorResponse = JsonConvert.DeserializeObject<OnboardErrorResponse>(httpResponseMessage.Content.ReadAsStringAsync().Result);
                throw new RevokeException(httpResponseMessage.StatusCode, onboardErrorResponse.OnboardError);
            }
        }

        /// <summary>
        ///     Revoke an existing endpoint.
        /// </summary>
        /// <param name="revokeParameters">The parameters for the revoke process.</param>
        /// <param name="privateKey">The private key.</param>
        /// <exception cref="RevokeException">Will be thrown if the revoking was not successful.</exception>
        public async Task RevokeAsync(RevokeParameters revokeParameters, string privateKey)
        {
            var revokeRequest = new RevokeRequest
            {
                AccountId = revokeParameters.AccountId,
                EndpointIds = revokeParameters.EndpointIds,
                TimeZone = UtcDataService.TimeZone,
                UtcTimestamp = UtcDataService.Now
            };

            var requestBody = JsonConvert.SerializeObject(revokeRequest);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(_environment.RevokeUrl()),
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };
            httpRequestMessage.Headers.Add("X-Agrirouter-ApplicationId", revokeParameters.ApplicationId);
            httpRequestMessage.Headers.Add("X-Agrirouter-Signature",
                SignatureService.XAgrirouterSignature(requestBody, privateKey));

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode) {
                var onboardErrorResponse = JsonConvert.DeserializeObject<OnboardErrorResponse>(await httpResponseMessage.Content.ReadAsStringAsync());
                throw new RevokeException(httpResponseMessage.StatusCode, onboardErrorResponse.OnboardError);
            }
        }
    }
}