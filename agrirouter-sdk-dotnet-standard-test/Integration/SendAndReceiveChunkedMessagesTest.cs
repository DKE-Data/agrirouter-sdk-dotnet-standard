using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Cloud.Registration;
using Agrirouter.Feed.Request;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Convenience;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Response;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Agrirouter.Test.Service;
using Xunit;
using Agrirouter.Impl.Service.Messaging.CancellationToken;
using Agrirouter.Request;
using Google.Protobuf;

namespace Agrirouter.Test.Integration
{

    [Collection("Integrationtest")]
    public class SendAndReceiveChunkedMessagesTest : AbstractIntegrationTestForCommunicationUnits {
    
        private static int MAX_CHUNK_SIZE = 1024000;

        private static OnboardResponse Sender =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Sender);

        private static OnboardResponse Recipient =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.Recipient);

        private static readonly HttpClient HttpClientForSender = HttpClientFactory.AuthenticatedHttpClient(Sender);
        private static readonly HttpClient HttpClientForRecipient = HttpClientFactory.AuthenticatedHttpClient(Recipient);

        
        void givenRealMessageContentWhenSendingMessagesTheContentShouldMatchAfterReceivingAndMergingIt(ByteString messageContent, int expectedNrOfChunks) {
            this.actionsForSender(messageContent, expectedNrOfChunks);
            this.actionsForTheRecipient(messageContent, expectedNrOfChunks);
        }
    
        private void actionsForTheRecipient(ByteString messageContent, int expectedNrOfChunks) {
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
            var fetchMessageResponse = fetchMessageService.Fetch(Recipient, new DefaultCancellationToken(3, 500));
            Assert.Single(fetchMessageResponse);
            Assert.Equal(expectedNrOfChunks, fetchMessageResponse.Count);

            //  [4] Check if the response from the AR only contains valid results.
            Assert.All(fetchMessageResponse
                .Select(response => DecodeMessageService.Decode(response.Command.Message)),
                message => 
                    Assert.Equal(Response.ResponseEnvelope.Types.ResponseBodyType.AckForFeedMessage,
                        message.ResponseEnvelope.Type
            ));
            

            //  [5] Map the results from the query to 'real' messages within the feed and perform some
            //  assertions.
            var feedMessages = fetchMessageResponse
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
            fetchMessageResponse = fetchMessageService.Fetch(Recipient, new DefaultCancellationToken(3, 500));
            Assert.Single(fetchMessageResponse);
        }
    
        private void actionsForSender(ByteString messageContent, int expectedNrOfChunks)
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
                Recipients = 

            };
            
            MessageHeaderParameters messageHeaderParameters = new MessageHeaderParameters();
            messageHeaderParameters.setTechnicalMessageType(ContentMessageType.ISO_11783_TASKDATA_ZIP);
            messageHeaderParameters.setApplicationMessageId(MessageIdService.generateMessageId());
            messageHeaderParameters.setApplicationMessageSeqNo(SequenceNumberService.generateSequenceNumberForEndpoint(onboardingResponse));
            messageHeaderParameters.setMode(Request.RequestEnvelope.Mode.DIRECT);
            messageHeaderParameters.setRecipients(Collections.singletonList(OnboardingResponseRepository.read(OnboardingResponseRepository.Identifier.COMMUNICATION_UNIT).getSensorAlternateId()));
            PayloadParameters payloadParameters = new PayloadParameters();
            payloadParameters.setValue(messageContent);
            payloadParameters.setTypeUrl(SystemMessageType.EMPTY.getKey());
            //  [2] Chunk the message content using the SDK specific methods ('chunkAndEncode').
            List<MessageParameterTuple> tuples = encodeMessageService.chunkAndBase64EncodeEachChunk(messageHeaderParameters, payloadParameters, onboardingResponse);
            tuples.forEach(messageParameterTuple, -, Greater, Assertions.assertTrue((Objects.requireNonNull(messageParameterTuple.getPayloadParameters().getValue()).toStringUtf8().length() <= MAX_CHUNK_SIZE)));
            List<String> encodedMessages = encodeMessageService.encode(tuples);
            //  [3] Send the chunks to the agrirouter.
            SendMessageParameters sendMessageParameters = new SendMessageParameters();
            sendMessageParameters.setEncodedMessages(encodedMessages);
            sendMessageParameters.setOnboardingResponse(onboardingResponse);
            sendMessageService.send(sendMessageParameters);
            //  [4] Wait for the AR to process the chunks.
            waitForTheAgrirouterToProcessMultipleMessages();
            //  [5] Check if the chunks were processed successfully.
            FetchMessageService fetchMessageService = new FetchMessageServiceImpl();
            Optional<List<FetchMessageResponse>> fetchMessageResponses = fetchMessageService.fetch(onboardingResponse, new DefaultCancellationToken(MAX_TRIES_BEFORE_FAILURE, DEFAULT_INTERVAL));
            Assertions.assertTrue(fetchMessageResponses.isPresent());
            Assertions.assertEquals(expectedNrOfChunks, fetchMessageResponses.get().size());
            DecodeMessageService decodeMessageService = new DecodeMessageServiceImpl();
            AtomicReference<DecodeMessageResponse> decodeMessageResponse = new AtomicReference();
            fetchMessageResponses.get().stream().map(FetchMessageResponse: FetchMessageResponse:, :, getCommand).forEach(message, -, Greater, {, Assertions.assertNotNull(message));
                decodeMessageResponse.set(decodeMessageService.decode(message.getMessage()));
                Assertions.assertMatchesAny(Arrays.asList(HttpStatus.SC_OK, HttpStatus.SC_CREATED, HttpStatus.SC_NO_CONTENT), decodeMessageResponse.get().getResponseEnvelope().getResponseCode());
            }
        }
        atSuppressWarnings("unused");
        privatestaticUnknown
    
        [NotNull()]
        Stream<Arguments> givenRealMessageContentWhenSendingMessagesTheContentShouldMatchAfterReceivingAndMergingIt() {
            return Stream.of(Arguments.of(ByteString.copyFrom(ContentReader.readRawData(ContentReader.Identifier.BIG_TASK_DATA)), 3));
        }
    
        [BeforeEach()]
        [AfterEach()]
        public void prepareTestEnvironment() {
            FetchMessageService fetchMessageService = new FetchMessageServiceImpl();
            OnboardingResponse recipient = OnboardingResponseRepository.read(OnboardingResponseRepository.Identifier.COMMUNICATION_UNIT);
            MessageHeaderQueryServiceImpl messageHeaderQueryService = new MessageHeaderQueryServiceImpl(new QA());
            //  [1] Clean the outbox of the endpoint.
            fetchMessageService.fetch(recipient, new DefaultCancellationToken(MAX_TRIES_BEFORE_FAILURE, DEFAULT_INTERVAL));
            //  [2] Fetch all message headers for the last 4 weeks (maximum retention time within the
            //  agrirouter).
            MessageQueryParameters messageQueryParameters = new MessageQueryParameters();
            messageQueryParameters.setOnboardingResponse(recipient);
            messageQueryParameters.setSentToInSeconds(UtcTimeService.inTheFuture(5).toEpochSecond());
            messageQueryParameters.setSentFromInSeconds(UtcTimeService.inThePast(UtcTimeService.FOUR_WEEKS_AGO).toEpochSecond());
            messageHeaderQueryService.send(messageQueryParameters);
            waitForTheAgrirouterToProcessSingleMessage();
            Optional<List<FetchMessageResponse>> fetchMessageResponses = fetchMessageService.fetch(recipient, new DefaultCancellationToken(MAX_TRIES_BEFORE_FAILURE, DEFAULT_INTERVAL));
            Assertions.assertTrue(fetchMessageResponses.isPresent());
            Assertions.assertEquals(1, fetchMessageResponses.get().size(), "This should be a single response.");
            DecodeMessageService decodeMessageService = new DecodeMessageServiceImpl();
            DecodeMessageResponse decodeMessageResponse = decodeMessageService.decode(fetchMessageResponses.get().get(0).getCommand().getMessage());
            Assertions.assertEquals(Response.ResponseEnvelope.ResponseBodyType.ACK_FOR_FEED_HEADER_LIST, decodeMessageResponse.getResponseEnvelope().getType());
            FeedResponse.HeaderQueryResponse headerQueryResponse = messageHeaderQueryService.decode(decodeMessageResponse.getResponsePayloadWrapper().getDetails().getValue());
            //  [3] Delete the dangling messages from the feed of the endpoint if necessary.
            if ((headerQueryResponse.getQueryMetrics().getTotalMessagesInQuery() > 0)) {
                DeleteMessageServiceImpl deleteMessageService = new DeleteMessageServiceImpl();
                DeleteMessageParameters deleteMessageParameters = new DeleteMessageParameters();
                deleteMessageParameters.setOnboardingResponse(recipient);
                List<String> messageIds = headerQueryResponse.getFeedList().stream().map(FeedResponse.HeaderQueryResponse.Feed, :, :, getHeadersList).flatMap(Collection: Collection:, :, stream).map(FeedResponse.HeaderQueryResponse.Header, :, :, getMessageId).collect(Collectors.toList());
                deleteMessageParameters.setMessageIds(messageIds);
                deleteMessageService.send(deleteMessageParameters);
                waitForTheAgrirouterToProcessSingleMessage();
                fetchMessageService.fetch(recipient, new DefaultCancellationToken(MAX_TRIES_BEFORE_FAILURE, DEFAULT_INTERVAL));
            }
        
            //  [4] Clean the outbox of the endpoint.
            fetchMessageService.fetch(recipient, new DefaultCancellationToken(MAX_TRIES_BEFORE_FAILURE, DEFAULT_INTERVAL));
        }
}