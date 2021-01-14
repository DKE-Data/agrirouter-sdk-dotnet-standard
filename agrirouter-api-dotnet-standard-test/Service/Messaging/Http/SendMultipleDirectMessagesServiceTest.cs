using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Api.Test.Data;
using Agrirouter.Api.Test.Helper;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Newtonsoft.Json;
using Xunit;

namespace Agrirouter.Api.Test.Service.Messaging.Http
{
    /// <summary>
    ///     Functional tests.
    /// </summary>
    [Collection("Integrationtest")]
    public class SendMultipleDirectMessagesServiceTest : AbstractIntegrationTestForCommunicationUnits
    {
        private static readonly HttpClient HttpClientForSender = HttpClientFactory.AuthenticatedHttpClient(Sender);

        private static OnboardResponse Sender =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Sender);

        private static OnboardResponse Recipient =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Recipient);

        [Fact]
        public void GivenMultipleValidMessageContentWhenSendingMessageToSingleRecipientThenTheMessageShouldBeDelivered()
        {
            var sendMessageService =
                new SendMultipleDirectMessagesService(new HttpMessagingService(HttpClientForSender));
            var sendMessageParameters = new SendMultipleMessagesParameters
            {
                OnboardResponse = Sender,
                ApplicationMessageId = MessageIdService.ApplicationMessageId(),
                MultipleMessageEntries = new List<MultipleMessageEntry>
                {
                    new MultipleMessageEntry
                    {
                        ApplicationMessageId = MessageIdService.ApplicationMessageId(),
                        TechnicalMessageType = TechnicalMessageTypes.ImgPng,
                        Recipients = new List<string> {Recipient.SensorAlternateId},
                        Base64MessageContent = DataProvider.ReadBase64EncodedImage()
                    },
                    new MultipleMessageEntry
                    {
                        ApplicationMessageId = MessageIdService.ApplicationMessageId(),
                        TechnicalMessageType = TechnicalMessageTypes.ImgPng,
                        Recipients = new List<string> {Recipient.SensorAlternateId},
                        Base64MessageContent = DataProvider.ReadBase64EncodedImage()
                    }
                }
            };
            sendMessageService.Send(sendMessageParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(HttpClientForSender);
            var fetch = fetchMessageService.Fetch(Sender);
            Assert.Equal(2, fetch.Count);

            Assert.Equal(201, DecodeMessageService.Decode(fetch[0].Command.Message).ResponseEnvelope.ResponseCode);
            Assert.Equal(201, DecodeMessageService.Decode(fetch[1].Command.Message).ResponseEnvelope.ResponseCode);
        }
    }
}