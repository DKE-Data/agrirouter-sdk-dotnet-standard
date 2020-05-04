using System.Collections.Generic;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Request.Payload.Endpoint;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using Xunit;

namespace Agrirouter.Api.Test.Service.Messaging
{
    public class MqttCapabilitiesServiceTest : AbstractIntegrationTest
    {
        [Fact]
        public void GivenValidCapabilitiesWhenSendingCapabilitiesMessageThenTheAgrirouterShouldSetTheCapabilities()
        {
            var capabilitiesServices = new CapabilitiesService(new MqttMessagingService(MqttClient));
            var capabilitiesParameters = new CapabilitiesParameters
            {
                OnboardResponse = OnboardResponse,
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
        }

        private static MqttClient MqttClient
        {
            get
            {
                var factory = new MqttFactory();
                var mqttClient = factory.CreateMqttClient();
                return mqttClient as MqttClient;
            }
        }

        private static OnboardResponse OnboardResponse
        {
            get
            {
                const string onboardingResponseAsJson =
                    "{\"deviceAlternateId\":\"13530694-7997-4530-8d76-f58745a158ed\",\"capabilityAlternateId\":\"c2467f6d-0a7e-48ca-9b57-1862186aef12\",\"sensorAlternateId\":\"d6a5ccf9-7040-44bb-96f0-181614a7f3e9\",\"connectionCriteria\":{\"gatewayId\":\"2\",\"host\":\"dke-qa.eu10.cp.iot.sap\",\"port\":8883,\"clientId\":\"13530694-7997-4530-8d76-f58745a158ed\",\"measures\":\"measures/13530694-7997-4530-8d76-f58745a158ed\",\"commands\":\"commands/13530694-7997-4530-8d76-f58745a158ed\"},\"authentication\":{\"type\":\"PEM\",\"secret\":\"AL1qLzP8tfMeIaBW2mu33FqQkvkOIldrW0jS\",\"certificate\":\"-----BEGIN ENCRYPTED PRIVATE KEY-----\\nMIIE6zAdBgoqhkiG9w0BDAEDMA8ECDZeCurbPqtyAgMCAAAEggTIpV3v+ZrIjUsK\\nHGzrAUCknwzU0SeafaVZx9A7Oo+SIdP/rrElC9mAdaQA+Il94LfyQY9ICaw0K1aR\\nMHDCKRVFEydbLVeFqqxDGsdVe8ezRMSnfM+uPlryDasKJYHPJheydKFF90NocoHc\\nzRQTDfgwOaHe994KMT5vBrLcXUlHkjV6wg7pwyAdqUAR/Fiz/M3KvJPMaNRWe3hl\\nAR2vk0U0SFX+vI6QSfnuuqcUO6P2zNpo+4rQYe4PyN/f+u76eITVMuX3D30Gma31\\n6EWv/kJYX0n96n6EmYfjv5qii+P66XZOUyJMgwxvca2g2CWvPC3Nze8xZ5UVia5z\\nIBFtet2bSDQSKLHBIBTwVilz9fabnkd6fEtmgJL9Hg1wqRIsshF4gmEUM0HDicFt\\nsenDYcSNSUmtfihN6A8gS/oX27LF/KFDjNvcX0MAaMJFEmQAwhBZT14f+EH38jju\\ny5yNuneEPNVDuTS1h19E1EmB0mhGb4di/7obgW0lJQ085+9DIJjO2FolPs0CHFdF\\nZXIubtfXytByjbMBMlbX2TqMHlxe/meFPzoQm4lzL4QNVbkc6vdFuXNI3B5OkDI4\\nblF4e2+kNkjKpJ/Gb0vwNFy/HLcjuxYhczT7l9SsvdTHPwaLuiqEzAw/PgBKkrK8\\nYXs6TJ2zZkx02hrtF0jm9n5opOANKKN/jZXciJGRQOtDAJr/E+wf5SnS7a4FD10W\\nH13Pb0K5dM6cYfMoL7iUdzXO7odEG6eANWr6JCC+vUnNeshOlXZW8XjjVCEgbz1o\\nUFAQ70xGt2hZk6cAj2mciaShjBBRvxga9H+DC+havPuIWD43iq6xrxumOyNffo0r\\nD/V4ohau+SuhA/rqsM4lFNukQyQnQB+pzy4NBX4pZ/w/iTpPuYp+171a4M9/Aapd\\n+njCRh6uI6P+vPTQbvTreqGi2YoKS3XvQk/ekQduv9r+e6PdeBwvzIDmZYdPMJVJ\\no+0izGZBUrb5lDuYUbsDoCq/AXLPg+W1LZsg3UPwQB1DL0x9btlsEWCeM2B6QlUo\\nzp6Ji8iAC1I6C/xOu08WGQKqUVrCBvPSCk8nMcNc9YPZ1gTJTdXW/Ou3H7EDwQMc\\nS25xeM65HpOb2xftt8aWkqwfKWBkG7/S8d/kMjAYC1USA8Lc6QArf+kFAXuBudxd\\nL3GxMP+ZcfampnBogSut+BTg/PEARU1WpLRNLHReHK6t2LBI1jHiNfx0J738YHGN\\nZqW5L9CJ+r/sNTjXvCMLl/yb0/eLoLNCwXmVBrpSO39Bx/LrxG/aGjkYjloba9Bm\\nXyrQkGfJ5UTTmqRLXMYTrKQyMXV6hiPeVWgG4tD20nesSG8jWqTC6n/COQxgr+F6\\nxZhwkEU9nNzHA8wbQP7lcF1zj3R+fLi71hKhFxsHJffChQbh6czNTOCx9EiAbJoO\\nB2RIMbkP1CsG/64/7PjhRTF/BU4hoVMekIai7GqAux8xhE1LwMT04FISgnseTCTb\\n/QDouG/+HIgB6RZ5ZCrIeVseVeYNMRNqUHksUhBJkSFb8YC8YWopUlN4Tbx42Ymp\\nNUgToj0TzzkmqkoXwEaINNEyACK4MdcSJQzPlfU92tt2pxyr0BR4177PUdocooWG\\nar0g/AqzwjGe+kxBwYHl\\n-----END ENCRYPTED PRIVATE KEY-----\\n-----BEGIN CERTIFICATE-----\\nMIIEZzCCA0+gAwIBAgIPALWIIdvFWfMXEAECuNJNMA0GCSqGSIb3DQEBCwUAMFYx\\nCzAJBgNVBAYTAkRFMSMwIQYDVQQKExpTQVAgSW9UIFRydXN0IENvbW11bml0eSBJ\\nSTEiMCAGA1UEAxMZU0FQIEludGVybmV0IG9mIFRoaW5ncyBDQTAeFw0yMDA0Mjgx\\nMTQ2MjFaFw0yMTA0MjgxMTQ2MjFaMIG0MQswCQYDVQQGEwJERTEcMBoGA1UEChMT\\nU0FQIFRydXN0IENvbW11bml0eTEVMBMGA1UECxMMSW9UIFNlcnZpY2VzMXAwbgYD\\nVQQDFGdkZXZpY2VBbHRlcm5hdGVJZDoxMzUzMDY5NC03OTk3LTQ1MzAtOGQ3Ni1m\\nNTg3NDVhMTU4ZWR8Z2F0ZXdheUlkOjJ8dGVuYW50SWQ6ODIxODg2MTk5fGluc3Rh\\nbmNlSWQ6ZGtlLXFhMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgRG9\\npRWg20qSuo4TB9wzBnCscvhOW922sgaB1ja+0FWBkPC35VK96mbofBMoEMlHOmtk\\nXTRH7Fm3QxDCYvfz/jGS8tRhG3aAXuUY+XfU5rYobJ12IKqiinfToZrivZSfvnTK\\nwyoN2VxXclaPP1wwDot4/8ab/sXfx0Oc3/PUrx5ohc4byrjEfYWriWcxEnxoLzZ1\\nPOmMHjU5sAaBFjpjDex/oCPMInlMGTduWnnjzWHT3QMvXcpEjyBk6aGfZlxNGjsp\\nVEZP4e495yHVIT+Rnhkbj/VIV+9YqzIfIszH2fhy7Y6jvq7hURpDxldA5jTxaGA4\\nKfJwfTGU+ic0zq3PKwIDAQABo4HSMIHPMEgGA1UdHwRBMD8wPaA7oDmGN2h0dHBz\\nOi8vdGNzLm15c2FwLmNvbS9jcmwvVHJ1c3RDb21tdW5pdHlJSS9TQVBJb1RDQS5j\\ncmwwDAYDVR0TAQH/BAIwADAlBgNVHRIEHjAchhpodHRwOi8vc2VydmljZS5zYXAu\\nY29tL1RDUzAOBgNVHQ8BAf8EBAMCBsAwHQYDVR0OBBYEFFWBVvVqRLZu6Cj6hFmM\\nMj3bv+r6MB8GA1UdIwQYMBaAFJW3s/VY3tW0s1hG4PKmyXhOvS11MA0GCSqGSIb3\\nDQEBCwUAA4IBAQB34+HzdAQBAl80JlXze1K2Xf98Ikn/UmZbgBCWz+wKDelfKgH9\\n/rXxhs7uQ8QpWgItgq1TuC6GFtOQi1dY7xTzL4eoUf334YzgxgcDW8Veb3MnTboJ\\nyK5AXWxwYsKb+v5Rjfsj5geGoMaF7f4R1myiOw97nqcK3MDIjtVb/Tbi/HL6wOWl\\nSv52UcbU+jMlMMyXvlECNKNe5X2xQitn4qitkwepI9a6UmrSkv/DakBCyFOsvXEF\\nL0JsiIdAhMkdWOQgBCmufp+mzdXlZXQ1Z8O85ShYOsOHsLpLXT3lZ4scnKCqBg+w\\nDovJQJrDjtrM273IfsTHxuoAepKwK8DU0GZW\\n-----END CERTIFICATE-----\\n\"}}";
                var onboardingResponse =
                    JsonConvert.DeserializeObject(onboardingResponseAsJson, typeof(OnboardResponse));
                return onboardingResponse as OnboardResponse;
            }
        }
    }
}