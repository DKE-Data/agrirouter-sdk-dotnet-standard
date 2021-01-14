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
using Agrirouter.Api.Test.Service;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Request.Payload.Endpoint;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using Xunit;

namespace Agrirouter.Api.test.Service.Messaging.Mqtt
{
    [Collection("Integrationtest")]
    public class CapabilitiesServiceTest : AbstractIntegrationTestForCommunicationUnits
    {
        [Fact]
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

        private OnboardResponse OnboardResponse
        {
            get
            {
                const string onboardingResponseAsJson =
                    "{\"deviceAlternateId\":\"fbd69481-f4c2-4a66-98b1-ffee6ef6a865\",\"capabilityAlternateId\":\"c2467f6d-0a7e-48ca-9b57-1862186aef12\",\"sensorAlternateId\":\"a24dbe24-7dc4-4720-92ea-4f7bb9c92df6\",\"connectionCriteria\":{\"gatewayId\":\"2\",\"host\":\"dke-qa.eu10.cp.iot.sap\",\"port\":8883,\"clientId\":\"fbd69481-f4c2-4a66-98b1-ffee6ef6a865\",\"measures\":\"measures/fbd69481-f4c2-4a66-98b1-ffee6ef6a865\",\"commands\":\"commands/fbd69481-f4c2-4a66-98b1-ffee6ef6a865\"},\"authentication\":{\"type\":\"P12\",\"secret\":\"bfbnyKEB5lni2v3bSII1iwtmBkQY7Pdo5rwf\",\"certificate\":\"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\"}}";
                var onboardingResponse =
                    JsonConvert.DeserializeObject(onboardingResponseAsJson, typeof(OnboardResponse));
                return onboardingResponse as OnboardResponse;
            }
        }
    }
}