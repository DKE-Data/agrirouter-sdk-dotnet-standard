using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Dto.Onboard.Inner;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.Messaging;
using Agrirouter.Request.Payload.Endpoint;
using Xunit;

namespace Agrirouter.Test.Integration
{
    public class LilCode
    {
        static string _applicationId = "8e8b7c05-e6e5-44ad-a406-ecd12c7ce8cc";
        static string _certificationVersionId = "4e8b51da-3e80-42db-a9be-0182062c04c0";

        [Fact]
        static async Task Main()
        {
            var onboarding = new OnboardResponse()
            {
                DeviceAlternateId = "cb681d9a-5434-4541-bbcf-eed50f0eb279",
                CapabilityAlternateId = "0e0dcefa-1ada-47f9-a871-7af4eb925561",
                SensorAlternateId = "5281c523-c23f-47d1-aef1-f4ec0743a01e",
                ConnectionCriteria = new ConnectionCriteria()
                {
                    Measures =
                        "https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/measures/cb681d9a-5434-4541-bbcf-eed50f0eb279",
                    Commands =
                        "https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/commands/cb681d9a-5434-4541-bbcf-eed50f0eb279",
                    GatewayId = "3",
                    Host = null,
                    Port = null,
                    ClientId = null
                },
                Authentication = new Authentication()
                {
                    Type = "PEM",
                    Secret = "ViDdDvb?!?oEHj?78#K4UY9tFP7xLhYNuGJk",
                    Certificate =
                        "-----BEGIN ENCRYPTED PRIVATE KEY-----\nMIIE6zAdBgoqhkiG9w0BDAEDMA8ECMU7MUwehUXFAgMCAAAEggTIylch3FMPEyRP\n2aaQNpdetf9XmT3he+NYAblqPzkHPk0zC6jaeQTi/r/CS5hVcmHmuhcs/2YxGHzZ\nnQEbt6VDD++m9dC5NQmte6Q43X1RIquZufSpoCzlwfZE21btiIX/Ft2JQT6DufL3\nxJQ3857zA3Ok1ehKxlZFhLeqSCSrocPfYEHKqg4LT4jxmg3Kebi0zVb66V+bKZ9i\nTfUqSLp9S0dr3pbzsROSkh2bWejhEdwQYM9J6xvU0Mq7aVvBMLyIn6yog0somS12\n7gE6mKLU8jALjP59flTkJEHXMyMcRE0Zx0oPRYvqPHYUviXzh+4pCc6+cjcAYEKh\nUjqdRSsdhLYcUT4wfV3KvDmR7kVwuGOUGe28K1dZ/K5Nox1b/7AQIHq/fLEpJlvO\nOqN5IJ0nXQ9gj6bWJguNBFwUc7+/ey7YvamEpiUwLf8EGYbgZSEyPFMhZGe7770n\netjZeDZTwAMBfOTXVmqFwibhZ1MwEM5giBxZ+MgzdCFDkWZzYPhbj+MTRSsjnL8I\nzjW9F1H41jh8XwUuKT8CQF1WSZBFzWY3D85J6SMg99FUFSDOal4+QD+gDEfBPHKY\neULplBFFGmfJfFPrUL4aOknwEh8AvQAw++bpqkfjqmwoEE9rL1sf+E4mLIBSuSM+\nU/cuGxSbD4yA5vlxB5unGjyPs/M7O9fFwpvM44C2EoF41L9c7jkFBDLkzN0Cnn91\nkGVxgdB2h2lOfESV5SAvno3UZ/MvBjF8sQjSN3DdONsLAY5G4dPVorQsAt4GmFDE\n17kHrd6yCoWb/uYHLQsGybhkSgw3k1L/zns9NAFij0Tt3qMR6p8h9JFXb/qbV12k\n23dWhS1UJx5UNqrJ47KtX9eZgUPXeFQVqPL1EfY8T4kNDHP/S5lHeEt8HT+aEosc\nrVfCENRplZ9QOF+I102M91INzzFF1FYSsH2nnw8m0kjQzwvo3PsfHKe87xMNYHp+\n2eVvzNMks3/9tY622QQDDIZ1OCo1y9YnF1IAXVSgBGpYE6E1ya6b2GhPsnuJBS9g\nGr64p6+OQ6aQJtrUb4s4NLyiba/cnIAHYJg91ig4vYWCmFSr5+74ywaQLsds3ZIu\nIn5yMYEkixYue4re7/bBlG6EensVF4ducrI1mSPQV3bn6rb1/QKUgXcZL7ZqmiTb\nHItI2rDGmRfHyIymr+5gSrQk7+o3oKrR7gescQEkUMzlCMAEX1VlAa1NIzNkRplV\niH6H14UDJuVl1qZx17bxw+I2rGqDk4g6PzATR2vSFRk9ML3aDd85i7FYM5vBTEMO\nzkogWy3quCxkPxtf6kAb7sQh+ji21X08uE4yXewdzBQkp2wBWrU7gbk7c5LhyOLf\ncZC3hKQZlXGEHey2C4ibhq8el20D8geLdncunYFWgR9uA5O7w6ZF/ycViB3h2E+f\nD9sgS4p4f0cKs3hqf+ZqrfHrLGTDb0eKF5ZWko/448F8TeL5Yk1hI6rfjEmVG22F\nnL+589+XNsstnfi7YpcYKnUJHNr7d6NNMYc+ZhUbzf+YzNA+JoGmZQjhBIFkT7Y7\nOc6WIoK2ALJDGvjXiIZZFEwDS1S+zkXCsM2rygAzE1ivd5oRc4akrEvCOVoAvCgQ\n1ElBDqcucVozGFVMub7a\n-----END ENCRYPTED PRIVATE KEY-----\n-----BEGIN CERTIFICATE-----\nMIIFejCCA2KgAwIBAgIRAI8P959z+evCuRuErxtCh6UwDQYJKoZIhvcNAQELBQAw\ngYIxCzAJBgNVBAYTAkRFMRUwEwYDVQQIDAxMb3dlciBTYXhvbnkxHzAdBgNVBAoM\nFkRLRS1EYXRhIEdtYkggJiBDby4gS0cxEzARBgNVBAcMCk9zbmFicnVlY2sxJjAk\nBgNVBAMMHWFncmlyb3V0ZXIgU0FQIElvVCBJc3N1aW5nIENBMB4XDTIyMDcxMTEy\nNTQyNVoXDTMyMDcwODEzNTQyNVowgaExCzAJBgNVBAYTAkRFMR8wHQYDVQQKDBZE\nS0UtRGF0YSBHbWJIICYgQ28uIEtHMXEwbwYDVQQDDGhkZXZpY2VBbHRlcm5hdGVJ\nZDpjYjY4MWQ5YS01NDM0LTQ1NDEtYmJjZi1lZWQ1MGYwZWIyNzl8Z2F0ZXdheUlk\nOjN8dGVuYW50SWQ6MTEwODM2NzA2OHxpbnN0YW5jZUlkOmRrZS1xYTCCASIwDQYJ\nKoZIhvcNAQEBBQADggEPADCCAQoCggEBAIzMDb63/0+Gl8BjvgS1h+71xSiMnP0v\n/mj16+Z8q8zscdREe5+lveFcpmyCP5sgjhw1sBsReArTsXKZZsdNyue9X8B0/GtB\no3X/NT9PI62+AUXuvDb0irHDygLfrKRbd25MdkC+vu5PfpL2tHXjnfWmWJ4Ff8lK\n9BhWnWWvAw1f3/IEaziBo8gEUYoZK2GA1S8RCulEBuEx5zMgosnAFJFmDpcwMBNh\n/zdDEBjzhM1mFGOIJuA9W7FbYL8gce4qpHl0tlbFo6/bfBhrmlB+PgwEDlZj/y58\nf9v+yEptHE/88mLDb8e6Ey6xnceCtMMHmE/ZQYSOKWaAo9aFfCMUGc8CAwEAAaOB\nyTCBxjAJBgNVHRMEAjAAMB8GA1UdIwQYMBaAFL0j/JLMFeGQZftrXKoTPyUIMWT2\nMB0GA1UdDgQWBBRqQvIeTAZcvkXa2+oP8paVoiVmWjAOBgNVHQ8BAf8EBAMCBaAw\nHQYDVR0lBBYwFAYIKwYBBQUHAwEGCCsGAQUFBwMCMEoGCCsGAQUFBwEBBD4wPDA6\nBggrBgEFBQcwAYYuaHR0cDovL29jc3AuYWNtLXBjYS5ldS1jZW50cmFsLTEuYW1h\nem9uYXdzLmNvbTANBgkqhkiG9w0BAQsFAAOCAgEAI+ODOkbZP2OpKkV/ZJNfwLSV\nmeOgyOcukAqU6XCVyQV66QbQVX/yyoKEq/sJI3yLbDQtiRrq/YWXIE3E/nBq2GLR\nEIzw1/sJlLiFydDAusVCa6yFPjdglfZ8jAqI5jwdzC/W/AstW7W8jj39VxljRVCD\nbR/TOU7unuD9kCz65oS8hZBgHhE3/gVjvPN/Kr7dTtQMyG7eeHO17m1HbwD2MctL\nHGDUadj23BMLv0PCL5Eqa4BBy2pJEOWUJm1s0qO9EcwK2wVYFWu96sCKH3RoQt93\nMbGoAOgZq12P5/9E0/J1gof6KrjcGlTG5jan5u7XnTDc2myqwoPDdb/JHsXyXdxw\nHJSdIvqwag8aPut5FjBITilMXw2nHNLpHh8fyWXRGFa/eKidyeC9fMUDsvcyZuos\nWt3DD5sqHn+ubvK+g9I2N9nEwAEmX90Dj5OYOyxUBYnLWpbDuc+71KA3w1idHtnU\nLHNNKc02MKVca9skeb6eKAbBBxHaXRLTWboTpffAwOSTHQy8MiHRvmPm9iglak0g\n8s12rYLBqtptDPOgGrxxA339C0lSs9F57/DPec+skLvFX41S9WpmncUn1MycVIAm\nehdM6aGoNqonVV4oNaO5WevQl7iJeCd9sk6Ih4kRDpQB9Yko8E/fUDJunYcBLf5g\nVwEDtxkVHEW4Hfkxk6w=\n-----END CERTIFICATE-----\n"
                }
            };
            var capabilitiesResp = await SendCapabilities(onboarding);
        }

        private static async Task<bool> SendCapabilities(OnboardResponse onboarding)
        {
            var intermediateCertB64 =
                "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";
            var issuingCertB64 = @"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";
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ClientCertificates.Add(X509CertificateService.GetCertificate(onboarding));
            httpClientHandler.ClientCertificates.Add(
                new X509Certificate2(Convert.FromBase64String(intermediateCertB64)));
            httpClientHandler.ClientCertificates.Add(new X509Certificate2(Convert.FromBase64String(issuingCertB64)));
            var _httpClient = new HttpClient(httpClientHandler);

            try
            {
                if (onboarding is null)
                    throw new ArgumentNullException(nameof(onboarding));
                var capabilitiesService = new CapabilitiesService(new HttpMessagingService(_httpClient));
                var capabilitiesParameters = new CapabilitiesParameters()
                {
                    ApplicationId = _applicationId,
                    CertificationVersionId = _certificationVersionId,
                    EnablePushNotifications = CapabilitySpecification.Types.PushNotification.Disabled,
                    OnboardResponse = onboarding,
                    ApplicationMessageId = Guid.NewGuid().ToString(),
                    CapabilityParameters = new()
                    {
                        new CapabilityParameter()
                        {
                            Direction = CapabilitySpecification.Types.Direction.SendReceive,
                            TechnicalMessageType = TechnicalMessageTypes.Iso11783TaskdataZip
                        },
                        new CapabilityParameter()
                        {
                            Direction = CapabilitySpecification.Types.Direction.SendReceive,
                            TechnicalMessageType = TechnicalMessageTypes.ShpShapeZip
                        },
                    }
                };
                var sendCapabilitiesResponse = await capabilitiesService.SendAsync(capabilitiesParameters);
                var fetchingService = new FetchMessageService(_httpClient);
                var fetched = await fetchingService.FetchAsync(onboarding);
                var message = DecodeMessageService.Decode(fetched[0].Command.Message);

                return message.ResponseEnvelope.ResponseCode.Equals(201);
            }
            catch (Exception e)
            {
                throw;
            }
        }
// You can define other methods, fields, classes and namespaces here        
    }
}