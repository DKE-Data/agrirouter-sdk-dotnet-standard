using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Test.Data;
using Agrirouter.Api.Test.Helper;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Response;
using Newtonsoft.Json;
using Xunit;

namespace Agrirouter.Api.Test.Service.Messaging.Http
{
    /// <summary>
    ///     Functional tests.
    /// </summary>
    [Collection("Integrationtest")]
    public class SubscriptionServiceTest : AbstractIntegrationTestForCommunicationUnits
    {
        private static readonly HttpClient HttpClient = HttpClientFactory.AuthenticatedHttpClient(OnboardResponse);

        private static OnboardResponse OnboardResponse => OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.SingleEndpointWithoutRoute);

        [Fact]
        public void GivenEmptySubscriptionWhenSendingSubscriptionMessageThenTheMessageShouldBeAccepted()
        {
            var subscriptionService =
                new SubscriptionService(new HttpMessagingService(HttpClient));
            var subscriptionParameters = new SubscriptionParameters
            {
                OnboardResponse = OnboardResponse
            };

            subscriptionService.Send(subscriptionParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.Ack,
                decodedMessage.ResponseEnvelope.Type);
        }

        [Fact]
        public void
            GivenMultipleSubscriptionEntriesWithOneInvalidTechnicalMessageTypeWhenSendingSubscriptionMessageThenTheMessageShouldBeNotBeAccepted()
        {
            var subscriptionService =
                new SubscriptionService(new HttpMessagingService(HttpClient));
            var subscriptionParameters = new SubscriptionParameters
            {
                OnboardResponse = OnboardResponse,
                TechnicalMessageTypes = new List<Subscription.Types.MessageTypeSubscriptionItem>()
            };
            var technicalMessageTypeForTaskdata = new Subscription.Types.MessageTypeSubscriptionItem
            {
                TechnicalMessageType = TechnicalMessageTypes.ImgPng
            };
            subscriptionParameters.TechnicalMessageTypes.Add(technicalMessageTypeForTaskdata);

            var technicalMessageTypeForProtobuf = new Subscription.Types.MessageTypeSubscriptionItem
            {
                TechnicalMessageType = "non:existent"
            };
            subscriptionParameters.TechnicalMessageTypes.Add(technicalMessageTypeForProtobuf);
            subscriptionService.Send(subscriptionParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(400, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.AckWithFailure,
                decodedMessage.ResponseEnvelope.Type);

            var messages = DecodeMessageService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(messages);
            Assert.NotEmpty(messages.Messages_);
            Assert.Single(messages.Messages_);
            Assert.Equal("VAL_000006", messages.Messages_[0].MessageCode);
            Assert.Equal(
                "Subscription to \"non:existent\" is not valid per reported capabilities.",
                messages.Messages_[0].Message_);
        }

        [Fact]
        public void GivenSingleSubscriptionEntryWhenSendingSubscriptionMessageThenTheMessageShouldBeAccepted()
        {
            var subscriptionService =
                new SubscriptionService(new HttpMessagingService(HttpClient));
            var subscriptionParameters = new SubscriptionParameters
            {
                OnboardResponse = OnboardResponse,
                TechnicalMessageTypes = new List<Subscription.Types.MessageTypeSubscriptionItem>()
            };
            var technicalMessageType = new Subscription.Types.MessageTypeSubscriptionItem
            {
                TechnicalMessageType = TechnicalMessageTypes.ImgPng
            };
            subscriptionParameters.TechnicalMessageTypes.Add(technicalMessageType);
            subscriptionService.Send(subscriptionParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.Ack,
                decodedMessage.ResponseEnvelope.Type);
        }
    }
}