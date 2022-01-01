using System.Collections.Generic;
using System.Net.Http;
using Agrirouter.Api.Definitions;
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
    /// Functional tests.
    /// </summary>
    public class SendDirectMessageForLargeContentServiceTest : AbstractIntegrationTestForCommunicationUnits
    {
        private static readonly HttpClient HttpClientForSender =
            HttpClientFactory.AuthenticatedNonLoggingHttpClient(Sender);

        [Fact(Skip =
            "Does currently fail because of the new Release 1.2 in QA and needs to be fixed when the implementation is clear.")]
        public void GivenValidMessageContentWhenSendingMessageToSingleRecipientThenTheMessageShouldBeDelivered()
        {
            var sendMessageService =
                new SendDirectMessageService(new HttpMessagingService(HttpClientForSender));
            var sendMessageParameters = new SendMessageParameters
            {
                OnboardResponse = Sender,
                ApplicationMessageId = MessageIdService.ApplicationMessageId(),
                TechnicalMessageType = TechnicalMessageTypes.ImgPng,
                Recipients = new List<string> {Recipient.SensorAlternateId},
                Base64MessageContent = DataProvider.ReadBase64EncodedLargeBmp()
            };
            sendMessageService.Send(sendMessageParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            var fetchMessageService = new FetchMessageService(HttpClientForSender);
            var fetch = fetchMessageService.Fetch(Sender);
            Assert.Equal(6, fetch.Count);

            foreach (var messageResponse in fetch)
            {
                var decodedMessage = DecodeMessageService.Decode(messageResponse.Command.Message);
                Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
            }
        }

        private static OnboardResponse Sender =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Sender);

        private static OnboardResponse Recipient =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Recipient);
    }
}