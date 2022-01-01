using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Response;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Agrirouter.Test.Service;
using Xunit;

namespace Agrirouter.Test.Integration
{
    [Collection("Integrationtest")]
    public class CleanYourFeedWithDeletingMessagesIntegrationTest : AbstractIntegrationTestForCommunicationUnits
    {
        private static readonly HttpClient HttpClientForSender = HttpClientFactory.AuthenticatedHttpClient(Sender);

        private static readonly HttpClient
            HttpClientForRecipient = HttpClientFactory.AuthenticatedHttpClient(Recipient);

        /// <summary>
        ///     The actions for the sender are the following:
        ///     1. Send the message containing the image file.
        ///     2. Let the AR process the message for some seconds to be sure (this depends on the use case and is just an example
        ///     time limit)
        ///     3. Fetch the message response and validate it.
        /// </summary>
        private static void ActionsForSender()
        {
            var sendMessageService =
                new SendDirectMessageService(new HttpMessagingService(HttpClientForSender));
            var sendMessageParameters = new SendMessageParameters
            {
                OnboardResponse = Sender,
                ApplicationMessageId = MessageIdService.ApplicationMessageId(),
                TechnicalMessageType = TechnicalMessageTypes.ImgPng,
                Recipients = new List<string> {Recipient.SensorAlternateId},
                Base64MessageContent = DataProvider.ReadBase64EncodedImage()
            };
            sendMessageService.Send(sendMessageParameters);

            Thread.Sleep(TimeSpan.FromSeconds(2));

            var fetchMessageService = new FetchMessageService(HttpClientForSender);
            var fetch = fetchMessageService.Fetch(Sender);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }

        /// <summary>
        ///     The actions for the recipient are the following:
        ///     1. Query the message headers.
        ///     2. Let the AR process the message for some seconds to be sure (this depends on the use case and is just an example
        ///     time limit)
        ///     3. Fetch the response from the AR and check.
        ///     4. Delete the messages using the message IDs to clean the feed.
        ///     5. Let the AR process the message for some seconds to be sure (this depends on the use case and is just an example
        ///     time limit)
        ///     6. Fetch the response from the AR and check.
        /// </summary>
        private static void ActionsForRecipient()
        {
            var queryMessageHeadersService =
                new QueryMessageHeadersService(new HttpMessagingService(HttpClientForRecipient));
            var queryMessageHeadersParameters = new QueryMessagesParameters
            {
                OnboardResponse = Recipient,
                Senders = new List<string> {Sender.SensorAlternateId}
            };
            queryMessageHeadersService.Send(queryMessageHeadersParameters);

            Thread.Sleep(TimeSpan.FromSeconds(2));

            var fetchMessageService = new FetchMessageService(HttpClientForRecipient);
            var fetch = fetchMessageService.Fetch(Recipient);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.AckForFeedHeaderList,
                decodedMessage.ResponseEnvelope.Type);

            var feedMessageHeaderQuery =
                queryMessageHeadersService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.True(feedMessageHeaderQuery.QueryMetrics.TotalMessagesInQuery > 0,
                "There has to be at least one message in the query.");

            var messageIds =
                (from feed in feedMessageHeaderQuery.Feed from feedHeader in feed.Headers select feedHeader.MessageId)
                .ToList();

            var feedDeleteService =
                new FeedDeleteService(new HttpMessagingService(HttpClientForRecipient));
            var feedDeleteParameters = new FeedDeleteParameters
            {
                OnboardResponse = Recipient,
                MessageIds = messageIds
            };
            feedDeleteService.Send(feedDeleteParameters);

            Thread.Sleep(TimeSpan.FromSeconds(2));

            fetch = fetchMessageService.Fetch(Recipient);
            Assert.Single(fetch);

            decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
        }

        private static OnboardResponse Sender =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Sender);

        private static OnboardResponse Recipient =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Recipient);

        [Fact(DisplayName = "Clean your feed with deleting messages integration test scenario.")]
        public void Run()
        {
            ActionsForSender();
            ActionsForRecipient();
        }
    }
}