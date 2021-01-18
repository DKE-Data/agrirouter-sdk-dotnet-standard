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
using Xunit;

namespace Agrirouter.Api.Test.Service.Messaging.Http
{
    /// <summary>
    ///     Functional tests.
    /// </summary>
    [Collection("Integrationtest")]
    public class PublishAndSendDirectMessageServiceTest : AbstractIntegrationTestForCommunicationUnits
    {
        private static readonly HttpClient HttpClientForSender = HttpClientFactory.AuthenticatedHttpClient(Sender);

        private static OnboardResponse Sender =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Sender);

        private static OnboardResponse Recipient =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Recipient);

        [Fact]
        public void
            GivenValidMessageContentWhenPublishingAndSendingMessageToSingleRecipientThenTheMessageShouldBeDelivered()
        {
            // Description of the messaging process.

            // 1. Set all capabilities for each endpoint - this is done once, not each time.
            // Done once before the test.

            // 2. Recipient has to create his subscriptions in order to get the messages. If they are not set correctly the AR will return a HTTP 400.
            // Done once before the test.

            // 3. Set routes within the UI - this is done once, not each time.
            // Done manually, not API interaction necessary.

            // 4. Publish message from sender to recipient.
            var publishAndSendMessageService =
                new PublishAndSendMessageService(new HttpMessagingService(HttpClientForSender));
            var sendMessageParameters = new SendMessageParameters
            {
                OnboardResponse = Sender,
                ApplicationMessageId = MessageIdService.ApplicationMessageId(),
                TechnicalMessageType = TechnicalMessageTypes.ImgPng,
                Recipients = new List<string> {Recipient.SensorAlternateId},
                Base64MessageContent = DataProvider.ReadBase64EncodedImage()
            };
            publishAndSendMessageService.Send(sendMessageParameters);

            // 5. Let the AR handle the message - this can take up to multiple seconds before receiving the ACK.
            Thread.Sleep(TimeSpan.FromSeconds(5));

            // 6. Fetch and analyze the ACK from the AR.
            var fetchMessageService = new FetchMessageService(HttpClientForSender);
            var fetch = fetchMessageService.Fetch(Sender);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }
    }
}