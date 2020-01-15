using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using com.dke.data.agrirouter.api.builder;
using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.dto.messaging.inner;
using com.dke.data.agrirouter.api.exception;
using com.dke.data.agrirouter.api.service.messaging;
using com.dke.data.agrirouter.api.service.parameters;
using Newtonsoft.Json;
using Serilog;

namespace com.dke.data.agrirouter.impl.service.common
{
    /**
     * Service to send messages to the AR.
     */
    public class MessagingService : IMessagingService<MessagingParameters>
    {
        private readonly HttpClient _httpClient;
        private readonly UtcDataService _utcDataService;

        public MessagingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _utcDataService = new UtcDataService();
        }

        /**
         * Send message to the AR using the given message parameters.
         */
        public MessagingResult Send(MessagingParameters messagingParameters)
        {
            var messageRequest = new MessageRequest
            {
                SensorAlternateId = messagingParameters.OnboardingResponse.SensorAlternateId,
                CapabilityAlternateId = messagingParameters.OnboardingResponse.CapabilityAlternateId,
                Messages = new List<Message>()
            };

            foreach (var encodedMessage in messagingParameters.EncodedMessages)
            {
                var message = new Message {Content = encodedMessage, Timestamp = _utcDataService.NowAsUnixTimestamp()};
                messageRequest.Messages.Add(message);
            }

            HttpContent requestBody = new StringContent(JsonConvert.SerializeObject(messageRequest), Encoding.UTF8,
                "application/json");
            var httpResponseMessage = _httpClient
                .PostAsync(messagingParameters.OnboardingResponse.ConnectionCriteria.Measures, requestBody).Result;

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                Log.Error("Sending the message was not successful. HTTP response was " +
                          httpResponseMessage.StatusCode + ". Please check exception for more details.");
                throw new CouldNotSendMessageException(httpResponseMessage.StatusCode,
                    httpResponseMessage.Content.ReadAsStringAsync().Result);
            }

            return new MessagingResultBuilder().WithApplicationMessageId(messagingParameters.ApplicationMessageId)
                .Build();
        }
    }
}