using System;
using System.Collections.Generic;
using System.Threading;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Api.Test.Data;
using Agrirouter.Api.Test.Helper;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Request.Payload.Endpoint;
using Xunit;

namespace Agrirouter.Api.Test.Data.OnboardingResponses.Http
{
    public class EnableAllCapabilitiesForOnboardingResponsesTest
    {
        [Fact]
        public void
            GivenValidCapabilitiesWhenSendingTheCapabilitiesThenTheAgrirouterShouldSetTheCapabilitiesForAllCommunicationUnits()
        {
            OnboardResponseIntegrationService.AllCommunicationUnits().ForEach(onboardResponse =>
            {
                var httpClient = HttpClientFactory.AuthenticatedHttpClient(onboardResponse);
                var capabilitiesServices =
                    new CapabilitiesService(new HttpMessagingService(httpClient));
                var capabilitiesParameters = new CapabilitiesParameters
                {
                    OnboardResponse = onboardResponse,
                    ApplicationId = Applications.CommunicationUnit.ApplicationId,
                    CertificationVersionId = Applications.CommunicationUnit.CertificationVersionId,
                    EnablePushNotifications = CapabilitySpecification.Types.PushNotification.Disabled,
                    CapabilityParameters = new List<CapabilityParameter>()
                };

                capabilitiesParameters.CapabilityParameters.AddRange(Capabilities);
                capabilitiesServices.Send(capabilitiesParameters);

                Thread.Sleep(TimeSpan.FromSeconds(5));

                var fetchMessageService = new FetchMessageService(httpClient);
                var fetch = fetchMessageService.Fetch(onboardResponse);
                Assert.Single(fetch);

                var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
                Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
            });
        }

        private static IEnumerable<CapabilityParameter> Capabilities
        {
            get
            {
                var all = new List<CapabilityParameter>();
                TechnicalMessageTypes.AllForCapabilitySetting().ForEach(technicalMessageType =>
                {
                    var capabilitiesParameter = new CapabilityParameter
                    {
                        Direction = CapabilitySpecification.Types.Direction.SendReceive,
                        TechnicalMessageType = technicalMessageType
                    };
                    all.Add(capabilitiesParameter);
                });
                return all;
            }
        }
    }
}