using System.Collections.Generic;
using System.Linq;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.test.Data;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.messaging;
using Xunit;

namespace Agrirouter.Api.Test.Service.Messaging
{
    /// <summary>
    /// Functional tests.
    /// </summary>
    public class MessageChunkingTest
    {
        [Fact]
        public void GivenSmallMessageContentWhenCheckingIfTheMessageHasToBeChunkedThenTheMethodShouldReturnFalse()
        {
            var sendMessageParameters = new SendMessageParameters
                {Base64MessageContent = DataProvider.ReadBase64EncodedSmallShape()};
            Assert.False(SendDirectMessageService.MessageHasToBeChunked(sendMessageParameters.Base64MessageContent));
        }

        [Fact]
        public void GivenBigMessageContentWhenCheckingIfTheMessageHasToBeChunkedThenTheMethodShouldReturnTrue()
        {
            var sendMessageParameters = new SendMessageParameters
                {Base64MessageContent = DataProvider.ReadBase64EncodedLargeShape()};
            Assert.True(SendDirectMessageService.MessageHasToBeChunked(sendMessageParameters.Base64MessageContent));
        }

        [Fact]
        public void GivenBigMessageContentWhenChunkingTheMessageThenTheMethodShouldReturnAllChunks()
        {
            var chunkedMessages = new List<string>(SendDirectMessageService.ChunkMessageContent(
                DataProvider.ReadBase64EncodedLargeShape(),
                ChunkSizeDefinition.MaximumSupported));
            Assert.Equal(5, chunkedMessages.Count);

            var completeContent = chunkedMessages.Aggregate("", (current, chunkedMessage) => current + chunkedMessage);
            Assert.Equal(DataProvider.ReadBase64EncodedLargeShape(), completeContent);
        }

        [Fact]
        public void GivenSmallMessageContentWhenChunkingTheMessageThenTheMethodShouldReturnASingleChunk()
        {
            var chunkedMessages = new List<string>(SendDirectMessageService.ChunkMessageContent(
                DataProvider.ReadBase64EncodedSmallShape(),
                ChunkSizeDefinition.MaximumSupported));
            Assert.Single(chunkedMessages);
            Assert.Equal(DataProvider.ReadBase64EncodedSmallShape(), chunkedMessages[0]);
        }

        private SendDirectMessageService SendDirectMessageService =>
            new SendDirectMessageService(new MessagingService(null), new EncodeMessageService());
    }
}