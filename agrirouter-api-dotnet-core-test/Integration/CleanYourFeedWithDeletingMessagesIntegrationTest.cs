using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Api.test.Data;
using Agrirouter.Api.test.helper;
using Agrirouter.Api.Test.Service;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.messaging;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Response;
using Newtonsoft.Json;
using Xunit;

namespace Agrirouter.Api.test.integration
{
    [Collection("Integrationtest")]
    public class CleanYourFeedWithDeletingMessagesIntegrationTest : AbstractIntegrationTest
    {
        private static readonly HttpClient HttpClientForSender = HttpClientFactory.AuthenticatedHttpClient(Sender);

        private static readonly HttpClient
            HttpClientForRecipient = HttpClientFactory.AuthenticatedHttpClient(Recipient);

        [Fact(DisplayName = "Clean your feed integration test scenario.")]
        public void Run()
        {
            PrepareTestEnvironment();
            ActionsForSender();
            ActionsForRecipient();
        }

        private static void PrepareTestEnvironment()
        {
            PrepareTestEnvironmentForSender();
            PrepareTestEnvironmentForRecipient();
        }

        private static void PrepareTestEnvironmentForSender()
        {
            var capabilitiesServices =
                new CapabilitiesService(new MessagingService(HttpClientForSender), new EncodeMessageService());
            var capabilitiesParameters = new CapabilitiesParameters
            {
                OnboardResponse = Sender,
                ApplicationId = ApplicationId,
                CertificationVersionId = CertificationVersionId,
                EnablePushNotifications = CapabilitySpecification.Types.PushNotification.Disabled,
                CapabilityParameters = new List<CapabilityParameter>()
            };
            var capabilitiesParameter = new CapabilityParameter
            {
                Direction = CapabilitySpecification.Types.Direction.SendReceive,
                TechnicalMessageType = TechnicalMessageTypes.ImgPng
            };
            capabilitiesParameters.CapabilityParameters.Add(capabilitiesParameter);
            capabilitiesServices.Send(capabilitiesParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(HttpClientForSender);
            var fetch = fetchMessageService.Fetch(Sender);
            Assert.Single(fetch);

            var decodeMessageService = new DecodeMessageService();
            var decodedMessage = decodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }

        private static void PrepareTestEnvironmentForRecipient()
        {
            var capabilitiesServices =
                new CapabilitiesService(new MessagingService(HttpClientForRecipient), new EncodeMessageService());
            var capabilitiesParameters = new CapabilitiesParameters
            {
                OnboardResponse = Recipient,
                ApplicationId = ApplicationId,
                CertificationVersionId = CertificationVersionId,
                EnablePushNotifications = CapabilitySpecification.Types.PushNotification.Disabled,
                CapabilityParameters = new List<CapabilityParameter>()
            };
            var capabilitiesParameter = new CapabilityParameter
            {
                Direction = CapabilitySpecification.Types.Direction.SendReceive,
                TechnicalMessageType = TechnicalMessageTypes.ImgPng
            };
            capabilitiesParameters.CapabilityParameters.Add(capabilitiesParameter);
            capabilitiesServices.Send(capabilitiesParameters);

            Thread.Sleep(TimeSpan.FromSeconds(2));

            var fetchMessageService = new FetchMessageService(HttpClientForRecipient);
            var fetch = fetchMessageService.Fetch(Recipient);
            Assert.Single(fetch);

            var decodeMessageService = new DecodeMessageService();
            var decodedMessage = decodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }

        /// <summary>
        /// The actions for the sender are the following:
        ///
        /// 1. Send the message containing the image file.
        /// 2. Let the AR process the message for some seconds to be sure (this depends on the use case and is just an example time limit)
        /// 3. Fetch the message response and validate it.
        /// 
        /// </summary>
        private static void ActionsForSender()
        {
            var sendMessageService =
                new SendDirectMessageService(new MessagingService(HttpClientForSender), new EncodeMessageService());
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
            var decodeMessageService = new DecodeMessageService();
            var decodedMessage = decodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }

        /// <summary>
        ///  The actions for the recipient are the following:
        ///
        /// 1. Query the message headers.
        /// 2. Let the AR process the message for some seconds to be sure (this depends on the use case and is just an example time limit)
        /// 3. Fetch the response from the AR and check.
        ///
        /// 4. Delete the messages using the message IDs to clean the feed.
        /// 5. Let the AR process the message for some seconds to be sure (this depends on the use case and is just an example time limit)
        /// 6. Fetch the response from the AR and check.
        /// 
        /// </summary>
        private void ActionsForRecipient()
        {
            var queryMessageHeadersService =
                new QueryMessageHeadersService(new MessagingService(HttpClientForRecipient),
                    new EncodeMessageService());
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

            var decodeMessageService = new DecodeMessageService();
            var decodedMessage = decodeMessageService.Decode(fetch[0].Command.Message);
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
                new FeedDeleteService(new MessagingService(HttpClientForRecipient), new EncodeMessageService());
            var feedDeleteParameters = new FeedDeleteParameters()
            {
                OnboardResponse = Recipient,
                MessageIds = messageIds
            };
            feedDeleteService.Send(feedDeleteParameters);

            Thread.Sleep(TimeSpan.FromSeconds(2));

            fetch = fetchMessageService.Fetch(Recipient);
            Assert.Single(fetch);

            decodedMessage = decodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
        }

        private static OnboardResponse Sender
        {
            get
            {
                var onboardingResponseAsJson =
                    "{\"deviceAlternateId\":\"44d7003e-2743-492b-86aa-4fed91fcf20a\",\"capabilityAlternateId\":\"c2467f6d-0a7e-48ca-9b57-1862186aef12\",\"sensorAlternateId\":\"bfbfa1a5-bf09-4423-9ad3-3678368ffe53\",\"connectionCriteria\":{\"gatewayId\":\"3\",\"measures\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/measures/44d7003e-2743-492b-86aa-4fed91fcf20a\",\"commands\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/commands/44d7003e-2743-492b-86aa-4fed91fcf20a\"},\"authentication\":{\"type\":\"P12\",\"secret\":\"8A71w6kCuckYiTK4R1k4bCVxsnB6flcK9crZ\",\"certificate\":\"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\"}}";
                var onboardingResponse =
                    JsonConvert.DeserializeObject(onboardingResponseAsJson, typeof(OnboardResponse));
                return onboardingResponse as OnboardResponse;
            }
        }

        private static OnboardResponse Recipient
        {
            get
            {
                var onboardingResponseAsJson =
                    "{\"deviceAlternateId\":\"fc43478e-f26c-4527-9bdd-c1fcb5542dc8\",\"capabilityAlternateId\":\"c2467f6d-0a7e-48ca-9b57-1862186aef12\",\"sensorAlternateId\":\"97c1eb60-b7a6-4ac4-8455-84d7c423e7ad\",\"connectionCriteria\":{\"gatewayId\":\"3\",\"measures\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/measures/fc43478e-f26c-4527-9bdd-c1fcb5542dc8\",\"commands\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/commands/fc43478e-f26c-4527-9bdd-c1fcb5542dc8\"},\"authentication\":{\"type\":\"P12\",\"secret\":\"LQXaUbU1IAzoRKvayZNvdeHVUORUM8sskr3Q\",\"certificate\":\"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\"}}";
                var onboardingResponse =
                    JsonConvert.DeserializeObject(onboardingResponseAsJson, typeof(OnboardResponse));
                return onboardingResponse as OnboardResponse;
            }
        }
    }
}