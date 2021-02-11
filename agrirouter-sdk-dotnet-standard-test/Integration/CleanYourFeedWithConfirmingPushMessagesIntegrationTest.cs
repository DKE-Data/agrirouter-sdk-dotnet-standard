using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Response;
using Agrirouter.Sdk.Api.Definitions;
using Agrirouter.Sdk.Api.Dto.Onboard;
using Agrirouter.Sdk.Api.Service.Parameters;
using Agrirouter.Sdk.Impl.Service.Common;
using Agrirouter.Sdk.Impl.Service.Convenience;
using Agrirouter.Sdk.Impl.Service.Messaging;
using Agrirouter.Sdk.Test.Data;
using Agrirouter.Sdk.Test.Helper;
using Agrirouter.Sdk.Test.Service;
using Xunit;

namespace Agrirouter.Sdk.Test.Integration
{
    [Collection("Integrationtest")]
    public class CleanYourFeedWithConfirmingPushMessagesIntegrationTest : AbstractIntegrationTestForCommunicationUnits
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
        ///     1. Enable push notifications for the recipient (done in the setup for the test).
        ///     2. Fetch messages from the message box.
        ///     3. Write the message content to the output folder.
        ///     4. Confirm the message using the message ID to clean the feed.
        ///     5. Let the AR process the message for some seconds to be sure (this depends on the use case and is just an example
        ///     time limit)
        ///     6. Fetch the response from the AR and check.
        /// </summary>
        private static void ActionsForRecipient()
        {
            var fetchMessageService = new FetchMessageService(HttpClientForRecipient);
            var fetch = fetchMessageService.Fetch(Recipient);

            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);

            var pushNotification =
                DecodeMessageService.DecodePushNotification(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(pushNotification);

            var messageId = pushNotification.Messages[0].Header.MessageId;
            var fileName = FilePrefix + pushNotification.Messages[0].Header.MessageId + ".png";
            var image = Encode.FromMessageContent(pushNotification.Messages[0].Content);
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

        /// <summary>
        /// Remove unconfirmed messages - perhaps there are any. If there are none, fine.
        /// Otherwise all will be removed to have a clean scenario.
        /// </summary>
        private static void RemoveUnconfirmedMessagesFromTheFeed()
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
            if (200 == decodedMessage.ResponseEnvelope.ResponseCode)
            {
                Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
                Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.AckForFeedHeaderList,
                    decodedMessage.ResponseEnvelope.Type);

                var feedMessageHeaderQuery =
                    queryMessageHeadersService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
                Assert.True(feedMessageHeaderQuery.QueryMetrics.TotalMessagesInQuery > 0,
                    "There has to be at least one message in the query.");

                var messageIds =
                    (from feed in feedMessageHeaderQuery.Feed
                        from feedHeader in feed.Headers
                        select feedHeader.MessageId)
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
        }

        private static OnboardResponse Sender =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Sender);

        private static OnboardResponse Recipient =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.RecipientWithEnabledPushMessages);

        [Fact(DisplayName = "Clean your feed with confirming push messages integration test scenario.")]
        public void Run()
        {
            EnablePushNotificationsForRecipient();
            RemoveUnconfirmedMessagesFromTheFeed();
            ActionsForSender();
            ActionsForRecipient();
        }

        /// <summary>
        /// Enabling push notifications for the recipient.
        /// Not needed if the capabilities are already set and push notifications are enabled.
        /// In this case - to have a full scenario - this is done each time.
        /// </summary>
        private static void EnablePushNotificationsForRecipient()
        {
            var capabilitiesServices =
                new CapabilitiesService(new HttpMessagingService(HttpClientForRecipient));
            var capabilitiesParameters = new CapabilitiesParameters
            {
                OnboardResponse = Recipient,
                ApplicationId = Applications.CommunicationUnit.ApplicationId,
                CertificationVersionId = Applications.CommunicationUnit.CertificationVersionId,
                EnablePushNotifications = CapabilitySpecification.Types.PushNotification.Enabled,
                CapabilityParameters = CapabilitiesHelper.AllCapabilities
            };

            capabilitiesServices.Send(capabilitiesParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(HttpClientForRecipient);
            var fetch = fetchMessageService.Fetch(Recipient);

            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }
    }
}