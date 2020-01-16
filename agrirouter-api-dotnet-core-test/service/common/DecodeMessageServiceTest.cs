using System;
using Agrirouter.Api.Exception;
using Agrirouter.Impl.Service.Common;
using Xunit;

namespace Agrirouter.Api.test.service.common
{
    /// <summary>
    /// Functional tests.
    /// </summary>
    public class DecodeMessageServiceTest
    {
        [Fact]
        public void GivenWhitespacesAsMessageWhenDecodingTheMessageThenThereShouldBeAnException()
        {
            Assert.Throws<ArgumentException>(() => new DecodeMessageService().Decode("   "));
        }

        [Fact]
        public void GivenInvalidDataAsMessageWhenDecodingTheMessageThenThereShouldBeAnException()
        {
            Assert.Throws<CouldNotDecodeMessageException>(() => new DecodeMessageService().Decode("INVALID MESSAGE"));
        }

        [Fact]
        public void
            GivenValidCapabilitiesMessageResponseWhenDecodingTheMessageThenTheResponseEnvelopeAndResponsePayloadWrapperShouldBeFilled()
        {
            var responseMessage =
                "XgjJARACGiQ5MDAxMjg3Yi03NjA5LTQ0MjYtYjU3My0xNTE5MmQwYzEyNDIiJGMyZGViM2VlLTIyMDItNDA1Mi1hZDJhLTUxMjVkND" +
                "g3YjdmNioLCMHy1/AFEICvvnGEAQqBAQowdHlwZXMuYWdyaXJvdXRlci5jb20vYWdyaXJvdXRlci5jb21tb25zLk1lc3NhZ2VzEk0K" +
                "Swo9U2tpcHBpbmcgY2FwYWJpbGl0aWVzIHVwZGF0ZSBiZWNhdXNlIHRoZXJlIGFyZSBubyBkaWZmZXJlbmNlcxIKVkFMXzAwMDAyMg==";

            var decodeMessageService = new DecodeMessageService();
            var decodedMessage = decodeMessageService.Decode(responseMessage);
            Assert.NotNull(decodedMessage.ResponseEnvelope);
            Assert.NotNull(decodedMessage.ResponsePayloadWrapper);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }

        [Fact]
        public void GivenValidMessageWhenDecodingTheMessageThenTheMessageShouldBeParsedCorrectly()
        {
            var responseMessage =
                "EwiQAxADKgwIpITc8AUQgP/1sQNvCm0KMHR5cGVzLmFncmlyb3V0ZXIuY29tL2Fncmlyb3V0ZXIuY29tbW9ucy5NZXNzYWdlcxI5" +
                "CjcKKUVycm9yIG9jY3VyZWQgd2hpbGUgZGVjb2RpbmcgdGhlIG1lc3NhZ2UuEgpWQUxfMDAwMzAw";

            var decodeMessageService = new DecodeMessageService();
            var decodedMessage = decodeMessageService.Decode(responseMessage);
            Assert.NotNull(decodedMessage.ResponseEnvelope);
            Assert.NotNull(decodedMessage.ResponsePayloadWrapper);
            Assert.Equal(400, decodedMessage.ResponseEnvelope.ResponseCode);

            var messages = decodeMessageService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(messages);
            Assert.NotEmpty(messages.Messages_);
            Assert.Single(messages.Messages_);
            Assert.Equal("VAL_000300", messages.Messages_[0].MessageCode);
            Assert.Equal("Error occured while decoding the message.", messages.Messages_[0].Message_);
        }
    }
}