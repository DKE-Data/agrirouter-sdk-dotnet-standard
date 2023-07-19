using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Feed.Request;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Impl.Service.Messaging.CancellationToken;
using Agrirouter.Request;
using Agrirouter.Response;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Agrirouter.Test.Service;
using Google.Protobuf;
using Xunit;

namespace Agrirouter.Test.Integration
{

    [Collection("Integrationtest")]
    public class SendAndReceiveChunkedMessagesTest : AbstractIntegrationTestForCommunicationUnits
    {

        private static readonly int _maxChunkSize = 1024000;

        private static OnboardResponse Sender =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Sender);

        private static OnboardResponse Recipient =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Recipient);

        private static readonly HttpClient HttpClientForSender = HttpClientFactory.AuthenticatedHttpClient(Sender);

        private static readonly HttpClient
            HttpClientForRecipient = HttpClientFactory.AuthenticatedHttpClient(Recipient);

        /**
         * Cleanup before each test case. These actions are necessary because it could be the
         * case that there are dangling messages from former tests.
         */
        public SendAndReceiveChunkedMessagesTest()
        {
            var fetchMessageService = new FetchMessageService(HttpClientForRecipient);
            var queryMessageHeadersService =
                new QueryMessageHeadersService(new HttpMessagingService(HttpClientForRecipient));

            //  [1] Clean the outbox of the endpoint.
            fetchMessageService.Fetch(Recipient, new DefaultCancellationToken(3, 500));
            //  [2] Fetch all message headers for the last 4 weeks (maximum retention time within the
            //  agrirouter).
            var queryMessageParameters = new QueryMessagesParameters()
            {
                OnboardResponse = Recipient,
                ValidityPeriod = new ValidityPeriod()
                {
                    SentFrom = UtcDataService.Timestamp(TimestampOffset.FourWeeks),
                    SentTo = UtcDataService.Timestamp(0)
                }
            };
            queryMessageHeadersService.Send(queryMessageParameters);
            Timer.WaitForTheAgrirouterToProcessTheMessage();
            var fetchMessageResponses = fetchMessageService.Fetch(Recipient, new DefaultCancellationToken(3, 500));
            Assert.Single(fetchMessageResponses);
            var decodedMessage = DecodeMessageService.Decode(fetchMessageResponses[0].Command.Message);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.AckForFeedHeaderList,
                decodedMessage.ResponseEnvelope.Type);
            var headerQueryResponse = queryMessageHeadersService.Decode(decodedMessage.ResponsePayloadWrapper.Details);

            //  [3] Delete the dangling messages from the feed of the endpoint if necessary.
            if (headerQueryResponse.QueryMetrics.TotalMessagesInQuery > 0)
            {
                var feedDeleteService = new FeedDeleteService(new HttpMessagingService(HttpClientForRecipient));
                var feedDeleteParameters = new FeedDeleteParameters()
                {
                    OnboardResponse = Recipient,
                    MessageIds = headerQueryResponse.Feed
                        .SelectMany(feed => feed.Headers)
                        .Select(header => header.MessageId)
                        .ToList()
                };
                feedDeleteService.Send(feedDeleteParameters);
                Timer.WaitForTheAgrirouterToProcessTheMessage();
            }

            //  [4] Clean the outbox of the endpoint.
            fetchMessageService.Fetch(Recipient, new DefaultCancellationToken(3, 500));
        }

        [Fact]
        void GivenRealMessageContentWhenSendingMessagesTheContentShouldMatchAfterReceivingAndMergingIt()
        {
            var messageContent = ByteString.FromBase64(DataProvider.ReadBase64EncodedBigTaskData());
            const int expectedNrOfChunks = 4;

            ActionsForSender(messageContent, expectedNrOfChunks);
            ActionsForTheRecipient(messageContent, expectedNrOfChunks);
        }

        private static void ActionsForTheRecipient(ByteString messageContent, int expectedNrOfChunks)
        {
            //  [1] Fetch all the messages within the feed. The number of headers should match the number of
            //  chunks sent.
            var queryMessagesService = new QueryMessagesService(new HttpMessagingService(HttpClientForRecipient));
            var queryMessagesParameters = new QueryMessagesParameters()
            {
                OnboardResponse = Recipient,
                Senders = new List<string> {Sender.SensorAlternateId}
            };
            queryMessagesService.Send(queryMessagesParameters);

            //  [2] Wait for the agrirouter to process the message.
            Timer.WaitForTheAgrirouterToProcessTheMessage();

            //  [3] Fetch the chunks from the outbox. Since we have the same restrictions while receiving,
            //  this has to be the same number of messages as it is chunks.
            var fetchMessageService = new FetchMessageService(HttpClientForRecipient);
            var fetchMessageResponses = fetchMessageService.Fetch(Recipient, new DefaultCancellationToken(3, 500));
            Assert.NotNull(fetchMessageResponses);
            Assert.Equal(expectedNrOfChunks, fetchMessageResponses.Count);

            //  [4] Check if the response from the AR only contains valid results.
            Assert.All(fetchMessageResponses
                    .Select(response => DecodeMessageService.Decode(response.Command.Message)),
                message =>
                    Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.AckForFeedMessage,
                        message.ResponseEnvelope.Type
                    ));


            //  [5] Map the results from the query to 'real' messages within the feed and perform some
            //  assertions.
            var feedMessages = fetchMessageResponses
                .Select(response => DecodeMessageService.Decode(response.Command.Message))
                .Select(message => queryMessagesService.Decode(message.ResponsePayloadWrapper.Details))
                .Select(response => response.Messages[0]);

            Assert.Equal(expectedNrOfChunks, feedMessages.Count());
            Assert.All(feedMessages, message => Assert.NotNull(message.Header.ChunkContext));
            Assert.All(feedMessages, message => Assert.Equal(expectedNrOfChunks, message.Header.ChunkContext.Total));
            Assert.True(feedMessages.Select(message => message.Header.ChunkContext.ContextId).Distinct().Count() == 1,
                "There should be only one chunk context ID.");

            //  [6] Confirm the chunks to remove them from the feed.
            var messageIdsToConfirm = feedMessages.Select(message => message.Header.MessageId).ToList();
            var feedConfirmParameters = new FeedConfirmParameters()
            {
                OnboardResponse = Recipient,
                MessageIds = messageIdsToConfirm
            };
            var feedConfirmService = new FeedConfirmService(new HttpMessagingService(HttpClientForRecipient));
            feedConfirmService.Send(feedConfirmParameters);

            //  [7] Fetch the response from the agrirouter after confirming the messages.
            Timer.WaitForTheAgrirouterToProcessTheMessage();
            fetchMessageResponses = fetchMessageService.Fetch(Recipient, new DefaultCancellationToken(3, 500));
            Assert.Single(fetchMessageResponses);
        }

        private static void ActionsForSender(ByteString messageContent, int expectedNrOfChunks)
        {
            var encodeMessageService = new EncodeMessageService();
            var sendMessageService = new SendDirectMessageService(new HttpMessagingService(HttpClientForSender));

            //  [1] Define the raw message, in this case this is the Base64 encoded message content, no
            //  chunking needed.
            var messageHeaderParameters = new MessageHeaderParameters()
            {
                TechnicalMessageType = TechnicalMessageTypes.Iso11783TaskdataZip,
                ApplicationMessageId = MessageIdService.ApplicationMessageId(),
                Mode = RequestEnvelope.Types.Mode.Direct,
                Recipients = new List<string> {Recipient.SensorAlternateId}
            };

            var messagePayloadParameters = new MessagePayloadParameters()
            {
                Value = messageContent,
                TypeUrl = TechnicalMessageTypes.Empty
            };

            //  [2] Chunk the message content using the SDK specific methods ('chunkAndEncode').
            var tuples =
                EncodeMessageService.ChunkAndBase64EncodeEachChunk(messageHeaderParameters, messagePayloadParameters);
            Assert.All(tuples,
                tuple => Assert.True(tuple.MessagePayloadParameters.Value.ToStringUtf8().Length <= _maxChunkSize));
            var encodedMessages = tuples.Select(tuple =>
                EncodeMessageService.Encode(tuple.MessageHeaderParameters, tuple.MessagePayloadParameters)).ToList();

            //  [3] Send the chunks to the agrirouter.
            var messagingParameters = new MessagingParameters()
            {
                OnboardResponse = Sender,
                EncodedMessages = encodedMessages,
                ApplicationMessageId = MessageIdService.ApplicationMessageId()
            };
            sendMessageService.Send(messagingParameters);

            //  [4] Wait for the AR to process the chunks.
            Timer.WaitForTheAgrirouterToProcessTheMessage();

            //  [5] Check if the chunks were processed successfully.
            var fetchMessageService = new FetchMessageService(HttpClientForSender);
            var fetchMessageResponses = fetchMessageService.Fetch(Sender, new DefaultCancellationToken(3, 500));

            Assert.NotEmpty(fetchMessageResponses);
            Assert.Equal(expectedNrOfChunks, fetchMessageResponses.Count);

            var feedMessages = fetchMessageResponses
                .Select(response => response.Command).ToList();

            Assert.All(feedMessages, Assert.NotNull);
            Assert.All(feedMessages, message =>
            {
                var decodedMessage = DecodeMessageService.Decode(message.Message);
                Assert.Contains(decodedMessage.ResponseEnvelope.ResponseCode,
                    new List<int>
                    {
                        (int) HttpStatusCode.OK,
                        (int) HttpStatusCode.Created,
                        (int) HttpStatusCode.NoContent
                    });
            });

        }
    }
}