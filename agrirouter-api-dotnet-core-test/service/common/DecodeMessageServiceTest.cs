using System;
using com.dke.data.agrirouter.api.exception;
using com.dke.data.agrirouter.impl.service.common;
using Xunit;

namespace com.dke.data.agrirouter.api.test.service.common
{
    public class DecodeMessageServiceTest
    {
        [Fact]
        public void GivenNullAsMessageWhenDecodingTheMessageThenThereShouldBeAnException()
        {
            Assert.Throws<ArgumentException>(() => new DecodeMessageService().Decode(null));
        }

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

            DecodeMessageService decodeMessageService = new DecodeMessageService();
            var decodedMessage = decodeMessageService.Decode(responseMessage);
            Assert.NotNull(decodedMessage.ResponseEnvelope);
            Assert.NotNull(decodedMessage.ResponsePayloadWrapper);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }
    }
}