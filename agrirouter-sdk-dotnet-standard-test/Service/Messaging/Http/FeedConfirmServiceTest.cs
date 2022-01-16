using System.Collections.Generic;
using System.Net.Http;
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
    public class FeedConfirmServiceTest : AbstractIntegrationTestForCommunicationUnits
    {
        private static readonly HttpClient HttpClient = HttpClientFactory.AuthenticatedHttpClient(OnboardResponse);

        private static OnboardResponse OnboardResponse =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.SingleEndpointWithoutRoute);


        [Fact]
        public void GivenEmptyMessageIdsWhenConfirmingMessagesThenTheMessageShouldNotBeAccepted()
        {
            var feedConfirmService =
                new FeedConfirmService(new HttpMessagingService(HttpClient));
            var feedConfirmParameters = new FeedConfirmParameters
            {
                OnboardResponse = OnboardResponse
            };
            feedConfirmService.Send(feedConfirmParameters);

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
                "messageIds information required to process message is missing or malformed.",
                messages.Messages_[0].Message_);
        }

        [Fact]
        public void GivenNonExistingMessageIdsWhenConfirmingMessagesThenTheMessageShouldBeAccepted()
        {
            var feedConfirmService =
                new FeedConfirmService(new HttpMessagingService(HttpClient));
            var feedConfirmParameters = new FeedConfirmParameters
            {
                OnboardResponse = OnboardResponse,
                MessageIds = new List<string>
                    {MessageIdService.ApplicationMessageId(), MessageIdService.ApplicationMessageId()}
            };
            feedConfirmService.Send(feedConfirmParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);

            var messages = DecodeMessageService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(messages);
            Assert.NotEmpty(messages.Messages_);
            Assert.Equal(2, messages.Messages_.Count);
            Assert.Equal("VAL_000205", messages.Messages_[0].MessageCode);
            Assert.Equal("VAL_000205", messages.Messages_[1].MessageCode);
            Assert.Equal(
                "Feed message cannot be found.",
                messages.Messages_[0].Message_);
            Assert.Equal(
                "Feed message cannot be found.",
                messages.Messages_[1].Message_);
        }

        [Fact]
        public void GivenNonExistingMessageIdWhenConfirmingMessagesThenTheMessageShouldBeAccepted()
        {
            var feedConfirmService =
                new FeedConfirmService(new HttpMessagingService(HttpClient));
            var feedConfirmParameters = new FeedConfirmParameters
            {
                OnboardResponse = OnboardResponse,
                MessageIds = new List<string> {MessageIdService.ApplicationMessageId()}
            };
            feedConfirmService.Send(feedConfirmParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);

            var messages = DecodeMessageService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(messages);
            Assert.NotEmpty(messages.Messages_);
            Assert.Single(messages.Messages_);
            Assert.Equal("VAL_000205", messages.Messages_[0].MessageCode);
            Assert.Equal(
                "Feed message cannot be found.",
                messages.Messages_[0].Message_);
        }
    }
}