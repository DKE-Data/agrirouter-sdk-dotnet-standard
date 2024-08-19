using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using MQTTnet;
using MQTTnet.Client;
using Xunit;
using Agrirouter.Api.Dto.Messaging;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Agrirouter.Test.Service.Messaging.Mqtt
{
    [Collection("Integrationtest")]
    public class PingServiceTest : AbstractIntegrationTestForCommunicationUnits
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public PingServiceTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async void
            GivenExistingEndpointWhenSendingPingThenShouldBeSuccessful()
        {
            var mqttClient = new MqttFactory().CreateMqttClient();

            await MqttConnectionHelper.ConnectMqttClient(mqttClient, OnboardResponse);
            await MqttConnectionHelper.SubscribeToTopics(mqttClient, OnboardResponse);

            var pingService = new PingService(new MqttMessagingService(mqttClient));
            var pingParameters = new PingParameters()
            {
                OnboardResponse = OnboardResponse
            };

            pingService.Send(pingParameters);

            var messageReceived = false;
            var counter = 0;

            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                messageReceived = true;

                MessageResponse msg =
                    JsonConvert.DeserializeObject<MessageResponse>(
                        Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                var decodedMessage = DecodeMessageService.Decode(msg.Command.Message);

                Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
            });

            while (!messageReceived && counter < 5)
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                counter++;
            }

            Assert.True(messageReceived);
        }

        private static OnboardResponse OnboardResponse =>
            OnboardResponseIntegrationService.Read(Identifier.Mqtt.CommunicationUnit.SingleEndpointWithP12Certificate);


        [Fact(Skip="Concept only")]
        public async void
            GivenRecentlyDeletedEndpointWhenSendingPingThenShouldReturn400()
        {
            // This test case can not run. We have no Router Device based tests currently, and this command only
            // makes sense with a Router Device.
            // It is only intended to show the concept of the test case.

            var mqttClient = new MqttFactory().CreateMqttClient();

            await MqttConnectionHelper.ConnectMqttClient(mqttClient, OnboardResponse);
            await MqttConnectionHelper.SubscribeToTopics(mqttClient, OnboardResponse);

            // Endpoint would need to be deleted here
            // if it was deleted earlier, the connection wouldn't be possible

            var pingService = new PingService(new MqttMessagingService(mqttClient));
            var pingParameters = new PingParameters()
            {
                OnboardResponse = OnboardResponse
            };

            pingService.Send(pingParameters);

            var messageReceived = false;
            var counter = 0;

            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                messageReceived = true;

                MessageResponse msg =
                    JsonConvert.DeserializeObject<MessageResponse>(
                        Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                var decodedMessage = DecodeMessageService.Decode(msg.Command.Message);

                // your own application should remove the endpoint from your endpoint list/registry now!
                Assert.Equal(400, decodedMessage.ResponseEnvelope.ResponseCode);
            });

            while (!messageReceived && counter < 5)
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                counter++;
            }

            Assert.True(messageReceived);
        }
    }
}