using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using Agrirouter.Response;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Convenience;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Agrirouter.Test.Service;
using Xunit;

namespace Agrirouter.Test.Integration
{
    [Collection("Integrationtest")]
    public class CleanYourFeedWithConfirmingMessagesIntegrationTest : AbstractIntegrationTestForCommunicationUnits
    {
        private static readonly HttpClient HttpClientForSender = HttpClientFactory.AuthenticatedHttpClient(Sender);
        private const string FilePrefix = "message-content-";

        private static readonly HttpClient
            HttpClientForRecipient = HttpClientFactory.AuthenticatedHttpClient(Recipient);

        /// <summary>
        ///     The actions for the sender are the following:
        ///     1. Send the message containing the image file from the input folder.
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
        ///     4. Query the message with that message ID and check the results.
        ///     5. Let the AR process the message for some seconds to be sure (this depends on the use case and is just an example
        ///     time limit)
        ///     6. Fetch the response from the AR and check.
        ///     7. Write the message content to the output folder.
        ///     8. Confirm the message using the message ID to clean the feed.
        ///     9. Let the AR process the message for some seconds to be sure (this depends on the use case and is just an example
        ///     time limit)
        ///     10. Fetch the response from the AR and check.
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

            var messageId = feedMessageHeaderQuery.Feed[0].Headers[0].MessageId;

            var queryMessagesService =
                new QueryMessagesService(new HttpMessagingService(HttpClientForRecipient));
            var queryMessagesParameters = new QueryMessagesParameters
            {
                OnboardResponse = Recipient,
                MessageIds = new List<string> {messageId}
            };
            queryMessagesService.Send(queryMessagesParameters);

            Thread.Sleep(TimeSpan.FromSeconds(2));

            fetch = fetchMessageService.Fetch(Recipient);
            Assert.Single(fetch);

            decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.AckForFeedMessage,
                decodedMessage.ResponseEnvelope.Type);

            var feedMessage = queryMessagesService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.Equal(1, feedMessage.QueryMetrics.TotalMessagesInQuery);

            var fileName = FilePrefix + feedMessage.Messages[0].Header.MessageId + ".png";
            var image = Encode.FromMessageContent(feedMessage.Messages[0].Content);
            File.WriteAllBytes(fileName, image);

            var feedConfirmService =
                new FeedConfirmService(new HttpMessagingService(HttpClientForRecipient));
            var feedConfirmParameters = new FeedConfirmParameters
            {
                OnboardResponse = Recipient,
                MessageIds = new List<string> {messageId}
            };
            feedConfirmService.Send(feedConfirmParameters);

            Thread.Sleep(TimeSpan.FromSeconds(2));

            fetch = fetchMessageService.Fetch(Recipient);
            Assert.Single(fetch);

            decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);

            var messages = DecodeMessageService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(messages);
            Assert.NotEmpty(messages.Messages_);
            Assert.Single(messages.Messages_);
            Assert.Equal("VAL_000206", messages.Messages_[0].MessageCode);
            Assert.Equal(
                "Feed message confirmation confirmed.",
                messages.Messages_[0].Message_);
        }

        private static OnboardResponse Sender =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Sender);

        private static OnboardResponse Recipient =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Recipient);

        [Fact(DisplayName = "Clean your feed with confirming messages integration test scenario.")]
        public void Run()
        {
            ActionsForSender();
            ActionsForRecipient();
        }
    }
}