using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Impl.Service.Common;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Xunit;

namespace Agrirouter.Test.Helper
{
    /// <summary>
    /// Simple helper class to manage the connections and subscriptions for the example client in the test environment.
    /// </summary>
    public class MqttConnectionHelper
    {
        /// <summary>
        /// Subscribing to a topic to receive the messages from the AR.
        /// </summary>
        /// <param name="mqttClient">-</param>
        /// <param name="onboardResponse">-</param>
        /// <returns>No dedicated response.</returns>
        public static async Task SubscribeToTopics(IMqttClient mqttClient, OnboardResponse onboardResponse)
        {
            Assert.True(mqttClient.IsConnected);
            await mqttClient.SubscribeAsync(onboardResponse.ConnectionCriteria.Commands);
        }

        /// <summary>
        /// Connect the client to the AR. The given options suit the testcases but have to be reviewed for a production environment regarding reconnecting for example.
        /// </summary>
        /// <param name="mqttClient">-</param>
        /// <param name="onboardResponse">-</param>
        /// <returns>No dedicated response.</returns>
        public static async Task ConnectMqttClient(IMqttClient mqttClient, OnboardResponse onboardResponse)
        {
            var tlsParameters = new MqttClientOptionsBuilderTlsParameters
            {
                Certificates = new[] {X509CertificateService.GetCertificate(onboardResponse)},
                UseTls = true
            };

            var options = new MqttClientOptionsBuilder()
                .WithClientId(onboardResponse.ConnectionCriteria.ClientId)
                .WithTcpServer(onboardResponse.ConnectionCriteria.Host,
                    int.Parse(onboardResponse.ConnectionCriteria.Port))
                .WithTls(tlsParameters)
                .WithCommunicationTimeout(TimeSpan.FromSeconds(20))
                .Build();

            await mqttClient.ConnectAsync(options);
        }
    }
}