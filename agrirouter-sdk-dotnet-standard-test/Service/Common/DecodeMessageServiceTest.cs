using System;
using Agrirouter.Sdk.Api.Exception;
using Agrirouter.Sdk.Impl.Service.Common;
using Xunit;

namespace Agrirouter.Sdk.Test.Service.Common
{
    /// <summary>
    ///     Functional tests.
    /// </summary>
    public class DecodeMessageServiceTest
    {
        [Fact]
        public void GivenInvalidDataAsMessageWhenDecodingTheMessageThenThereShouldBeAnException()
        {
            Assert.Throws<CouldNotDecodeMessageException>(() => DecodeMessageService.Decode("INVALID MESSAGE"));
        }

        [Fact]
        public void
            GivenValidCapabilitiesMessageResponseWhenDecodingTheMessageThenTheResponseEnvelopeAndResponsePayloadWrapperShouldBeFilled()
        {
            const string responseMessage =
                "XgjJARACGiQ5MDAxMjg3Yi03NjA5LTQ0MjYtYjU3My0xNTE5MmQwYzEyNDIiJGMyZGViM2VlLTIyMDItNDA1Mi1hZDJhLTUxMjVkND" +
                "g3YjdmNioLCMHy1/AFEICvvnGEAQqBAQowdHlwZXMuYWdyaXJvdXRlci5jb20vYWdyaXJvdXRlci5jb21tb25zLk1lc3NhZ2VzEk0K" +
                "Swo9U2tpcHBpbmcgY2FwYWJpbGl0aWVzIHVwZGF0ZSBiZWNhdXNlIHRoZXJlIGFyZSBubyBkaWZmZXJlbmNlcxIKVkFMXzAwMDAyMg==";

            var decodedMessage = DecodeMessageService.Decode(responseMessage);
            Assert.NotNull(decodedMessage.ResponseEnvelope);
            Assert.NotNull(decodedMessage.ResponsePayloadWrapper);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }

        [Fact]
        public void GivenValidMessageWhenDecodingTheMessageThenTheMessageShouldBeParsedCorrectly()
        {
            const string responseMessage =
                "EwiQAxADKgwIpITc8AUQgP/1sQNvCm0KMHR5cGVzLmFncmlyb3V0ZXIuY29tL2Fncmlyb3V0ZXIuY29tbW9ucy5NZXNzYWdlcxI5" +
                "CjcKKUVycm9yIG9jY3VyZWQgd2hpbGUgZGVjb2RpbmcgdGhlIG1lc3NhZ2UuEgpWQUxfMDAwMzAw";

            var decodedMessage = DecodeMessageService.Decode(responseMessage);
            Assert.NotNull(decodedMessage.ResponseEnvelope);
            Assert.NotNull(decodedMessage.ResponsePayloadWrapper);
            Assert.Equal(400, decodedMessage.ResponseEnvelope.ResponseCode);

            var messages = DecodeMessageService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(messages);
            Assert.NotEmpty(messages.Messages_);
            Assert.Single(messages.Messages_);
            Assert.Equal("VAL_000300", messages.Messages_[0].MessageCode);
            Assert.Equal("Error occured while decoding the message.", messages.Messages_[0].Message_);
        }

        [Fact]
        public void GivenWhitespacesAsMessageWhenDecodingTheMessageThenThereShouldBeAnException()
        {
            Assert.Throws<ArgumentException>(() => DecodeMessageService.Decode("   "));
        }

        [Fact]
        public void GivenErrorMessageWhenDecodingTheMessageThenTheMessageShouldBeParsedCorrectly()
        {
            const string responseMessage =
                "XgiQAxADGiQ0MDkyNzU3Yi1iNjIxLTRjMzktODMyMi03ODIzOGY3Mjc3NDciJDhiODA3M2Y0LThhOGItNDU5NC1iMTYwLWZmYWRmMDJm" +
                "NTFiNioLCMHNs/oFEMDDkweYAQqVAQowdHlwZXMuYWdyaXJvdXRlci5jb20vYWdyaXJvdXRlci5jb21tb25zLk1lc3NhZ2VzEmEKXwpR" +
                "Tm8gcmVjaXBpZW50cyBoYXZlIGJlZW4gaWRlbnRpZmllZCBmb3IgaXNvOjExNzgzOi0xMDpkZXZpY2VfZGVzY3JpcHRpb246cHJvdG9" +
                "idWYuEgpWQUxfMDAwMDA0";

            var decodedMessage = DecodeMessageService.Decode(responseMessage);
            Assert.NotNull(decodedMessage.ResponseEnvelope);
            Assert.NotNull(decodedMessage.ResponsePayloadWrapper);
            Assert.Equal(400, decodedMessage.ResponseEnvelope.ResponseCode);

            var messages = DecodeMessageService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(messages);
            Assert.NotEmpty(messages.Messages_);
            Assert.Single(messages.Messages_);
            Assert.Equal("VAL_000004", messages.Messages_[0].MessageCode);
            Assert.Equal("No recipients have been identified for iso:11783:-10:device_description:protobuf.",
                messages.Messages_[0].Message_);
        }
    }
}