using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Impl.Service.Common;
using Newtonsoft.Json;
using Xunit;

namespace Agrirouter.Api.Test.Service.Common
{
    public class X509CertificateServiceTest : AbstractIntegrationTest
    {

        [Fact]
        public void
            GivenValidOnboardingResponseWhenCreatingCertificateThenTheCertificateCreationShouldNotThrowAnyError()
        {
            var certificate = X509CertificateService.GetCertificate(OnboardResponse);
            Assert.NotNull(certificate);
        }
        
        private static OnboardResponse OnboardResponse
        {
            get
            {
                const string onboardingResponseAsJson =
                    "{\"deviceAlternateId\":\"68c1abbf-3675-4015-92e3-33b41893e5eb\",\"capabilityAlternateId\":\"c2467f6d-0a7e-48ca-9b57-1862186aef12\",\"sensorAlternateId\":\"ce45c0f7-6c23-481f-8f49-5e1a84cdf7fe\",\"connectionCriteria\":{\"gatewayId\":\"3\",\"measures\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/measures/68c1abbf-3675-4015-92e3-33b41893e5eb\",\"commands\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/commands/68c1abbf-3675-4015-92e3-33b41893e5eb\"},\"authentication\":{\"type\":\"PEM\",\"secret\":\"caWOIgat0vpA97KlXkefisLm00eh1G2weCU7\",\"certificate\":\"-----BEGIN ENCRYPTED PRIVATE KEY-----\\nMIIE6zAdBgoqhkiG9w0BDAEDMA8ECHApruoKTV+EAgMCAAAEggTIVHDDQo9JVCdB\\n8SuaTCjeKFVuTwcF0bD12DGQ39akTP65E4LtBVf3P3E4K0eSJ8onKO6x2mKdUKdM\\nX2ypLb8bjj2sDiYoTsqceRKdCy1SVt6dgp6Na5wxi9JelWLITwa1xdYqpoUxvPv9\\n/7IXHEVTo0s4wnLjnZlM7B85JjbW8Eeqqz/GmT6CoHLMXQ3EXVfSAy4w2iQVpRpj\\nmQfUwCk+2frvq36YQrpsL42PWsGyuSX5nOV1lLl70NTsRvFHCZmnmhTgx/WTVCWq\\ngiy7WBYKUpx/gdukUpVDoMiTI/p/82ttbF7nt/bsUegcSVKSLQ1QGllAR76Acw7w\\niqsmXil1R9orK74i3SnUfSCnvXdZKfiP0zVadHp3oAEY7A5mTHhemYhNrTnYAX+0\\nOsE2U1D21Jvb15Mk2GdkIZ/GIaal8bGP9EBgjiiMInAqcMOdmC33Qw4Q01J12tMY\\nFDWRo+44TfNLllpTMsPgPVf6umlJBP6vjOabryl1MPwkyvo19qG0BzmpRGEQMEUl\\nfxVU/nVPH/vnok9VjiBRqYZ9PRfi0VR//baohEWA437P0rMp/mKjjLZ2+PMM7CYf\\n8YAUKLDO3fQscmgsx1YTP1fs9pESYsdl3ZkgwD+bsL2e24H49bcEMOEDRfgv09N/\\nRXrvJAecxIyU/UqzQt4XMjLmo7p4A/rP2QFbU+og2YgYcFoF9hv7We/YSIJ1n976\\nadOm1pCtsTUPCse9JBHlX5JoYnabs/N7gBf+qE0xazJ9G5+/7RqeCOQscxi9C33d\\nV10x1952JvRk8CRnDUHPSs1VGXlNZpFrY8m7Di6FJP8asb9WWhXTUIPHEAw6mwil\\nTSyNIxzW4ZGAUszfOkdZwCR34RhC8H22gyaW1F3aARuh6a+UvSlQ7S6wghQIrCKG\\nf/ZhGy6+4QPsFE20vsnLnJWTd41tmTx7dLCY8T9+5HJu32Hj3oqgouOoTcESLRES\\n3ck9O3a47OL0V7dWMCDfBQOZuQZQf73t8VF0OpxBi3CZAS7lRu4WAwEbNrQ5LbUh\\nor1GB4xFdLwvgbAHL7EaIOe+gY0PjWhYQcUHeBXpD8EF9xyweBq60hWWaKa6km2O\\nS4NxfgN6FYNLolmQDxBTLoM/XOJtj9mmSwN/l9d1hdSuixCGT/xSYejnnGeYQR9s\\n7J15Yuyt1cT+0Goabx1Qm5jLqJ3XNvCb/RXgFySEzW7oYHwqiKkgvAKHXWH2giT2\\nyV2uoJI2D1vKfzmQ3a858KHmCZrDDC1ILmdmbpV5KCg80BLLAqf0alngnqVX1MZB\\nnu9O8OAgBwAqVngaqJcoAc9VA8gw/6qhShpdNgSu69e5dzxr5/VgRtT/8+L3wyq1\\nkiYZJEOOrVQq3OFI6t+wC7iHpfWdWBemqd8pjfKSz7CBY2wJ7A/c3oSXzuCW/vz8\\nBK6EsQ3If1CogphpqkAxicouE31scmOb2s6vJCZpkNAdeARLy/wyYkDp9k1T1cX9\\nNbS3W3w9IafO4GZ9joC+kz+2lHquI5rw0pYa3Yqpc2g6fOIEO02CLBNVTtg5nnES\\nJ996BP7J/gWaYV37hdGLMf0ExKYnYXGyHlZbc9od8KJQoWLTjcPe61EVw00WJTZL\\nweWd4bQvFCLLQ6lQgpcO\\n-----END ENCRYPTED PRIVATE KEY-----\\n-----BEGIN CERTIFICATE-----\\nMIIEZzCCA0+gAwIBAgIPAPghbDHc+WR8EAECw4WEMA0GCSqGSIb3DQEBCwUAMFYx\\nCzAJBgNVBAYTAkRFMSMwIQYDVQQKExpTQVAgSW9UIFRydXN0IENvbW11bml0eSBJ\\nSTEiMCAGA1UEAxMZU0FQIEludGVybmV0IG9mIFRoaW5ncyBDQTAeFw0yMDA1Mjcx\\nMzQyMDZaFw0yMTA1MjcxMzQyMDZaMIG0MQswCQYDVQQGEwJERTEcMBoGA1UEChMT\\nU0FQIFRydXN0IENvbW11bml0eTEVMBMGA1UECxMMSW9UIFNlcnZpY2VzMXAwbgYD\\nVQQDFGdkZXZpY2VBbHRlcm5hdGVJZDo2OGMxYWJiZi0zNjc1LTQwMTUtOTJlMy0z\\nM2I0MTg5M2U1ZWJ8Z2F0ZXdheUlkOjN8dGVuYW50SWQ6ODIxODg2MTk5fGluc3Rh\\nbmNlSWQ6ZGtlLXFhMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEApv7R\\nRv8BgqfhgfgivLsDT6/WYthD6j54QN3oIQM2m5jUd5OP3bcOnh64kxYq7mxVcz8h\\nkTLbogbYZ+Y9HutO//yK/+UVUxO97FljQieJC3WFRSNx9AMSr0J/FqFnkdMHy/It\\nK0z9P5r2KOCYVKMBFHjlcy+uUwIw1Iurxg7lSIZvchcUua/j1bEe+harohVw8mSu\\nsGVn6mJGAnorDgJkaa52PghvhzM/PUyyHqF4QrMzzATAtXD9eya/B4pgqPXJMIRq\\nTuHjMGvvvJCKfe+3Klj7JcSCwLY/as3miIvEdlqRVXbVGbzqVzxX2O1cOSgKu5hU\\nNDMBTJGtQ20hPP6ofwIDAQABo4HSMIHPMEgGA1UdHwRBMD8wPaA7oDmGN2h0dHBz\\nOi8vdGNzLm15c2FwLmNvbS9jcmwvVHJ1c3RDb21tdW5pdHlJSS9TQVBJb1RDQS5j\\ncmwwDAYDVR0TAQH/BAIwADAlBgNVHRIEHjAchhpodHRwOi8vc2VydmljZS5zYXAu\\nY29tL1RDUzAOBgNVHQ8BAf8EBAMCBsAwHQYDVR0OBBYEFB1GG5A7a0zccbROVgJd\\nSjSWq5sNMB8GA1UdIwQYMBaAFJW3s/VY3tW0s1hG4PKmyXhOvS11MA0GCSqGSIb3\\nDQEBCwUAA4IBAQCTFrXLpJtQ4IIpOeGNn3bHMt+weHbVffxjbf/7LpQ2T1DpGAcA\\n3IlGjydLljPytueBdcsI7Hjn1+/N2XYNxYpZSGXQlpKye2A31Kzq/xGA+AYanq0e\\n/7FLxP9IrdCejHzp23PiJd6ckHJEKM5o1Wcv9ZXjB0pr0yvkGZPOjXTlhngjHlu+\\nppM7lOBqyPlPE21V9YWkDvPrzCj18WeUhYiLsu+sMHwNIaYXft9LxZ+jXJ6FvNYJ\\neMLyIy2zvKhcESMnrlFoRc6LMABXm5vfNZXB2BZRbBvvilVhubonFX3Hn8FHOnAH\\nguGhrb6zAjXfWElo+nwcyGC5mNm6H+z8yA/t\\n-----END CERTIFICATE-----\\n\"}}";
                var onboardingResponse =
                    JsonConvert.DeserializeObject(onboardingResponseAsJson, typeof(OnboardResponse));
                return onboardingResponse as OnboardResponse;
            }
        }
    }
}