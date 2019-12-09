using System;
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
using Newtonsoft.Json;

namespace com.dke.data.agrirouter.impl.service
{
    public class MessagingService : IMessagingService<MessagingParameters>
    {
        private readonly UtcDataService _utcDataService;

        public MessagingService()
        {
            _utcDataService = new UtcDataService();
        }

        public string send(MessagingParameters capabilitiesParameters)
        {
            var messageRequest = new MessageRequest
            {
                SensorAlternateId = capabilitiesParameters.OnboardingResponse.SensorAlternateId,
                CapabilityAlternateId = capabilitiesParameters.OnboardingResponse.CapabilityAlternateId,
                Messages = new List<Message>()
            };
            foreach (var encodedMessage in capabilitiesParameters.EncodedMessages)
            {
                var message = new Message {Content = encodedMessage, Timestamp = _utcDataService.Now};
                messageRequest.Messages.Add(message);
            }

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ClientCertificates.Add(new X509Certificate2(
                Convert.FromBase64String(capabilitiesParameters.OnboardingResponse.Authentication.Certificate),
                capabilitiesParameters.OnboardingResponse.Authentication.Secret));
            var httpClient = new HttpClient(new LoggingHandler(httpClientHandler));
            HttpContent requestBody = new StringContent(JsonConvert.SerializeObject(messageRequest), Encoding.UTF8,
                "application/json");
            var httpResponseMessage = httpClient
                .PostAsync(capabilitiesParameters.OnboardingResponse.ConnectionCriteria.Measures, requestBody).Result;

            return string.Empty;
        }
    }
}