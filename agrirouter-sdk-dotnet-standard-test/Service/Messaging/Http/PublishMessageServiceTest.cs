using System.Collections.Generic;
using System.Net.Http;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Xunit;

namespace Agrirouter.Test.Service.Messaging.Http
{
    /// <summary>
    ///     Functional tests.
    /// </summary>
    [Collection("Integrationtest")]
    public class PublishMessageServiceTest : AbstractIntegrationTestForCommunicationUnits
    {
        private static readonly HttpClient HttpClientForSender = HttpClientFactory.AuthenticatedHttpClient(Sender);

        private static readonly HttpClient
            HttpClientForRecipient = HttpClientFactory.AuthenticatedHttpClient(Recipient);

        private static OnboardResponse Sender =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Sender);

        private static OnboardResponse Recipient =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Recipient);

        [Fact]
        public void GivenValidMessageContentWhenPublishingMessageThenTheMessageShouldBeDelivered()
        {
            // Description of the messaging process.

            // 1. Set all capabilities for each endpoint - this is done once, not each time.
            // Done once before the test.

            // 2. Recipient has to create his subscriptions in order to get the messages. If they are not set correctly the AR will return a HTTP 400.
            var subscriptionService =
                new SubscriptionService(new HttpMessagingService(HttpClientForRecipient));
            var subscriptionParameters = new SubscriptionParameters
            {
                OnboardResponse = Recipient,
                TechnicalMessageTypes = new List<Subscription.Types.MessageTypeSubscriptionItem>()
            };
            var technicalMessageType = new Subscription.Types.MessageTypeSubscriptionItem
            {
                TechnicalMessageType = TechnicalMessageTypes.ImgPng
            };
            subscriptionParameters.TechnicalMessageTypes.Add(technicalMessageType);
            subscriptionService.Send(subscriptionParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClientForRecipient);
            var fetch = fetchMessageService.Fetch(Recipient);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);

            // 3. Set routes within the UI - this is done once, not each time.
            // Done manually, not API interaction necessary.

            // 4. Publish message from sender to recipient.
            var publishMessageService =
                new PublishMessageService(new HttpMessagingService(HttpClientForSender));
            var sendMessageParameters = new SendMessageParameters
            {
                OnboardResponse = Sender,
                ApplicationMessageId = MessageIdService.ApplicationMessageId(),
                TechnicalMessageType = TechnicalMessageTypes.ImgPng,
                Base64MessageContent = DataProvider.ReadBase64EncodedImage()
            };
            publishMessageService.Send(sendMessageParameters);

            // 5. Let the AR handle the message - this can take up to multiple seconds before receiving the ACK.
            Timer.WaitForTheAgrirouterToProcessTheMessage();

            // 6. Fetch and analyze the ACK from the AR.
            fetchMessageService = new FetchMessageService(HttpClientForSender);
            fetch = fetchMessageService.Fetch(Sender);
            Assert.Single(fetch);

            decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }
    }
}