using System;
using System.Collections.Generic;
using System.Net.Http;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Feed.Request;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Response;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Xunit;

namespace Agrirouter.Test.Service.Messaging.Http
{
    /// <summary>
    ///     Functional tests.
    /// </summary>
    [Collection("Integrationtest")]
    public class QueryMessageHeadersServiceTest
    {
        private static readonly HttpClient HttpClient = HttpClientFactory.AuthenticatedHttpClient(OnboardResponse);

        private static OnboardResponse OnboardResponse =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.SingleEndpointWithoutRoute);

        [Fact]
        public void
            GivenExistingEndpointsWhenQueryMessageHeadersWithoutParametersWhenPerformingQueryThenTheMessageShouldNotBeAccepted()
        {
            var queryMessageHeadersService =
                new QueryMessageHeadersService(new HttpMessagingService(HttpClient));
            var queryMessagesParameters = new QueryMessagesParameters
            {
                OnboardResponse = OnboardResponse
            };
            queryMessageHeadersService.Send(queryMessagesParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(400, decodedMessage.ResponseEnvelope.ResponseCode);

            var messages = DecodeMessageService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(messages);
            Assert.NotEmpty(messages.Messages_);
            Assert.Single(messages.Messages_);
            Assert.Equal("VAL_000017", messages.Messages_[0].MessageCode);
            Assert.Equal(
                "Query does not contain any filtering criteria: messageIds, senders or validityPeriod. Information required to process message is missing or malformed.",
                messages.Messages_[0].Message_);
        }

        [Fact]
        public void
            GivenExistingEndpointsWhenQueryMessageHeadersWithUnknownMessageIdsMessageIdsThenTheResultShouldBeAnEmptySetOfMessages()
        {
            var queryMessageHeadersService =
                new QueryMessageHeadersService(new HttpMessagingService(HttpClient));
            var queryMessagesParameters = new QueryMessagesParameters
            {
                OnboardResponse = OnboardResponse,
                MessageIds = new List<string> {Guid.NewGuid().ToString()}
            };
            queryMessageHeadersService.Send(queryMessagesParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(204, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.AckForFeedHeaderList,
                decodedMessage.ResponseEnvelope.Type);
        }

        [Fact]
        public void
            GivenExistingEndpointsWhenQueryMessageHeadersWithUnknownMessageIdsSenderIdsThenTheResultShouldBeAnEmptySetOfMessages()
        {
            var queryMessageHeadersService =
                new QueryMessageHeadersService(new HttpMessagingService(HttpClient));
            var queryMessagesParameters = new QueryMessagesParameters
            {
                OnboardResponse = OnboardResponse,
                Senders = new List<string> {Guid.NewGuid().ToString()}
            };
            queryMessageHeadersService.Send(queryMessagesParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(204, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.AckForFeedHeaderList,
                decodedMessage.ResponseEnvelope.Type);
        }

        [Fact]
        public void
            GivenExistingEndpointsWhenQueryMessageHeadersWithValidityPeriodThenTheResultShouldBeAnEmptySetOfMessages()
        {
            var queryMessageHeadersService =
                new QueryMessageHeadersService(new HttpMessagingService(HttpClient));
            var queryMessagesParameters = new QueryMessagesParameters
            {
                OnboardResponse = OnboardResponse,
                ValidityPeriod = new ValidityPeriod()
            };
            queryMessagesParameters.ValidityPeriod.SentFrom = UtcDataService.Timestamp(TimestampOffset.FourWeeks);
            queryMessagesParameters.ValidityPeriod.SentTo = UtcDataService.Timestamp(TimestampOffset.None);
            queryMessageHeadersService.Send(queryMessagesParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(204, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.AckForFeedHeaderList,
                decodedMessage.ResponseEnvelope.Type);
        }
    }
}