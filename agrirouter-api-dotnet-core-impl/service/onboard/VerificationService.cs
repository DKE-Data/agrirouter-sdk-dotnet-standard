using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using com.dke.data.agrirouter.api.dto.onboard;
using com.dke.data.agrirouter.api.logging;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.impl.service.common;
using Newtonsoft.Json;
using Environment = com.dke.data.agrirouter.api.env.Environment;

namespace com.dke.data.agrirouter.impl.service.onboard
{
    public class VerificationService
    {
        private readonly Environment _environment;
        private readonly UtcDataService _utcDataService;

        public VerificationService(Environment environment)
        {
            _environment = environment;
            _utcDataService = new UtcDataService();
        }
        
        public bool Verify(VerificationParameters verificationParameters)
        {
            var onboardingRequest = new VerificationRequest()
            {
                Id = verificationParameters.Uuid,
                ApplicationId = verificationParameters.ApplicationId,
                CertificationVersionId = verificationParameters.CertificationVersionId,
                GatewayId = verificationParameters.GatewayId,
                CertificateType = verificationParameters.CertificationType,
                TimeZone = _utcDataService.TimeZone,
                UTCTimestamp = _utcDataService.Now
            };

            var jsonContent = JsonConvert.SerializeObject(onboardingRequest);
            var requestBody = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var httpClient = new HttpClient(new LoggingHandler(new HttpClientHandler()));
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", verificationParameters.RegistrationCode);

            var httpResponseMessage = httpClient.PostAsync(_environment.OnboardUrl(), requestBody).Result;

            throw new NotImplementedException("Still to come...");
        }
    }
}