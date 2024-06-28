using System;
using System.Collections.Generic;
using System.Net.Http;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Env;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Feed.Request;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Impl.Service.Onboard;
using Agrirouter.Request;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Test.Data;
using Agrirouter.Test.Helper;
using Agrirouter.Test.Service;
using Efdi;
using Google.Protobuf;
using Xunit;

namespace Agrirouter.Test.Integration
{
    public class
        UpdateCertificationVersionIdOfExistingEndpoint : AbstractIntegrationTestForCommunicationUnits
    {

        private static readonly UtcDataService UtcDataService = new UtcDataService();
        private static readonly HttpClient HttpClient = HttpClientFactory.HttpClient();

        [Fact]
        public void OnboardingAndOldVersionAndSendingCapabilitiesWithNewVersionShouldUpdateEndpoint()
        {
            var onboardService = new OnboardService(new ProductionEnvironment(), UtcDataService, HttpClient);

            var applicationId = "4639d04f-7031-4836-9240-e241d84d4513";
            var oldVersionId = "f66772a0-4715-428a-841e-5e63011c3f62"; // Version 1 of this app
            var newVersionId = "50c84baa-ed1e-4afe-9bfe-c8d8ff890188";


            // [1] Onboard new endpoint with _old_ version id
            var parameters = new OnboardParameters
            {
                Uuid = Guid.NewGuid().ToString(),
                ApplicationId = applicationId,
                ApplicationType = ApplicationTypeDefinitions.Application,
                CertificationType = CertificationTypeDefinition.P12,
                GatewayId = GatewayTypeDefinition.Http,
                RegistrationCode = "606438468f",
                CertificationVersionId = oldVersionId
            };

            var onboardResponse = onboardService.Onboard(parameters);
            var authenticatedHttpClient = HttpClientFactory.AuthenticatedHttpClient(onboardResponse);

            // [2] send capabilities (only taskdata sending) with _old_ version id
            var capabilitiesServices =
                new CapabilitiesService(new HttpMessagingService(authenticatedHttpClient));
            var capabilitiesParameters = new CapabilitiesParameters
            {
                OnboardResponse = onboardResponse,
                ApplicationId = applicationId,
                CertificationVersionId = oldVersionId,
                EnablePushNotifications = CapabilitySpecification.Types.PushNotification.Disabled,
                CapabilityParameters = new List<CapabilityParameter>()
            };

            capabilitiesParameters.CapabilityParameters.Add(
                new CapabilityParameter()
                {
                    Direction = CapabilitySpecification.Types.Direction.Send,
                    TechnicalMessageType = TechnicalMessageTypes.Iso11783TaskdataZip
                });

            capabilitiesServices.Send(capabilitiesParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();
            
            // [2b] Reonboard endpoint
            parameters.RegistrationCode = "09a6b792ce";
            parameters.CertificationVersionId = newVersionId;

            onboardResponse = onboardService.Onboard(parameters);
            authenticatedHttpClient = HttpClientFactory.AuthenticatedHttpClient(onboardResponse);


            // [3] send capabilities (additionally telemetry) with _new_ version id
            capabilitiesParameters.CertificationVersionId = newVersionId;
            capabilitiesParameters.CapabilityParameters.Add(                
                new CapabilityParameter()
                {
                    Direction = CapabilitySpecification.Types.Direction.Send,
                    TechnicalMessageType = TechnicalMessageTypes.Iso11783DeviceDescriptionProtobuf
                });
            capabilitiesParameters.CapabilityParameters.Add(                
                new CapabilityParameter()
                {
                    Direction = CapabilitySpecification.Types.Direction.Send,
                    TechnicalMessageType = TechnicalMessageTypes.Iso11783TimeLogProtobuf
                });
            
            capabilitiesServices.Send(capabilitiesParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();

            // [4] send any other message (feed delete in this case) to have some metrics to compare against
            var feedDeleteService = new FeedDeleteService(new HttpMessagingService(authenticatedHttpClient));
            var feedDeleteParameters = new FeedDeleteParameters
            {
                OnboardResponse = onboardResponse,
                ValidityPeriod = new ValidityPeriod
                {
                    SentFrom = UtcDataService.Timestamp(TimestampOffset.FourWeeks),
                    SentTo = UtcDataService.Timestamp(TimestampOffset.None)
                }
            };
            feedDeleteService.Send(feedDeleteParameters);

            Timer.WaitForTheAgrirouterToProcessTheMessage();


        }
    }
}