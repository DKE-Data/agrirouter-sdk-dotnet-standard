using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Agrirouter.Api.Builder;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Newtonsoft.Json;
using Serilog;

namespace Agrirouter.Impl.Service.Common
{
    /// <summary>
    ///     Service to send messages to the AR.
    /// </summary>
    public class HttpMessagingService : IMessagingService<MessagingParameters>
    {
        private readonly HttpClient _httpClient;

        public HttpClient HttpClient
        {
            get { return _httpClient; }
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="httpClient">-</param>
        public HttpMessagingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        ///     Send message to the AR using the given message parameters.
        /// </summary>
        /// <param name="messagingParameters">Messaging parameters.</param>
        /// <returns>-</returns>
        /// <exception cref="CouldNotSendHttpMessageException">Will be thrown if the message could not be send.</exception>
        public MessagingResult Send(MessagingParameters messagingParameters)
        {
            var messageRequest = new MessageRequest
            {
                SensorAlternateId = messagingParameters.OnboardResponse.SensorAlternateId,
                CapabilityAlternateId = messagingParameters.OnboardResponse.CapabilityAlternateId,
                Messages = new List<Api.Dto.Messaging.Inner.Message>()
            };

            foreach (var message in messagingParameters.EncodedMessages.Select(encodedMessage =>
                new Api.Dto.Messaging.Inner.Message
                    {Content = encodedMessage, Timestamp = UtcDataService.NowAsUnixTimestamp()}))
                messageRequest.Messages.Add(message);

            HttpContent requestBody = new StringContent(JsonConvert.SerializeObject(messageRequest), Encoding.UTF8,
                "application/json");
            var httpResponseMessage = _httpClient
                .PostAsync(messagingParameters.OnboardResponse.ConnectionCriteria.Measures, requestBody).Result;

            if (httpResponseMessage.IsSuccessStatusCode)
                return new MessagingResultBuilder().WithApplicationMessageId(messagingParameters.ApplicationMessageId)
                    .Build();

            Log.Error("Sending the message was not successful. HTTP response was " +
                      httpResponseMessage.StatusCode + ". Please check exception for more details.");
            throw new CouldNotSendHttpMessageException(httpResponseMessage.StatusCode,
                httpResponseMessage.Content.ReadAsStringAsync().Result);
        }
        
        /// <summary>
        ///     Send message to the AR using the given message parameters.
        /// </summary>
        /// <param name="messagingParameters">Messaging parameters.</param>
        /// <returns>-</returns>
        /// <exception cref="CouldNotSendHttpMessageException">Will be thrown if the message could not be send.</exception>
        public async Task<MessagingResult> SendAsync(MessagingParameters messagingParameters)
        {
            var messageRequest = new MessageRequest
            {
                SensorAlternateId = messagingParameters.OnboardResponse.SensorAlternateId,
                CapabilityAlternateId = messagingParameters.OnboardResponse.CapabilityAlternateId,
                Messages = new List<Api.Dto.Messaging.Inner.Message>()
            };

            foreach (var message in messagingParameters.EncodedMessages.Select(encodedMessage =>
                new Api.Dto.Messaging.Inner.Message
                    {Content = encodedMessage, Timestamp = UtcDataService.NowAsUnixTimestamp()}))
                messageRequest.Messages.Add(message);

            HttpContent requestBody = new StringContent(JsonConvert.SerializeObject(messageRequest), Encoding.UTF8,
                "application/json");
            var httpResponseMessage = await _httpClient.PostAsync(messagingParameters.OnboardResponse.ConnectionCriteria.Measures, requestBody).ConfigureAwait(false);

            if (httpResponseMessage.IsSuccessStatusCode)
                return new MessagingResultBuilder().WithApplicationMessageId(messagingParameters.ApplicationMessageId).Build();

            Log.Error($"Sending the message was not successful. HTTP response was {httpResponseMessage.StatusCode}. Please check exception for more details.");
            throw new CouldNotSendHttpMessageException(httpResponseMessage.StatusCode,
                httpResponseMessage.Content.ReadAsStringAsync().Result);
        }
    }
}