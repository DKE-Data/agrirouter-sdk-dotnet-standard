using System;
using System.Collections.Generic;
using System.Net.Http;
using com.dke.data.agrirouter.api.exception;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.api.test.helper;
using com.dke.data.agrirouter.impl.service.common;
using com.dke.data.agrirouter.impl.service.onboard;
using Xunit;

namespace com.dke.data.agrirouter.api.test.service.onboard
{
    /// <summary>
    /// Functional tests.
    /// </summary>
    public class RevokeServiceTest : AbstractSecuredIntegrationTest
    {
        private static readonly UtcDataService UtcDataService = new UtcDataService();
        private static readonly SignatureService SignatureService = new SignatureService();
        private static readonly HttpClient HttpClient = HttpClientFactory.HttpClient();

        [Fact(Skip = "Will only run if there is an endpoint with the given endpoint ID.")]
        public void GivenExistingEndpointWhenRevokingThenTheEndpointShouldBeRevoked()
        {
            var revokeParameters = new RevokeParameters
            {
                AccountId = AccountId,
                EndpointIds = new List<string> {EndpointId},
                ApplicationId = ApplicationId
            };

            var revokeService = new RevokeService(Environment, UtcDataService, SignatureService, HttpClient);
            revokeService.Revoke(revokeParameters, PrivateKey);
        }

        [Fact]
        public void GivenNonExistingEndpointWhenRevokingThenTheEndpointShouldBeRevoked()
        {
            var revokeParameters = new RevokeParameters
            {
                AccountId = AccountId,
                EndpointIds = new List<string> {Guid.NewGuid().ToString()},
                ApplicationId = ApplicationId
            };

            var revokeService = new RevokeService(Environment, UtcDataService, SignatureService, HttpClient);
            Assert.Throws<RevokeException>(() => revokeService.Revoke(revokeParameters, PrivateKey));
        }

        private static string AccountId => "5d47a537-9455-410d-aa6d-fbd69a5cf990";

        private static string ApplicationId => "16b1c3ab-55ef-412c-952b-f280424272e1";

        private static string EndpointId => "72c57d24-7d38-4607-b9f3-c3afbe8473db";
    }
}