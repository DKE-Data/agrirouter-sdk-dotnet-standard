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
    public class OffboardVcuServiceTest : AbstractSecuredIntegrationTestForFarmingSoftware
    {
        private static readonly HttpClient HttpClient = HttpClientFactory.AuthenticatedHttpClient(OnboardResponse);

        [Fact]
        public void GivenNonExistingEndpointIdWhenOffboardingVcuThenTheArShouldReturnErrorMessage()
        {
            var offboardVcuService =
                new OffboardVcuService(new MessagingService(HttpClient), new EncodeMessageService());
            var offboardVcuParameters = new OffboardVcuParameters
            {
                OnboardResponse = OnboardResponse,
                ApplicationMessageId = MessageIdService.ApplicationMessageId(),
                Endpoints = new List<string>
                {
                    "8597bd75-0366-41a9-b13c-3e685a47909e"
                }
            };
            offboardVcuService.Send(offboardVcuParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(400, decodedMessage.ResponseEnvelope.ResponseCode);
        }

        [Fact]
        public void GivenExistingEndpointIdWhenOffboardingVcuThenTheArShouldReturnErrorMessage()
        {
            var onboardVcuService = new OnboardVcuService(new MessagingService(HttpClient), new EncodeMessageService());
            var endpointId = Guid.NewGuid().ToString();

            var onboardVcuParameters = new OnboardVcuParameters
            {
                OnboardResponse = OnboardResponse,
                ApplicationMessageId = MessageIdService.ApplicationMessageId(),
                OnboardingRequests = new List<OnboardingRequest.Types.EndpointRegistrationDetails>
                {
                    new OnboardingRequest.Types.EndpointRegistrationDetails
                    {
                        Id = endpointId,
                        Name = "My first virtual CU which is deleted directly..."
                    }
                }
            };
            onboardVcuService.Send(onboardVcuParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService(HttpClient);
            var fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            var decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);

            var offboardVcuService =
                new OffboardVcuService(new MessagingService(HttpClient), new EncodeMessageService());
            var offboardVcuParameters = new OffboardVcuParameters
            {
                OnboardResponse = OnboardResponse,
                ApplicationMessageId = MessageIdService.ApplicationMessageId(),
                Endpoints = new List<string>
                {
                    endpointId
                }
            };
            offboardVcuService.Send(offboardVcuParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            fetch = fetchMessageService.Fetch(OnboardResponse);
            Assert.Single(fetch);

            decodedMessage = DecodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(400, decodedMessage.ResponseEnvelope.ResponseCode);
        }

        private static OnboardResponse OnboardResponse
        {
            get
            {
                const string onboardingResponseAsJson = "{\"deviceAlternateId\":\"4b6c4c92-806f-4295-b19b-c5076b824661\",\"capabilityAlternateId\":\"c2467f6d-0a7e-48ca-9b57-1862186aef12\",\"sensorAlternateId\":\"0e1607ed-57cc-4ce3-bdb9-2cf465b61e84\",\"connectionCriteria\":{\"gatewayId\":\"3\",\"measures\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/measures/4b6c4c92-806f-4295-b19b-c5076b824661\",\"commands\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/commands/4b6c4c92-806f-4295-b19b-c5076b824661\"},\"authentication\":{\"type\":\"P12\",\"secret\":\"9tbtNSpeZnn6REJvB2j5oWywLlguuS1HidIt\",\"certificate\":\"MIACAQMwgAYJKoZIhvcNAQcBoIAkgASCBAAwgDCABgkqhkiG9w0BBwGggCSABIIEADCCBRowggUWBgsqhkiG9w0BDAoBAqCCBO4wggTqMBwGCiqGSIb3DQEMAQMwDgQIa5I8i6hzBvYCAgfQBIIEyHXruz9nvKwMBkfP4LYtTk4rAfU3FybB8Jx6Q9Y4djdW3fBpTz7ZPh9Htwoa7o4XQM8PInahkBJGVtPLiQp5+mfWkhuf13OOjE64xZxw2rxpCyKLL7htqGSlVrtSvxlWzHcHU2w+yNBIypL1HmUDylPRQfebs+kdccrHhT1qFr+Lsax7tlgm6huJpT3ciI4rBL9F9g4FhKIdBfhJaP4QcSkGAdU9ysX+RlpxhWKkVh4hI6L1Rh1rVNxFYMai024LmYoovt/Szrmiv8rci/TvY3DAkMEhYtwTDrmJZRkmqksj/Uu0sFfMWOvyqjzQIXzFobdAx8ed7y6vF01Keu728gMtwDmAaCH4hC08lj9HexUM1WMqsOZjW9wT1mIeVOxtDAnpEPxcyp4uKSRDcjgMlG0NVbuZyFairEJsB+1Y4mkdPp8qyJtHbwdBEvNnlbrtMjGIHffxMf4LhHMBu4mwS6K3+3Smx5jNTWPM67rbABGR8z88Lcmfgq0v4vHGewUqC52x/mDvTtnJ3B63XvpWC8919x5ZMYYxgi5Jq8mJ0dG/N0btWTH92kP/obGHx1sPZ1ZkC5ydb7AbaWna5GOkbA+wS7sbMuPvs7NjFJoSSxjiOI7b+PkGyjBH9DdkaFzqgA+/gS2xkivhN3njHFKmbe4tMgNjKCQKtLXezpp6lLGV1ikPDMPZXO4p7/VkJ8ivguj71covqKIEjrHnTYaiNZXQc3FOZOmjhbP02FABHNbZ4nOgLBdUzqNPskDRRFtOsvOnwqai69DDbvoERuDwoFEPORtI6ql1EGsgQJNs3/4B2u/i7Ur+MpCTwyRMJFbODjfv50z1ifEC+OHJmTU22Ew9OU2hHjwtJrIWtbOGQnzdg5g8whZn+uDbpc+8E2wzGRLcSY5Rn+ybB9Ku03c1T/fEkxl9yH9Sx9JaLhzbntV5sQAXXIV/n0o/v2GQFH3GWwsTlg0juvIQ3/A10psHzdrl+VxMGehojAs1d5gz7W34DCzG4kt2SY+OoYPEcKKqPdsUpVDUaF1LV42TSHDDrGRVrjtpGdrroXkBxEfa0RcVf45VqcFRdlGavHlUJZHB6RrI+tj+hwvPuzkv+7qPqw64Li+ZdeCO/tIKJKc3UPUpko9eQmCmUlQ00DTmqO9Ixp7fiGTYyRFmFmRNhOF8NxmoYJLr2X4yzjJZyCwVQxW/6vpFqDlaxxiSKe2G1QoUNmEqpyEwDbuhmAITcCRU8g4ViG5sg1wYsJp8BIIEACzO5xVe2w0i3l76ONBZ6e6gjhNCpqkvBIIBHkQJELNV/oCuUovpB0AhfTKnkqZg4jUO1w9ge7hZ7oZh2JaXnHRdYv8ZTozDyM/xRl/H4wcyKY9C4nA8sToOFN2gLZ6kMZz2N2MDGem7fcwNPWU8zrUjZSjcUVZo5ZM0uqXE2338d25hZaiIG4uzpHgO+AWYSkJtxqc9HQMoyK7wtRuhxaUfyjpSL6DG06/DQ6kwX/eOjEAfCu0eA5gQ2Edggv2/ZwolNn80uQ01dJxlaKGG7FhimX1TYX8YUULfpJ/TSyfqLAAqrRx5KHmx4jPFvHpaqKN+vnvuyXWO2Bb1Xd4cHEQBynCpvPo8KXjrRQmpFUL/12rpQhSqJtfnfMYPK3pOdZGmMRUwEwYJKoZIhvcNAQkVMQYEBAEAAAAAAAAAAAAwgAYJKoZIhvcNAQcGoIAwgAIBADCABgkqhkiG9w0BBwEwHAYKKoZIhvcNAQwBBjAOBAjjO0p29d4IpgICB9CggASCBLg2s+aoKFViOSWu1D9cI2F51NQYP4xaz4N9W6pFT2S9WIszId05IVmO5tJtP8SpTrqpbbjJ4csIBSDq/+R2eZB28eD/wlnYsihDAggfsLY1waaoZPJpolTOxmCp76Jem0We7P9FyAsgZmnnNcTQMFNyyLOM7Crm1ilYRTUY+BlTMtDq1dBmjcBJZ9JVVbN1mLEc//uTDHslG0EwGM+/XIPq31n8kS7VR1jLBeGGeMuJP449fjnI4BXlUaWGqMkn5idzvjyCII2MGyfryEHCSZOh60W7uW1+t1ZtqicT19BFHk8M9gFJiVOQX3KSO592/KK1rh9sTMAYMjf9Vw2rg5Hp97pkcuOmJwBS64U585FceVXPObp3Hb5z0CidKu+ejnydijLTNBVHK5sWOQ13XDRrmJjmdGPDIbk1HzEhsAjdaLrqqM41ZdQhIOssqm5nhTqlH3uMzN+r6seGHEHS6Y7Qc9iy3yRTbhRfUScrdRgRQyytBhQhCUdiHwUSq+N/iNP9fhS/7iffdXBfX8Oe0yGScEO5EDWdOti/drJyXgTMXT8QwDv2gpuOakeGHwVeimhiDq6u6KWljJuwM6T42MmL+jZ7QRNHz/cPpk2qnVhAz6pvzVYBC9eeI6mp77JvLh5+sNBlwrgvCOrhSwPLGTz21LdRPGEuIMn4j9hoVH4s6yLk/hlInXMhXfJj/adVHpkbmSHYBWn/ZIzwC2PIsAA5OfdYgdh2XSN59/pwEilkpE4mlZp2iCrkhfYTT4ehPKtBpUJs408au/ep1/EW3V7viRSP4HefgBaJFp25b9IBxGoYQB4YLaSpUC7AVMnDp3zzK3GNtbu0dYwfxPAEggJIPjyDZ7LngrcLrtsIFSXGpunxKkQeMH2GR28oaYupMBV2pBa/UDnquOrYwbMKki+AUuIF11G0J/PMFDLTQ4uqNlhTt0mNvNpxp2bKeQwxpwcLFix6vV0DCIGFlDsdPagMOAiaUHqw/bXcZU8JDxEOy7O4CGAUoKhNqd2dwyqbkfNMBvfjnpfOJVZceXG8kDNW9LYVrwLIlJGfSq5ki8mGeg+32vyWll85bWHTW3L0V1dl4KObTy+/d5uM88Y+2TDf0mvdODLjg2d/G6QOT/3Ixv2A0Mc0ns6z9fhU6s7gWg+0eh3eVfzzjMj/crcGcxPBFpcFFJ3Sj2EdYwsdXtscjGyn4+j0xXoxmPzhZqsE5TDrHeh9YihwS2jwMN2bLoCv6eQpAygwl6BBLV7pnwHzNua9qZe5j+wp1KeolK2IeG/BWvVuPLoVD5kIHHlx4OsbkBncorjcC+uX45z3DVRvegS7MvJvcJ/DdSPGhRZq4vpCeq7/JwY6B5CRmSHmYk1ThXzy60GGe7Mx8lcpOTpu9hvgGa02WLxZGd5RSo40fKHCIM3OKVYChNmmMO2LmQHywHdhIPXKhKwTRlxI82Ra7rh7+gN+azxGadQUEkdtvAq9wLX85bUctpsWQCc7jIL3ckz4NC60r14U+GuUK4NImHUgZzAiuXQDz55rZR7qeH1PmNjf0Cz2IX7Je4zdHXwnkvmz1LEgWu+wXGqBbjYWYZsityympYMd/pkcfNzdGKMWhtEO5WKuw498negAAAAAAAAAAAAAAAAAAAAAAAAwMTAhMAkGBSsOAwIaBQAEFM4KmRvmoEv12hOIQxhLWNllGNBiBAh234yf+XqwwAICB9AAAA==\"}}";
                var onboardingResponse =
                    JsonConvert.DeserializeObject(onboardingResponseAsJson, typeof(OnboardResponse));
                return onboardingResponse as OnboardResponse;
            }
        }
    }
}