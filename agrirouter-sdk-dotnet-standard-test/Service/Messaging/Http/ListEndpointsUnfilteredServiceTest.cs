using System;
using System.Net.Http;
using System.Threading;
using Agrirouter.Request.Payload.Account;
using Agrirouter.Response;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Xunit;

namespace Agrirouter.Test.Service.Messaging.Http
{
    /// <summary>
    ///     Functional tests.
    /// </summary>
    [Collection("Integrationtest")]
    public class ListEndpointsUnfilteredServiceTest : AbstractIntegrationTestForCommunicationUnits
    {
        private static readonly HttpClient HttpClient = HttpClientFactory.AuthenticatedHttpClient(OnboardResponse);

        private static OnboardResponse OnboardResponse =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.SingleEndpointWithoutRoute);

        [Fact]
        public void
            GivenExistingEndpointsWhenListEndpointsIsExecutedWithDirectionAndTechnicalMessageTypeThenTheMessageShouldReturnAValidResult()
        {
            var listEndpointsService =
                new ListEndpointsUnfilteredService(new HttpMessagingService(HttpClient));
            var listEndpointsParameters = new ListEndpointsParameters
            {
                OnboardResponse = OnboardResponse,
                Direction = ListEndpointsQuery.Types.Direction.SendReceive,
                TechnicalMessageType = TechnicalMessageTypes.Empty
            };
            listEndpointsService.Send(listEndpointsParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.EndpointsListing,
                decodedMessage.ResponseEnvelope.Type);
        }

        [Fact]
        public void
            GivenExistingEndpointsWhenListEndpointsIsExecutedWithDirectionReceiveAndEmptyTechnicalMessageTypeThenTheMessageShouldReturnAValidResult()
        {
            var listEndpointsService =
                new ListEndpointsUnfilteredService(new HttpMessagingService(HttpClient));
            var listEndpointsParameters = new ListEndpointsParameters
            {
                OnboardResponse = OnboardResponse,
                Direction = ListEndpointsQuery.Types.Direction.Receive
            };
            listEndpointsService.Send(listEndpointsParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.EndpointsListing,
                decodedMessage.ResponseEnvelope.Type);
        }

        [Fact]
        public void
            GivenExistingEndpointsWhenListEndpointsIsExecutedWithDirectionSendAndEmptyTechnicalMessageTypeThenTheMessageShouldReturnAValidResult()
        {
            var listEndpointsService =
                new ListEndpointsUnfilteredService(new HttpMessagingService(HttpClient));
            var listEndpointsParameters = new ListEndpointsParameters
            {
                OnboardResponse = OnboardResponse,
                Direction = ListEndpointsQuery.Types.Direction.Send
            };
            listEndpointsService.Send(listEndpointsParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.EndpointsListing,
                decodedMessage.ResponseEnvelope.Type);
        }

        [Fact]
        public void
            GivenExistingEndpointsWhenListEndpointsIsExecutedWithDirectionSendReceiveAndEmptyTechnicalMessageTypeThenTheMessageShouldReturnAValidResult()
        {
            var listEndpointsService =
                new ListEndpointsUnfilteredService(new HttpMessagingService(HttpClient));
            var listEndpointsParameters = new ListEndpointsParameters
            {
                OnboardResponse = OnboardResponse,
                Direction = ListEndpointsQuery.Types.Direction.SendReceive
            };
            listEndpointsService.Send(listEndpointsParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.EndpointsListing,
                decodedMessage.ResponseEnvelope.Type);
        }

        [Fact]
        public void
            GivenExistingEndpointsWhenListEndpointsIsExecutedWithEmptyDirectionAndEmptyAsTechnicalMessageTypeThenTheMessageShouldReturnAValidResult()
        {
            var listEndpointsService =
                new ListEndpointsUnfilteredService(new HttpMessagingService(HttpClient));
            var listEndpointsParameters = new ListEndpointsParameters
            {
                OnboardResponse = OnboardResponse,
                TechnicalMessageType = TechnicalMessageTypes.Empty
            };
            listEndpointsService.Send(listEndpointsParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.EndpointsListing,
                decodedMessage.ResponseEnvelope.Type);
        }

        [Fact]
        public void
            GivenExistingEndpointsWhenListEndpointsIsExecutedWithEmptyDirectionAndTechnicalMessageTypeThenTheMessageShouldReturnAValidResult()
        {
            var listEndpointsService =
                new ListEndpointsUnfilteredService(new HttpMessagingService(HttpClient));
            var listEndpointsParameters = new ListEndpointsParameters
            {
                OnboardResponse = OnboardResponse
            };
            listEndpointsService.Send(listEndpointsParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.EndpointsListing,
                decodedMessage.ResponseEnvelope.Type);
        }
    }
}