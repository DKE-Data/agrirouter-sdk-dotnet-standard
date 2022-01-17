using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Impl.Service.Messaging.CancellationToken;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Xunit;

namespace Agrirouter.Test.Service.Messaging.Http
{
    /// <summary>
    ///     Functional tests.
    /// </summary>
    [Collection("Integrationtest")]
    public class CancellationTokenTest : AbstractIntegrationTestForCommunicationUnits
    {
        private static readonly HttpClient HttpClient = HttpClientFactory.AuthenticatedHttpClient(OnboardResponse);

        private static OnboardResponse OnboardResponse =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.SingleEndpointWithP12Certificate);

        [Fact(Timeout = 1600)]
        public void
            GivenDefaultCancellationTokenWhenFetchingMessagesTheCancellationTokenShouldBeCancelledAfterCertainRetries()
        {
            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse, new DefaultCancellationToken(3, 500));
            Assert.Empty(fetch);
        }
    }
}