using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.dto.messaging.inner;
using com.dke.data.agrirouter.api.logging;
using com.dke.data.agrirouter.api.service;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.impl.service.common;

namespace com.dke.data.agrirouter.impl.service
{
    public class MessagingService : IMessagingService<MessagingParameters>
    {
        private readonly UtcDataService _utcDataService;

        public MessagingService()
        {
            _utcDataService = new UtcDataService();
        }

        public string send(MessagingParameters parameters)
        {
            var messageRequest = new MessageRequest
            {
                SensorAlternateId = parameters.OnboardingResponse.SensorAlternateId,
                CapabilityAlternateId = parameters.OnboardingResponse.CapabilityAlternateId,
                Messages = new List<Message>()
            };
            foreach (var encodedMessage in parameters.EncodedMessages)
            {
                var message = new Message {Content = encodedMessage, Timestamp = _utcDataService.Now};
            }

            // https://github.com/dotnet/corefx/issues/2910
            
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ClientCertificates.Add(new X509Certificate2(
                Encoding.UTF8.GetBytes(parameters.OnboardingResponse.Authentication.Certificate),
                parameters.OnboardingResponse.Authentication.Secret));
            var httpClient = new HttpClient(new LoggingHandler(httpClientHandler));
            HttpContent requestBody = new StringContent("Tofuwurst");
            var httpResponseMessage = httpClient.PostAsync(parameters.OnboardingResponse.ConnectionCriteria.Measures, requestBody).Result;
            
            return string.Empty;
        }
    }
}