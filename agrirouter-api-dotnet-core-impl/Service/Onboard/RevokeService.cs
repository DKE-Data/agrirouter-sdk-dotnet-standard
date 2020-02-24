using System;
using System.Net.Http;
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
    /// Service for revoking endpoints.
    /// </summary>
    public class RevokeService
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
        public RevokeService(Environment environment, UtcDataService utcDataService,
            SignatureService signatureService, HttpClient httpClient)
        {
            _environment = environment;
            _httpClient = httpClient;
            _utcDataService = utcDataService;
            _signatureService = signatureService;
        }

        /// <summary>
        /// Revoke an existing endpoint.
        /// </summary>
        /// <param name="revokeParameters">The parameters for the revoke process.</param>
        /// <param name="privateKey">The private key.</param>
        /// <exception cref="RevokeException">Will be thrown if the revoking was not successful.</exception>
        public void Revoke(RevokeParameters revokeParameters, string privateKey)
        {
            var revokeRequest = new RevokeRequest()
            {
                AccountId = revokeParameters.AccountId,
                EndpointIds = revokeParameters.EndpointIds,
                TimeZone = _utcDataService.TimeZone,
                UtcTimestamp = _utcDataService.Now
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
                _signatureService.XAgrirouterSignature(requestBody, privateKey));

            var httpResponseMessage = _httpClient.SendAsync(httpRequestMessage).Result;

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new RevokeException(httpResponseMessage.StatusCode,
                    httpResponseMessage.Content.ReadAsStringAsync().Result);
            }
        }
    }
}