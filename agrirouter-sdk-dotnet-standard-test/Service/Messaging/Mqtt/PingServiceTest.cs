using System;
using System.Text;
using System.Threading.Tasks;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using Xunit;

namespace Agrirouter.Test.Service.Messaging.Mqtt
{
    [Collection("Integrationtest")]
    public class PingServiceTest : AbstractIntegrationTestForCommunicationUnits
    {
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

            mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                messageReceived = true;

                var msg = JsonConvert.DeserializeObject<MessageResponse>(
                    Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                var decodedMessage = DecodeMessageService.Decode(msg.Command.Message);

                Assert.Equal(200, decodedMessage.ResponseEnvelope.ResponseCode);
            };

            while (!messageReceived && counter < 5)
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                counter++;
            }

            Assert.True(messageReceived);
        }

        private static OnboardResponse OnboardResponse =>
            OnboardResponseIntegrationService.Read(Identifier.Mqtt.CommunicationUnit.SingleEndpointWithP12Certificate);


        [Fact(Skip =
            "Concept only: A deleted endpoint can not be tested automatically and requires some manual actions.")]
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

            mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                messageReceived = true;

                var msg = JsonConvert.DeserializeObject<MessageResponse>(
                    Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                var decodedMessage = DecodeMessageService.Decode(msg.Command.Message);

                // your own application should remove the endpoint from your endpoint list/registry now!
                Assert.Equal(400, decodedMessage.ResponseEnvelope.ResponseCode);
            };

            while (!messageReceived && counter < 5)
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                counter++;
            }

            Assert.True(messageReceived);
        }
    }
}