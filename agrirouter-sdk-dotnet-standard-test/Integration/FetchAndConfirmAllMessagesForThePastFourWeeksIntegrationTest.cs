using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Feed.Request;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Convenience;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Response;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Agrirouter.Test.Service;
using Xunit;

namespace Agrirouter.Test.Integration
{
    [Collection("Integrationtest")]
    public class
        FetchAndConfirmAllMessagesForThePastFourWeeksIntegrationTest : AbstractIntegrationTestForCommunicationUnits
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

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClientForSender);
            var fetch = fetchMessageService.Fetch(Sender);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }

        /// <summary>
        ///     The actions for the recipient are the following:
        ///     1. Query all the message of the past 4 weeks (which is the maximum interval.
        ///     2. Let the AR process the message for some seconds to be sure (this depends on the use case and is just an example
        ///     time limit)
        ///     3. Fetch the response from the AR and check.
        ///     4. Write the message content to the output folder.
        ///     5. Confirm the message using the message ID to clean the feed.
        ///     6. Let the AR process the message for some seconds to be sure (this depends on the use case and is just an example
        ///     time limit)
        ///     7. Fetch the response from the AR and check.
        /// </summary>
        private static void ActionsForRecipient()
        {
            var queryMessagesService =
                new QueryMessagesService(new HttpMessagingService(HttpClientForRecipient));
            var queryMessagesParameters = new QueryMessagesParameters
            {
                OnboardResponse = Recipient,
                ValidityPeriod = new ValidityPeriod()
            };
            queryMessagesParameters.ValidityPeriod.SentFrom = UtcDataService.Timestamp(TimestampOffset.FourWeeks);
            queryMessagesParameters.ValidityPeriod.SentTo = UtcDataService.Timestamp(TimestampOffset.None);
            queryMessagesService.Send(queryMessagesParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClientForRecipient);
            var fetch = fetchMessageService.Fetch(Recipient);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.AckForFeedMessage,
                decodedMessage.ResponseEnvelope.Type);

            var feedMessage = queryMessagesService.Decode(decodedMessage.ResponsePayloadWrapper.Details);

            var fileName = FilePrefix + feedMessage.Messages[0].Header.MessageId + ".png";
            var image = Encode.FromMessageContent(feedMessage.Messages[0].Content);
            File.WriteAllBytes(fileName, image);

            var feedConfirmService =
                new FeedConfirmService(new HttpMessagingService(HttpClientForRecipient));
            var feedConfirmParameters = new FeedConfirmParameters
            {
                OnboardResponse = Recipient,
                MessageIds = feedMessage.Messages.Select(message => message.Header.MessageId).ToList()
            };
            feedConfirmService.Send(feedConfirmParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            fetch = fetchMessageService.Fetch(Recipient);
            Assert.Single(fetch);

            decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);

            var messages = DecodeMessageService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(messages);
            Assert.NotEmpty(messages.Messages_);
            foreach (var message in messages.Messages_)
            {
                Assert.Equal("VAL_000206", message.MessageCode);
                Assert.Equal(
                    "Feed message confirmation confirmed.",
                    message.Message_);
            }
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