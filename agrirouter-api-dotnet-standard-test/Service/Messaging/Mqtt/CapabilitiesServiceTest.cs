using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Api.Test.Data;
using Agrirouter.Api.Test.Helper;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Request.Payload.Endpoint;
using MQTTnet;
using MQTTnet.Client;
using Xunit;

namespace Agrirouter.Api.Test.Service.Messaging.Mqtt
{
    [Collection("Integrationtest")]
    public class CapabilitiesServiceTest : AbstractIntegrationTestForCommunicationUnits
    {
        [Fact(Skip =
            "Currently throws an exception with 'Server Unavailable', even after updating the onboarding responses.")]
        public async void
            GivenValidCapabilitiesWhenSendingCapabilitiesMessageThenTheAgrirouterShouldSetTheCapabilities()
        {
            var mqttClient = new MqttFactory().CreateMqttClient();

            await MqttConnectionHelper.ConnectMqttClient(mqttClient, OnboardResponse);
            await MqttConnectionHelper.SubscribeToTopics(mqttClient, OnboardResponse);

            var capabilitiesServices = new CapabilitiesService(new MqttMessagingService(mqttClient));
            var capabilitiesParameters = new CapabilitiesParameters
            {
                OnboardResponse = OnboardResponse,
                ApplicationId = Applications.CommunicationUnit.ApplicationId,
                CertificationVersionId = Applications.CommunicationUnit.CertificationVersionId,
                EnablePushNotifications = CapabilitySpecification.Types.PushNotification.Disabled,
                CapabilityParameters = new List<CapabilityParameter>()
            };

            var capabilitiesParameter = new CapabilityParameter
            {
                Direction = CapabilitySpecification.Types.Direction.SendReceive,
                TechnicalMessageType = TechnicalMessageTypes.Iso11783TaskdataZip
            };

            capabilitiesParameters.CapabilityParameters.Add(capabilitiesParameter);
            capabilitiesServices.Send(capabilitiesParameters);
            var messageReceived = false;
            var counter = 0;

            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                messageReceived = true;
                var messagePayload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                var decodedMessage = DecodeMessageService.Decode(messagePayload);
                Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
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
    }
}