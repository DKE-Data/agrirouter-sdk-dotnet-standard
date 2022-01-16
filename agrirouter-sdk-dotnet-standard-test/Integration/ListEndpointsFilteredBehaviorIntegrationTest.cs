using System.Linq;
using System.Net.Http;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Request.Payload.Account;
using Agrirouter.Response;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Agrirouter.Test.Service;
using Xunit;
using static Agrirouter.Test.Data.OnboardResponseIntegrationService;

namespace Agrirouter.Test.Integration
{
    public class ListEndpointsFilteredBehaviorIntegrationTest : AbstractIntegrationTestForCommunicationUnits
    {
        private static readonly HttpClient HttpClient = HttpClientFactory.AuthenticatedHttpClient(OnboardResponse);

        private static OnboardResponse OnboardResponse =>
            Read(Identifier.Http.CommunicationUnit.SenderWithMultipleRecipients);

        /**
         * Searching for all endpoints that are connected via route and are able to SEND OR RECEIVE.
         */
        [Fact]
        public void
            GivenMultipleRecipientsWhenFilteringForEndpointsThatCanSendOrReceiveThenTheResultSetShouldContainTheExpectedOnes()
        {
            var listEndpointsService =
                new ListEndpointsService(new HttpMessagingService(HttpClient));
            var listEndpointsParameters = new ListEndpointsParameters
            {
                OnboardResponse = OnboardResponse,
                Direction = ListEndpointsQuery.Types.Direction.SendReceive,
                TechnicalMessageType = TechnicalMessageTypes.Iso11783TaskdataZip
            };
            listEndpointsService.Send(listEndpointsParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.EndpointsListing,
                decodedMessage.ResponseEnvelope.Type);

            var listEndpointsResponse = listEndpointsService.Decode(decodedMessage.ResponsePayloadWrapper.Details);

            // Asserting only that the number of endpoints is at least those three that are expected when searching for SEND OR RECEIVE.
            Assert.True(listEndpointsResponse.Endpoints.Count >= 3);

            var endpoints = listEndpointsResponse.Endpoints
                .Where(endpoint =>
                    endpoint.EndpointId.Equals(Read(Identifier.Http.CommunicationUnit.RecipientWithEnabledPushMessages)
                        .SensorAlternateId) ||
                    endpoint.EndpointId.Equals(Read(Identifier.Http.CommunicationUnit.Recipient).SensorAlternateId))
                .ToList();

            Assert.Equal(2, endpoints.Count);
        }

        /**
         * Searching for all endpoints that are connected via route and are able to SEND.
         */
        [Fact]
        public void
            GivenMultipleRecipientsWhenFilteringForEndpointsThatCanSendThenTheResultSetShouldContainTheExpectedOnes()
        {
            var listEndpointsService =
                new ListEndpointsService(new HttpMessagingService(HttpClient));
            var listEndpointsParameters = new ListEndpointsParameters
            {
                OnboardResponse = OnboardResponse,
                Direction = ListEndpointsQuery.Types.Direction.Send,
                TechnicalMessageType = TechnicalMessageTypes.Iso11783TaskdataZip
            };
            listEndpointsService.Send(listEndpointsParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.EndpointsListing,
                decodedMessage.ResponseEnvelope.Type);

            var listEndpointsResponse = listEndpointsService.Decode(decodedMessage.ResponsePayloadWrapper.Details);

            // Asserting only that the number of endpoints is at least those three that are expected when searching for SEND OR RECEIVE.
            Assert.True(listEndpointsResponse.Endpoints.Count >= 2);

            var endpoints = listEndpointsResponse.Endpoints
                .Where(endpoint =>
                    endpoint.EndpointId.Equals(Read(Identifier.Http.CommunicationUnit.RecipientWithEnabledPushMessages)
                        .SensorAlternateId) ||
                    endpoint.EndpointId.Equals(Read(Identifier.Http.CommunicationUnit.Recipient).SensorAlternateId))
                .ToList();

            Assert.Equal(2, endpoints.Count);
        }

        /**
         * Searching for all endpoints that are connected via route and are able to RECEIVE.
         */
        [Fact]
        public void
            GivenMultipleRecipientsWhenFilteringForEndpointsThatCanReceiveThenTheResultSetShouldContainTheExpectedOnes()
        {
            var listEndpointsService =
                new ListEndpointsService(new HttpMessagingService(HttpClient));
            var listEndpointsParameters = new ListEndpointsParameters
            {
                OnboardResponse = OnboardResponse,
                Direction = ListEndpointsQuery.Types.Direction.Send,
                TechnicalMessageType = TechnicalMessageTypes.Iso11783TaskdataZip
            };
            listEndpointsService.Send(listEndpointsParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.EndpointsListing,
                decodedMessage.ResponseEnvelope.Type);

            var listEndpointsResponse = listEndpointsService.Decode(decodedMessage.ResponsePayloadWrapper.Details);

            // Asserting only that the number of endpoints is at least those two that are expected when searching for SEND OR RECEIVE.
            Assert.True(listEndpointsResponse.Endpoints.Count >= 2);

            var endpoints = listEndpointsResponse.Endpoints
                .Where(endpoint =>
                    endpoint.EndpointId.Equals(Read(Identifier.Http.CommunicationUnit.RecipientWithEnabledPushMessages)
                        .SensorAlternateId) ||
                    endpoint.EndpointId.Equals(Read(Identifier.Http.CommunicationUnit.Recipient).SensorAlternateId))
                .ToList();

            Assert.Equal(2, endpoints.Count);
        }
    }
}