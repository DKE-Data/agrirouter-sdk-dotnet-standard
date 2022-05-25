using System;
using Agrirouter.Api.Exception;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Xunit;

namespace Agrirouter.Test.Service.Common
{
    /// <summary>
    ///     Functional tests.
    /// </summary>
    public class DecodeListEndpointsMessageTest
    {
        [Fact]
        public void
            GivenValidListEndpointsMessageResponseFromTheAgrirouterWhenDecodingThenTheServiceShouldReturnTheRightResult()
        {
            const string responseMessage =
                "CogBCiQzNDE5MjM4OS02YWM4LTQ4MTYtOGNhNS1hNjY3ZWYwMmExOTUSIFdhZ2VuaW5nZW4gVW5pdmVyc2l0eSAmIFJlc2VhcmNoGg1wYWlyZWRBY2NvdW50IgZhY3RpdmUqHgoaaXNvOjExNzgzOi0xMDp0YXNrZGF0YTp6aXAQADIHbWFjaGluZQ==";

            var decodedMessage = new ListEndpointsService(null).Decode(new Any
                { Value = ByteString.CopyFrom(Convert.FromBase64String(responseMessage)) });
            Assert.NotNull(decodedMessage.Endpoints);
            Assert.NotEmpty(decodedMessage.Endpoints);
            Assert.Single(decodedMessage.Endpoints);
            Assert.Equal("Wageningen University & Research", decodedMessage.Endpoints[0].EndpointName);
            Assert.Equal("34192389-6ac8-4816-8ca5-a667ef02a195", decodedMessage.Endpoints[0].EndpointId);
            Assert.Equal("active", decodedMessage.Endpoints[0].Status);
        }
    }
}