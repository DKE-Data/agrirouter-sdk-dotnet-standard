using System;
using System.Collections.Generic;
using System.Net.Http;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Test.Data;
using Agrirouter.Api.Test.Helper;
using Agrirouter.Impl.Service.onboard;
using Xunit;

namespace Agrirouter.Api.Test.Service.Onboard
{
    /// <summary>
    ///     Functional tests.
    /// </summary>
    [Collection("Integrationtest")]
    public class RevokeServiceTestForFarmingSoftware : AbstractSecuredIntegrationTestForFarmingSoftware
    {
        private static readonly HttpClient HttpClient = HttpClientFactory.HttpClient();

        private static string AccountId => "add3db16-0972-4bce-abab-2c233becbd86";

        [Fact(Skip = "Will only run if there is an endpoint with the given endpoint ID.")]
        public void GivenExistingEndpointWhenRevokingThenTheEndpointShouldBeRevoked()
        {
            var revokeParameters = new RevokeParameters
            {
                AccountId = AccountId,
                EndpointIds = new List<string> {"72c57d24-7d38-4607-b9f3-c3afbe8473db"},
                ApplicationId = Applications.FarmingSoftware.ApplicationId
            };

            var revokeService = new RevokeService(Environment, HttpClient);
            revokeService.Revoke(revokeParameters, Applications.FarmingSoftware.PrivateKey);
        }

        [Fact]
        public void GivenNonExistingEndpointWhenRevokingThenTheEndpointShouldBeRevoked()
        {
            var revokeParameters = new RevokeParameters
            {
                AccountId = AccountId,
                EndpointIds = new List<string> {Guid.NewGuid().ToString()},
                ApplicationId = Applications.FarmingSoftware.ApplicationId
            };

            var revokeService = new RevokeService(Environment, HttpClient);
            Assert.Throws<RevokeException>(() =>
                revokeService.Revoke(revokeParameters, Applications.FarmingSoftware.PrivateKey));
        }
    }
}