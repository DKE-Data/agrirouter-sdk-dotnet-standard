using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.test.helper;
using Agrirouter.Cloud.Registration;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.messaging;
using Agrirouter.Impl.Service.Messaging.Vcu;
using Newtonsoft.Json;
using Xunit;

namespace Agrirouter.Api.Test.Service.Messaging.vcu
{
    /// <summary>
    /// Functional tests.
    /// </summary>
    public class OnboardVcuServiceTest : AbstractSecuredIntegrationTest
    {
        private static readonly HttpClient HttpClient = HttpClientFactory.AuthenticatedHttpClient(OnboardResponse);

        [Fact]
        public void GivenValidIdAndNameWhenOnboardingVirtualCuThenTheOnbardingShouldBePossible()
        {
            var onboardVcuService = new OnboardVcuService(new MessagingService(HttpClient), new EncodeMessageService());
            var onboardVcuParameters = new OnboardVcuParameters
            {
                OnboardResponse = OnboardResponse,
                ApplicationMessageId = MessageIdService.ApplicationMessageId(),
                OnboardingRequests = new List<OnboardingRequest.Types.EndpointRegistrationDetails>
                {
                    new OnboardingRequest.Types.EndpointRegistrationDetails
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "My first virtual CU..."
                    }
                }
            };
            var messageResponse = onboardVcuService.Send(onboardVcuParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodeMessageService = new DecodeMessageService();
            var decodedMessage = decodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }

        private static OnboardResponse OnboardResponse
        {
            get
            {
                var onboardingResponseAsJson =
                    "{\"deviceAlternateId\":\"7d3838b9-b772-4bb2-8c7f-c5626eec641c\",\"capabilityAlternateId\":\"c2467f6d-0a7e-48ca-9b57-1862186aef12\",\"sensorAlternateId\":\"36c2e048-aca7-45ca-9103-dd5332904ca1\",\"connectionCriteria\":{\"gatewayId\":\"3\",\"measures\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/measures/7d3838b9-b772-4bb2-8c7f-c5626eec641c\",\"commands\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/commands/7d3838b9-b772-4bb2-8c7f-c5626eec641c\"},\"authentication\":{\"type\":\"P12\",\"secret\":\"C4vJcxylvQrTJKqLrEFRsvLcMZhzvzpj7a3S\",\"certificate\":\"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\"}}";
                var onboardingResponse =
                    JsonConvert.DeserializeObject(onboardingResponseAsJson, typeof(OnboardResponse));
                return onboardingResponse as OnboardResponse;
            }
        }
    }
}