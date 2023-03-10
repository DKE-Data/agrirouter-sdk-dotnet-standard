using System;
using System.Collections.Generic;
using System.Net.Http;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Request;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Agrirouter.Test.Service;
using Efdi;
using Google.Protobuf;
using Xunit;

namespace Agrirouter.Test.Integration
{
    public class
        PublishDeviceDescriptionAndTimeLogIntegrationTest : AbstractIntegrationTestForCommunicationUnits
    {
        private static readonly HttpClient HttpClient = HttpClientFactory.AuthenticatedHttpClient(OnboardResponse);

        private static OnboardResponse OnboardResponse =>
            OnboardResponseIntegrationService.Read(Identifier.Http.CommunicationUnit.SenderWithMultipleRecipients);

        /// <summary>
        /// Publish device description to the agrirouter.
        /// The test is successful if the device description is published and there is at least one recipient.
        /// </summary>
        [Fact]
        public void
            GivenValidDeviceDescriptionWhenPublishingTheMessageTheAgrirouterShouldCreateTheMachine()
        {
            // Arrange, read the device description from the JSON structure.
            var deviceDescription = ISO11783_TaskData.Parser.ParseJson(DeviceDescriptionAsJson);

            // Define the header parameters for sending the message.
            var messageHeaderParameters = new MessageHeaderParameters
            {
                Mode = RequestEnvelope.Types.Mode.Publish,
                ApplicationMessageId = MessageIdService.ApplicationMessageId(),
                // IMPORTANT NOTE //
                // You need to set the team set context ID by yourself.
                // This is only an example value, since there is no real team set checking in the test.
                // Please be aware that the team set context ID has some business constraints.
                // IMPORTANT NOTE //
                TeamSetContextId = Guid.NewGuid().ToString(),
                TechnicalMessageType = TechnicalMessageTypes.Iso11783DeviceDescriptionProtobuf
            };

            // Define the payload parameters for sending the message.
            var messagePayloadParameters = new MessagePayloadParameters()
            {
                Value = deviceDescription.ToByteString(),
                TypeUrl = ISO11783_TaskData.Descriptor.FullName
            };

            // Encode the message.
            var encodedMessage = EncodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters);

            // Publish the message.
            var publishMessageService = new PublishMessageService(new HttpMessagingService(HttpClient));
            var messagingParameters = new MessagingParameters
            {
                OnboardResponse = OnboardResponse,
                EncodedMessages = new List<string> { encodedMessage }
            };
            publishMessageService.Send(messagingParameters);

            // Wait for the agrirouter to create the machine and process the message in total.
            Timer.WaitForTheAgrirouterToProcessTheMessage();

            // Assert that the machine is created and there is at least one recipient.
            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);
          
            // We are not able to guarantee that the message is received by the recipient. Therefore the result would be an HTTP 400.
            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(400, decodedMessage.ResponseEnvelope.ResponseCode);
            var messages = DecodeMessageService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(messages);
            Assert.NotEmpty(messages.Messages_);
            Assert.Single(messages.Messages_);
            Assert.Equal("VAL_000004", messages.Messages_[0].MessageCode);
            Assert.Equal("No recipients have been identified for iso:11783:-10:device_description:protobuf.",messages.Messages_[0].Message_);
        }

        private static string DeviceDescriptionAsJson => "{\n" +
                                                         "  \"versionMajor\": \"VERSION_MAJOR_E2_DIS\",\n" +
                                                         "  \"versionMinor\": 1,\n" +
                                                         "  \"taskControllerManufacturer\": \"HOLMER EasyHelp 4.0\",\n" +
                                                         "  \"taskControllerVersion\": \"0.0.1\",\n" +
                                                         "  \"device\": [\n" +
                                                         "    {\n" +
                                                         "      \"deviceId\": {\n" +
                                                         "        \"number\": \"-1\"\n" +
                                                         "      },\n" +
                                                         "      \"deviceDesignator\": \"harvester\",\n" +
                                                         "      \"clientName\": \"oBCEAD3hBNI=\",\n" +
                                                         "      \"deviceSerialNumber\": \"T4_4095\",\n" +
                                                         "      \"deviceElement\": [\n" +
                                                         "        {\n" +
                                                         "          \"deviceElementId\": {\n" +
                                                         "            \"number\": \"-1\"\n" +
                                                         "          },\n" +
                                                         "          \"deviceElementObjectId\": 100,\n" +
                                                         "          \"deviceElementType\": \"C_DEVICE\",\n" +
                                                         "          \"deviceElementDesignator\": \"Maschine\",\n" +
                                                         "          \"deviceObjectReference\": [\n" +
                                                         "            {\n" +
                                                         "              \"deviceObjectId\": 10000\n" +
                                                         "            },\n" +
                                                         "            {\n" +
                                                         "              \"deviceObjectId\": 10001\n" +
                                                         "            },\n" +
                                                         "            {\n" +
                                                         "              \"deviceObjectId\": 10002\n" +
                                                         "            },\n" +
                                                         "            {\n" +
                                                         "              \"deviceObjectId\": 10003\n" +
                                                         "            },\n" +
                                                         "            {\n" +
                                                         "              \"deviceObjectId\": 10004\n" +
                                                         "            }\n" +
                                                         "          ]\n" +
                                                         "        }\n" +
                                                         "      ],\n" +
                                                         "      \"deviceProcessData\": [\n" +
                                                         "        {\n" +
                                                         "          \"deviceProcessDataObjectId\": 10000,\n" +
                                                         "          \"deviceProcessDataDdi\": 271,\n" +
                                                         "          \"deviceValuePresentationObjectId\": 10000\n" +
                                                         "        },\n" +
                                                         "        {\n" +
                                                         "          \"deviceProcessDataObjectId\": 10001,\n" +
                                                         "          \"deviceProcessDataDdi\": 394,\n" +
                                                         "          \"deviceValuePresentationObjectId\": 10001\n" +
                                                         "        },\n" +
                                                         "        {\n" +
                                                         "          \"deviceProcessDataObjectId\": 10002,\n" +
                                                         "          \"deviceProcessDataDdi\": 395,\n" +
                                                         "          \"deviceValuePresentationObjectId\": 10002\n" +
                                                         "        },\n" +
                                                         "        {\n" +
                                                         "          \"deviceProcessDataObjectId\": 10003,\n" +
                                                         "          \"deviceProcessDataDdi\": 397,\n" +
                                                         "          \"deviceValuePresentationObjectId\": 10003\n" +
                                                         "        },\n" +
                                                         "        {\n" +
                                                         "          \"deviceProcessDataObjectId\": 10004,\n" +
                                                         "          \"deviceProcessDataDdi\": 493,\n" +
                                                         "          \"deviceValuePresentationObjectId\": 10004\n" +
                                                         "        }\n" +
                                                         "      ],\n" +
                                                         "      \"deviceValuePresentation\": [\n" +
                                                         "        {\n" +
                                                         "          \"deviceValuePresentationObjectId\": 10000,\n" +
                                                         "          \"scale\": 1.0\n" +
                                                         "        },\n" +
                                                         "        {\n" +
                                                         "          \"deviceValuePresentationObjectId\": 10001,\n" +
                                                         "          \"scale\": 1.0\n" +
                                                         "        },\n" +
                                                         "        {\n" +
                                                         "          \"deviceValuePresentationObjectId\": 10002,\n" +
                                                         "          \"scale\": 1.0\n" +
                                                         "        },\n" +
                                                         "        {\n" +
                                                         "          \"deviceValuePresentationObjectId\": 10003,\n" +
                                                         "          \"scale\": 1.0\n" +
                                                         "        },\n" +
                                                         "        {\n" +
                                                         "          \"deviceValuePresentationObjectId\": 10004,\n" +
                                                         "          \"scale\": 1.0\n" +
                                                         "        }\n" +
                                                         "      ]\n" +
                                                         "    }\n" +
                                                         "  ]\n" +
                                                         "}";
    }
}