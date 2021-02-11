using System;
using System.Net.Http;
using System.Text;
using Agrirouter.Sdk.Api.Dto.Onboard;
using Agrirouter.Sdk.Api.Exception;
using Agrirouter.Sdk.Api.Service.Parameters;
using Agrirouter.Sdk.Impl.Service.Common;
using Newtonsoft.Json;
using Environment = Agrirouter.Sdk.Api.Env.Environment;

namespace Agrirouter.Sdk.Impl.Service.Onboard
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

            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new RevokeException(httpResponseMessage.StatusCode,
                    httpResponseMessage.Content.ReadAsStringAsync().Result);
        }
    }
}