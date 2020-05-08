using System;
using System.Collections.Generic;
using System.Threading;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Api.Test.Helper;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Request.Payload.Endpoint;
using Newtonsoft.Json;
using Xunit;

namespace Agrirouter.Api.Test.Service.Messaging.Http
{
    /// <summary>
    ///     Functional tests.
    /// </summary>
    [Collection("Integrationtest")]
    public class PemAndP12MessagingTest : AbstractIntegrationTest
    {
        private static void RunWith(OnboardResponse onboardResponse)
        {
            var httpClient = HttpClientFactory.AuthenticatedHttpClient(onboardResponse);
            var capabilitiesServices =
                new CapabilitiesService(new HttpMessagingService(httpClient));
            var capabilitiesParameters = new CapabilitiesParameters
            {
                OnboardResponse = onboardResponse,
                ApplicationId = ApplicationId,
                CertificationVersionId = CertificationVersionId,
                EnablePushNotifications = CapabilitySpecification.Types.PushNotification.Disabled,
                CapabilityParameters = new List<CapabilityParameter>()
            };

            var capabilitiesParameter = new CapabilityParameter
            {
                Direction = CapabilitySpecification.Types.Direction.SendReceive,
                TechnicalMessageType = TechnicalMessageTypes.Iso11783TaskdataZip
            };

            capabilitiesParameters.CapabilityParameters.Add(capabilitiesParameter);
            capabilitiesServices.Send(capabilitiesParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(httpClient);
            var fetch = fetchMessageService.Fetch(onboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
        }

        private static OnboardResponse OnboardResponseWithP12Certificate
        {
            get
            {
                const string onboardingResponseAsJson =
                    "{\"deviceAlternateId\":\"b2806337-0ad0-4df2-9b69-d85b5302a5f8\",\"capabilityAlternateId\":\"c2467f6d-0a7e-48ca-9b57-1862186aef12\",\"sensorAlternateId\":\"798fae43-defa-4ef1-930a-b3bfe50eb5a5\",\"connectionCriteria\":{\"gatewayId\":\"3\",\"measures\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/measures/b2806337-0ad0-4df2-9b69-d85b5302a5f8\",\"commands\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/commands/b2806337-0ad0-4df2-9b69-d85b5302a5f8\"},\"authentication\":{\"type\":\"P12\",\"secret\":\"cyva4MsqSpJCNOFBsYmEpiZAvJlVa639PLEt\",\"certificate\":\"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\"}}\n";
                var onboardingResponse =
                    JsonConvert.DeserializeObject(onboardingResponseAsJson, typeof(OnboardResponse));
                return onboardingResponse as OnboardResponse;
            }
        }

        private static OnboardResponse OnboardResponseWithPemCertificate
        {
            get
            {
                const string onboardingResponseAsJson =
                    "{\"deviceAlternateId\":\"d3d765d5-e23e-4345-a9a3-b3e093262749\",\"capabilityAlternateId\":\"c2467f6d-0a7e-48ca-9b57-1862186aef12\",\"sensorAlternateId\":\"45748507-23c8-4402-8b22-dba227a0d1a0\",\"connectionCriteria\":{\"gatewayId\":\"3\",\"measures\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/measures/d3d765d5-e23e-4345-a9a3-b3e093262749\",\"commands\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/commands/d3d765d5-e23e-4345-a9a3-b3e093262749\"},\"authentication\":{\"type\":\"PEM\",\"secret\":\"1TOh2VA1HnojF8xmF9cIWR6ZhoVJf7AWqIpC\",\"certificate\":\"-----BEGIN ENCRYPTED PRIVATE KEY-----\\nMIIE6zAdBgoqhkiG9w0BDAEDMA8ECFwhT7qMAQ1xAgMCAAAEggTIFVQ4CP6uoLqF\\n8poG5ApHcnWxbgTVjxPi2zdyFIFuWAArdY3G2tCGp6oCQpeGBpO+ydPVFm7GJiC8\\nT0UtiAWPirxVQO3wSUeNiOTcRb6xziyEP4ooewplhE9PDWe0P63ON3XqalP9zfcj\\nWqyq/IVvNrP34aIjnhzO/hsWGV3C4st8NfvUIdXCdbFq6Rt9uxlVYJ2K2JWxf44p\\nRE/UOPZQMk2pEPYQpmIQK9iOGoyDfgh79VuyXQUUbUngx2Sff5MeQLssUICXzauL\\nYfU5K7z+Q6lmzSl8YAi8oPJKswRd2RN2xZdZb/ov3wSuDeQaGrt4r97MIMK+m+1B\\nPCOz1QBi3DeD6Gxs2RIUrETfnVi8K1oOForTdPldNMsD9DlrhPhjXrzrcWqAh9jI\\nw0UaeUclHl51wfy++NHdrlnmG6yqi7t+PHWGmqTJEsJvzRQQPOAeH/VrtgL2Lmhp\\nxo2ZP6gTOWO+prlahLHmfLV8NET5eeB3m5IiS3VlMZMwzYP3n09GAHJnBLhyiHpK\\noa5haw+mmu3uxGbEyRMCD1v9q0TdRX6s8byAKgq84lljkcLTaqG7rZafglveVbX3\\nuKVUcTKIQeVnVrgKhhMik2odzezuFFzi7UMwIWDKjUd9NG3WaDSOiDL3sLQ5EXnR\\n2D9etznnUntQlsXZNFczl2/rke9n/xbFoQVUZweSm1kImNKqGs7BYGIoLx2qjC+y\\nvwdXsflkXEJhSdot/y1gEeXirBmZH7BeJah5OPe2G6G+swEaQcpLub1iHn8eVJel\\nmFek1J1j2OlJaGcZbF7B/y1aERiLm4+tpa0oRAkmasxY0wFbU1691JfIluq0bwv9\\nxRlFdedyPx+thEDGvhTtzfv2XaRi1cOW0oi5L1xHc8pXyIS6MOyfKetX+AYwhPnX\\ncN8OgqJmWSvrdYnrXCMqIDXIEujZ6mX2nA+kZLNS5oZQJqH0ZxpqmgOsMTkjVCOB\\nyOv3TjPDDXQpuiQ+2DlK9odeLkcaLiEzpBAdhqVtij23dreIvMlVqG1zlufDXH2n\\nT2YQCkNlEOhONoRZy1Nrh8y5kiHmvVT7b6PM77yjMfkk4xNUjKZ6/gPPxwo8fR46\\n434oT+3CBoy/k0cK9E/JNIUaYXZghU82nbnny7JCYWQFNE3ub7mXpbdJt0xyMMgI\\nbcSCKbV/EDR5nIgOvBVfZVMnDTauBfpFyMiQg0dEpzEOQbAgagXl8Kpcs8ku9D9u\\n0TmzwXon3L2Qz0fqLd/TO9Y8+TK2HOfmwTFboI7A5UYviaQn9J4un9qXRsHArpX6\\nc1RR0DWwaa7ZNN7MUyZeUVor8lakgDohRSr6F32qPHBsTo2x+k0ptmWCqgQMfJVm\\n5HEP4pUvSNDMEhzSNcQ44xoAaL4yhRqYRGFpEm83AvbGCaaQhTyZgik6epNsnjyp\\niZYb9YRNg8UIQOzJU2eR/TU5Z2rC1xOczQzXNP6COLPntSIEpR3N9ikGfH0OwaZy\\n/TgmNYbfpjVnbwmiVp85CLT4RktJMv3O/7clkHuhYoa9T8lynQ02LZRRT3zMGiQq\\n+w0+uu/7ErvNR8JmsnCDpyzSDIIXx5ttPHlnfOBpo11GwH8NMNAEXh5n5Kczm689\\nHpr0Q34wN9dv+yABfcAf\\n-----END ENCRYPTED PRIVATE KEY-----\\n-----BEGIN CERTIFICATE-----\\nMIIEZzCCA0+gAwIBAgIPAMbqjmzeqBYREAECkUx9MA0GCSqGSIb3DQEBCwUAMFYx\\nCzAJBgNVBAYTAkRFMSMwIQYDVQQKExpTQVAgSW9UIFRydXN0IENvbW11bml0eSBJ\\nSTEiMCAGA1UEAxMZU0FQIEludGVybmV0IG9mIFRoaW5ncyBDQTAeFw0yMDAxMTUx\\nNTUyNTBaFw0yMTAxMTUxNTUyNTBaMIG0MQswCQYDVQQGEwJERTEcMBoGA1UEChMT\\nU0FQIFRydXN0IENvbW11bml0eTEVMBMGA1UECxMMSW9UIFNlcnZpY2VzMXAwbgYD\\nVQQDFGdkZXZpY2VBbHRlcm5hdGVJZDpkM2Q3NjVkNS1lMjNlLTQzNDUtYTlhMy1i\\nM2UwOTMyNjI3NDl8Z2F0ZXdheUlkOjN8dGVuYW50SWQ6ODIxODg2MTk5fGluc3Rh\\nbmNlSWQ6ZGtlLXFhMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAluII\\nwo/6dNkyth+r9PK/0DTRqWeOWjXAWve9Q4TO+FHW/iMjXwyj2j7Q5rtRiAGksB+M\\nUl5p3xCwRb9NERcC9HlPZXYk+GZGadDyJCKA6vQLaZR7xKOFIjsNvxeDE2YzDmB5\\nIXPxmj0pmD1lTgryv6Xd0nIcK7dg2DGu+xujGwUXbyIqCRngh+deazzCeZX9j7OG\\nlSHH7tb3Rtq02KI5c6sjQjpFWTAkUij5E8MLxHRQLkrZPsZHZMaj3JNZQIvR/Z6x\\na1bp43SihaZu2zRYwip6Ydk3g1/OEn7olRHIW8eHWaXQgC1jdizxbnfxYFYwQv/a\\nyXUX62NoiBGWsW47UQIDAQABo4HSMIHPMEgGA1UdHwRBMD8wPaA7oDmGN2h0dHBz\\nOi8vdGNzLm15c2FwLmNvbS9jcmwvVHJ1c3RDb21tdW5pdHlJSS9TQVBJb1RDQS5j\\ncmwwDAYDVR0TAQH/BAIwADAlBgNVHRIEHjAchhpodHRwOi8vc2VydmljZS5zYXAu\\nY29tL1RDUzAOBgNVHQ8BAf8EBAMCBsAwHQYDVR0OBBYEFJbaI015jz0arj1xxqAJ\\nEJS9tdp0MB8GA1UdIwQYMBaAFJW3s/VY3tW0s1hG4PKmyXhOvS11MA0GCSqGSIb3\\nDQEBCwUAA4IBAQBL5bZnt+x229RJnxwacYstYGeg3dF016tFY+zQ96j0/P/gKUUo\\nKqSnIv/E2cI8JmqfRrSK1szxvwrCGFpX6HWGioPB8LVoDspDbUrSYHbY+4k0QBE2\\n+6hKuSD5C78Xce2zQs677JCcDNq5khI4Cv9CEOfVlrfR368LyQUiokrRQmr5fKqp\\nMP5Emk4EnL8pygQhaAaA2HkPQxjSC/QYCMK0OY1nrZmMJrGlzb9hHUZY/pTzenQh\\nPD1lFxFQABTJBKB6wZZVbchkJMG03YkkQc+LySy3u7PegrN9QojdDYwOl29JhE6j\\noxSssR4i+OZq7PkYS9bBjydAeSBJEbGYl2rC\\n-----END CERTIFICATE-----\\n\"}}";
                var onboardingResponse =
                    JsonConvert.DeserializeObject(onboardingResponseAsJson, typeof(OnboardResponse));
                return onboardingResponse as OnboardResponse;
            }
        }

        [Fact]
        public void
            GivenValidCapabilitiesWhenSendingCapabilitiesMessageWithP12CertificateThenTheAgrirouterShouldSetTheCapabilities()
        {
            RunWith(OnboardResponseWithP12Certificate);
        }

        [Fact(Skip = "Due to PEM type this does currently not work.")]
        public void
            GivenValidCapabilitiesWhenSendingCapabilitiesMessageWithPemCertificateThenTheAgrirouterShouldSetTheCapabilities()
        {
            RunWith(OnboardResponseWithPemCertificate);
        }
    }
}