using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.dto.onboard;
using com.dke.data.agrirouter.api.exception;
using Newtonsoft.Json;
using Serilog;

namespace com.dke.data.agrirouter.impl.service.messaging
{
    /// <summary>
    /// Service to fetch messages from the agrirouter.
    /// </summary>
    public class FetchMessageService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpClient">-</param>
        public FetchMessageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Fetch messages from the inbox using the given onboarding response.
        /// </summary>
        /// <param name="onboardingResponse">All the messages that are in the inbox.</param>
        /// <returns>-</returns>
        /// <exception cref="CouldNotFetchMessagesException">Will be thrown if the messages can not be fetched.</exception>
        public List<MessageResponse> Fetch(OnboardingResponse onboardingResponse)
        {
            Log.Debug("Begin fetching messages.");
            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(onboardingResponse.ConnectionCriteria.Commands)
            };
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            var httpResponseMessage = _httpClient
                .SendAsync(httpRequestMessage).Result;
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var messageResponses =
                    JsonConvert.DeserializeObject<List<MessageResponse>>(httpResponseMessage.Content.ReadAsStringAsync()
                        .Result);
                Log.Debug("Finished fetching messages.");
                return messageResponses;
            }

            throw new CouldNotFetchMessagesException(httpResponseMessage.StatusCode,
                httpResponseMessage.Content.ReadAsStringAsync().Result);
        }
    }
}