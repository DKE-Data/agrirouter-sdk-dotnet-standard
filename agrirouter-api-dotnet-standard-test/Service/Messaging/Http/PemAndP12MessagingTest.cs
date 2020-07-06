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
    /// Functional tests.
    /// </summary>
    [Collection("Integrationtest")]
    public class PemAndP12MessagingTest : AbstractIntegrationTest
    {
        [Fact]
        public void
            GivenValidCapabilitiesWhenSendingCapabilitiesMessageWithP12CertificateThenTheAgrirouterShouldSetTheCapabilities()
        {
            RunWith(OnboardResponseWithP12Certificate);
        }

        [Fact]
        public void
            GivenValidCapabilitiesWhenSendingCapabilitiesMessageWithPemCertificateThenTheAgrirouterShouldSetTheCapabilities()
        {
            RunWith(OnboardResponseWithPemCertificate);
        }

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
                const string onboardingResponseAsJson = "{\"deviceAlternateId\":\"68c1abbf-3675-4015-92e3-33b41893e5eb\",\"capabilityAlternateId\":\"c2467f6d-0a7e-48ca-9b57-1862186aef12\",\"sensorAlternateId\":\"ce45c0f7-6c23-481f-8f49-5e1a84cdf7fe\",\"connectionCriteria\":{\"gatewayId\":\"3\",\"measures\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/measures/68c1abbf-3675-4015-92e3-33b41893e5eb\",\"commands\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/commands/68c1abbf-3675-4015-92e3-33b41893e5eb\"},\"authentication\":{\"type\":\"PEM\",\"secret\":\"caWOIgat0vpA97KlXkefisLm00eh1G2weCU7\",\"certificate\":\"-----BEGIN ENCRYPTED PRIVATE KEY-----\\nMIIE6zAdBgoqhkiG9w0BDAEDMA8ECHApruoKTV+EAgMCAAAEggTIVHDDQo9JVCdB\\n8SuaTCjeKFVuTwcF0bD12DGQ39akTP65E4LtBVf3P3E4K0eSJ8onKO6x2mKdUKdM\\nX2ypLb8bjj2sDiYoTsqceRKdCy1SVt6dgp6Na5wxi9JelWLITwa1xdYqpoUxvPv9\\n/7IXHEVTo0s4wnLjnZlM7B85JjbW8Eeqqz/GmT6CoHLMXQ3EXVfSAy4w2iQVpRpj\\nmQfUwCk+2frvq36YQrpsL42PWsGyuSX5nOV1lLl70NTsRvFHCZmnmhTgx/WTVCWq\\ngiy7WBYKUpx/gdukUpVDoMiTI/p/82ttbF7nt/bsUegcSVKSLQ1QGllAR76Acw7w\\niqsmXil1R9orK74i3SnUfSCnvXdZKfiP0zVadHp3oAEY7A5mTHhemYhNrTnYAX+0\\nOsE2U1D21Jvb15Mk2GdkIZ/GIaal8bGP9EBgjiiMInAqcMOdmC33Qw4Q01J12tMY\\nFDWRo+44TfNLllpTMsPgPVf6umlJBP6vjOabryl1MPwkyvo19qG0BzmpRGEQMEUl\\nfxVU/nVPH/vnok9VjiBRqYZ9PRfi0VR//baohEWA437P0rMp/mKjjLZ2+PMM7CYf\\n8YAUKLDO3fQscmgsx1YTP1fs9pESYsdl3ZkgwD+bsL2e24H49bcEMOEDRfgv09N/\\nRXrvJAecxIyU/UqzQt4XMjLmo7p4A/rP2QFbU+og2YgYcFoF9hv7We/YSIJ1n976\\nadOm1pCtsTUPCse9JBHlX5JoYnabs/N7gBf+qE0xazJ9G5+/7RqeCOQscxi9C33d\\nV10x1952JvRk8CRnDUHPSs1VGXlNZpFrY8m7Di6FJP8asb9WWhXTUIPHEAw6mwil\\nTSyNIxzW4ZGAUszfOkdZwCR34RhC8H22gyaW1F3aARuh6a+UvSlQ7S6wghQIrCKG\\nf/ZhGy6+4QPsFE20vsnLnJWTd41tmTx7dLCY8T9+5HJu32Hj3oqgouOoTcESLRES\\n3ck9O3a47OL0V7dWMCDfBQOZuQZQf73t8VF0OpxBi3CZAS7lRu4WAwEbNrQ5LbUh\\nor1GB4xFdLwvgbAHL7EaIOe+gY0PjWhYQcUHeBXpD8EF9xyweBq60hWWaKa6km2O\\nS4NxfgN6FYNLolmQDxBTLoM/XOJtj9mmSwN/l9d1hdSuixCGT/xSYejnnGeYQR9s\\n7J15Yuyt1cT+0Goabx1Qm5jLqJ3XNvCb/RXgFySEzW7oYHwqiKkgvAKHXWH2giT2\\nyV2uoJI2D1vKfzmQ3a858KHmCZrDDC1ILmdmbpV5KCg80BLLAqf0alngnqVX1MZB\\nnu9O8OAgBwAqVngaqJcoAc9VA8gw/6qhShpdNgSu69e5dzxr5/VgRtT/8+L3wyq1\\nkiYZJEOOrVQq3OFI6t+wC7iHpfWdWBemqd8pjfKSz7CBY2wJ7A/c3oSXzuCW/vz8\\nBK6EsQ3If1CogphpqkAxicouE31scmOb2s6vJCZpkNAdeARLy/wyYkDp9k1T1cX9\\nNbS3W3w9IafO4GZ9joC+kz+2lHquI5rw0pYa3Yqpc2g6fOIEO02CLBNVTtg5nnES\\nJ996BP7J/gWaYV37hdGLMf0ExKYnYXGyHlZbc9od8KJQoWLTjcPe61EVw00WJTZL\\nweWd4bQvFCLLQ6lQgpcO\\n-----END ENCRYPTED PRIVATE KEY-----\\n-----BEGIN CERTIFICATE-----\\nMIIEZzCCA0+gAwIBAgIPAPghbDHc+WR8EAECw4WEMA0GCSqGSIb3DQEBCwUAMFYx\\nCzAJBgNVBAYTAkRFMSMwIQYDVQQKExpTQVAgSW9UIFRydXN0IENvbW11bml0eSBJ\\nSTEiMCAGA1UEAxMZU0FQIEludGVybmV0IG9mIFRoaW5ncyBDQTAeFw0yMDA1Mjcx\\nMzQyMDZaFw0yMTA1MjcxMzQyMDZaMIG0MQswCQYDVQQGEwJERTEcMBoGA1UEChMT\\nU0FQIFRydXN0IENvbW11bml0eTEVMBMGA1UECxMMSW9UIFNlcnZpY2VzMXAwbgYD\\nVQQDFGdkZXZpY2VBbHRlcm5hdGVJZDo2OGMxYWJiZi0zNjc1LTQwMTUtOTJlMy0z\\nM2I0MTg5M2U1ZWJ8Z2F0ZXdheUlkOjN8dGVuYW50SWQ6ODIxODg2MTk5fGluc3Rh\\nbmNlSWQ6ZGtlLXFhMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEApv7R\\nRv8BgqfhgfgivLsDT6/WYthD6j54QN3oIQM2m5jUd5OP3bcOnh64kxYq7mxVcz8h\\nkTLbogbYZ+Y9HutO//yK/+UVUxO97FljQieJC3WFRSNx9AMSr0J/FqFnkdMHy/It\\nK0z9P5r2KOCYVKMBFHjlcy+uUwIw1Iurxg7lSIZvchcUua/j1bEe+harohVw8mSu\\nsGVn6mJGAnorDgJkaa52PghvhzM/PUyyHqF4QrMzzATAtXD9eya/B4pgqPXJMIRq\\nTuHjMGvvvJCKfe+3Klj7JcSCwLY/as3miIvEdlqRVXbVGbzqVzxX2O1cOSgKu5hU\\nNDMBTJGtQ20hPP6ofwIDAQABo4HSMIHPMEgGA1UdHwRBMD8wPaA7oDmGN2h0dHBz\\nOi8vdGNzLm15c2FwLmNvbS9jcmwvVHJ1c3RDb21tdW5pdHlJSS9TQVBJb1RDQS5j\\ncmwwDAYDVR0TAQH/BAIwADAlBgNVHRIEHjAchhpodHRwOi8vc2VydmljZS5zYXAu\\nY29tL1RDUzAOBgNVHQ8BAf8EBAMCBsAwHQYDVR0OBBYEFB1GG5A7a0zccbROVgJd\\nSjSWq5sNMB8GA1UdIwQYMBaAFJW3s/VY3tW0s1hG4PKmyXhOvS11MA0GCSqGSIb3\\nDQEBCwUAA4IBAQCTFrXLpJtQ4IIpOeGNn3bHMt+weHbVffxjbf/7LpQ2T1DpGAcA\\n3IlGjydLljPytueBdcsI7Hjn1+/N2XYNxYpZSGXQlpKye2A31Kzq/xGA+AYanq0e\\n/7FLxP9IrdCejHzp23PiJd6ckHJEKM5o1Wcv9ZXjB0pr0yvkGZPOjXTlhngjHlu+\\nppM7lOBqyPlPE21V9YWkDvPrzCj18WeUhYiLsu+sMHwNIaYXft9LxZ+jXJ6FvNYJ\\neMLyIy2zvKhcESMnrlFoRc6LMABXm5vfNZXB2BZRbBvvilVhubonFX3Hn8FHOnAH\\nguGhrb6zAjXfWElo+nwcyGC5mNm6H+z8yA/t\\n-----END CERTIFICATE-----\\n\"}}";
                    var onboardingResponse =
                    JsonConvert.DeserializeObject(onboardingResponseAsJson, typeof(OnboardResponse));
                return onboardingResponse as OnboardResponse;
            }
        }
    }
}