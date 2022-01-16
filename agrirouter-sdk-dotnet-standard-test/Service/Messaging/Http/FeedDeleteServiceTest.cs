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
    public class FeedDeleteServiceTest
    {
        private static readonly HttpClient HttpClient = HttpClientFactory.AuthenticatedHttpClient(OnboardResponse);

        private static OnboardResponse OnboardResponse =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.SingleEndpointWithoutRoute);

        [Fact]
        public void
            GivenExistingEndpointsWhenFeedDeleteWithoutParametersWhenPerformingQueryThenTheMessageShouldNotBeOkBecauseFilterCriteriaIsMissing()
        {
            var feedDeleteService = new FeedDeleteService(new HttpMessagingService(HttpClient));
            var feedDeleteParameters = new FeedDeleteParameters
            {
                OnboardResponse = OnboardResponse
            };
            feedDeleteService.Send(feedDeleteParameters);

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
            Assert.Equal("VAL_000018", messages.Messages_[0].MessageCode);
            Assert.Equal(
                "Information required to process message is missing or malformed. Query does not contain any filtering criteria: messageIds, senders or validityPeriod",
                messages.Messages_[0].Message_);
        }

        [Fact]
        public void GivenExistingEndpointsWhenFeedDeleteWithUnknownMessageIdsMessageIdsThenTheResultShouldBeOkButThereAreNoMessagesToDelete()
        {
            var feedDeleteService = new FeedDeleteService(new HttpMessagingService(HttpClient));
            var feedDeleteParameters = new FeedDeleteParameters
            {
                OnboardResponse = OnboardResponse,
                MessageIds = new List<string> { Guid.NewGuid().ToString() }
            };
            feedDeleteService.Send(feedDeleteParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(204, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.AckWithMessages,
                decodedMessage.ResponseEnvelope.Type);

            var messages = DecodeMessageService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(messages);
            Assert.NotEmpty(messages.Messages_);
            Assert.Single(messages.Messages_);
            Assert.Equal("VAL_000208", messages.Messages_[0].MessageCode);
            Assert.Equal(
                "Feed does not contain any data to be deleted.",
                messages.Messages_[0].Message_);
        }

        [Fact]
        public void
            GivenExistingEndpointsWhenFeedDeleteWithSenderIdsThenTheResultShouldNotOkButThereAreNoMessagesToDelete()
        {
            var feedDeleteService = new FeedDeleteService(new HttpMessagingService(HttpClient));
            var feedDeleteParameters = new FeedDeleteParameters
            {
                OnboardResponse = OnboardResponse,
                Senders = new List<string> { Guid.NewGuid().ToString() }
            };
            feedDeleteService.Send(feedDeleteParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(204, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.AckWithMessages,
                decodedMessage.ResponseEnvelope.Type);

            var messages = DecodeMessageService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(messages);
            Assert.NotEmpty(messages.Messages_);
            Assert.Single(messages.Messages_);
            Assert.Equal("VAL_000208", messages.Messages_[0].MessageCode);
            Assert.Equal(
                "Feed does not contain any data to be deleted.",
                messages.Messages_[0].Message_);
        }

        [Fact]
        public void
            GivenExistingEndpointsWhenFeedDeleteWithFullValidityPeriodThenTheResultShouldBeOkButThereAreNoMessagesToDelete()
        {
            var feedDeleteService = new FeedDeleteService(new HttpMessagingService(HttpClient));
            var feedDeleteParameters = new FeedDeleteParameters
            {
                OnboardResponse = OnboardResponse,
                ValidityPeriod = new ValidityPeriod
                {
                    SentFrom = UtcDataService.Timestamp(TimestampOffset.FourWeeks),
                    SentTo = UtcDataService.Timestamp(TimestampOffset.None)
                }
            };
            feedDeleteService.Send(feedDeleteParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(204, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.AckWithMessages,
                decodedMessage.ResponseEnvelope.Type);

            var messages = DecodeMessageService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(messages);
            Assert.NotEmpty(messages.Messages_);
            Assert.Single(messages.Messages_);
            Assert.Equal("VAL_000208", messages.Messages_[0].MessageCode);
            Assert.Equal(
                "Feed does not contain any data to be deleted.",
                messages.Messages_[0].Message_);
        }

    }
}