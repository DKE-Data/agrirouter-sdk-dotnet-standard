using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Exception;
using Newtonsoft.Json;
using Serilog;

namespace Agrirouter.Impl.Service.Messaging
{
    /// <summary>
    ///     Service to fetch messages from the agrirouter.
    /// </summary>
    public class FetchMessageService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="httpClient">-</param>
        public FetchMessageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        ///     Fetch messages from the inbox using the given onboarding response.
        /// </summary>
        /// <param name="onboardResponse">All the messages that are in the inbox.</param>
        /// <returns>-</returns>
        /// <exception cref="CouldNotFetchMessagesException">Will be thrown if the messages can not be fetched.</exception>
        public List<MessageResponse> Fetch(OnboardResponse onboardResponse)
        {
            Log.Debug("Begin fetching messages.");
            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(onboardResponse.ConnectionCriteria.Commands)
            };
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            var httpResponseMessage = _httpClient
                .SendAsync(httpRequestMessage).Result;

            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new CouldNotFetchMessagesException(httpResponseMessage.StatusCode,
                    httpResponseMessage.Content.ReadAsStringAsync().Result);

            var messageResponses =
                JsonConvert.DeserializeObject<List<MessageResponse>>(httpResponseMessage.Content.ReadAsStringAsync()
                    .Result);
            Log.Debug("Finished fetching messages.");
            return messageResponses;
        }

        public async Task<List<MessageResponse>> FetchAsync(OnboardResponse onboardResponse)
        {
            Log.Debug("Begin fetching messages.");
            
            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(onboardResponse.ConnectionCriteria.Commands)
            };
            
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            var messageContent = await httpResponseMessage.Content.ReadAsStringAsync();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new CouldNotFetchMessagesException(httpResponseMessage.StatusCode, messageContent);
            }

            var messageResponses = JsonConvert.DeserializeObject<List<MessageResponse>>(messageContent);
            Log.Debug("Finished fetching messages.");
            return messageResponses;
        }
    }
}