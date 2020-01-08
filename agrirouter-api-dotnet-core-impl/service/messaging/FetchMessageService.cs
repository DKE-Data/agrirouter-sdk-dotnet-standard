using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Mime;
using com.dke.data.agrirouter.api.dto.messaging;
using com.dke.data.agrirouter.api.dto.onboard;
using com.dke.data.agrirouter.api.exception;
using com.dke.data.agrirouter.impl.service.common;
using Newtonsoft.Json;
using Serilog;

namespace com.dke.data.agrirouter.impl.service.messaging
{
    /**
     * Service to fetch messages from the AR inbox.
     */
    public class FetchMessageService
    {
        private readonly HttpClientService _httpClientService;

        public FetchMessageService()
        {
            _httpClientService = new HttpClientService();
        }

        /**
         * Fetch messages from the inbox using the given onboarding response.
         */
        public List<MessageResponse> Fetch(OnboardingResponse onboardingResponse)
        {
            Log.Debug("Begin fetching messages.");
            var httpClient = _httpClientService.AuthenticatedHttpClient(onboardingResponse);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            var httpResponseMessage = httpClient
                .GetAsync(onboardingResponse.ConnectionCriteria.Commands).Result;
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var messageResponses =
                    JsonConvert.DeserializeObject<List<MessageResponse>>(httpResponseMessage.Content.ReadAsStringAsync()
                        .Result);
                Log.Debug("Finished fetching messages.");
                return messageResponses;
            }

            throw new CouldNotFetchMessagesException(httpResponseMessage.StatusCode, httpResponseMessage.Content.ReadAsStringAsync().Result);
        }
    }
}