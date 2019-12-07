using com.dke.data.agrirouter.api.dto.onboard;
using com.dke.data.agrirouter.api.service;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.impl.service.messaging;
using Newtonsoft.Json;
using Xunit;

namespace com.dke.data.agrirouter.api.test.service.messaging
{
    public class CapabilitiesServiceTest : AbstractIntegrationTest
    {
        [Fact]
        public void GivenValidCapabilitiesWhenSendingCapabilitiesMessageThenTheAgrirouterShouldSetTheCapabilities()
        {
            ICapabilitiesServices capabilitiesServices = new CapabilitiesService();
            var parameters = new CapabilitiesParameters
            {
                OnboardingResponse = OnboardingResponse,
                ApplicationId = ApplicationId,
                CertificationVersionId = CertificationVersionId,
                EnablePushNotifications = false
            };
            capabilitiesServices.send(parameters);
        }

        private OnboardingResponse OnboardingResponse
        {
            get
            {
                string onboardingResponseAsJson =
                    "{\"deviceAlternateId\":\"79c36c10-65c9-40eb-9045-c581a3a1596b\",\"capabilityAlternateId\":\"c2467f6d-0a7e-48ca-9b57-1862186aef12\",\"sensorAlternateId\":\"d9536a56-1f36-4159-9d1b-c37ec5216c88\",\"connectionCriteria\":{\"gatewayId\":\"3\",\"measures\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/measures/79c36c10-65c9-40eb-9045-c581a3a1596b\",\"commands\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/commands/79c36c10-65c9-40eb-9045-c581a3a1596b\"},\"authentication\":{\"type\":\"P12\",\"secret\":\"L3t75USovZnj2zFFnLstootMwI0g0sI3DjJ3\",\"certificate\":\"MIACAQMwgAYJKoZIhvcNAQcBoIAkgASCBAAwgDCABgkqhkiG9w0BBwGggCSABIIEADCCBRowggUWBgsqhkiG9w0BDAoBAqCCBO4wggTqMBwGCiqGSIb3DQEMAQMwDgQIatTRXUg7WGMCAgfQBIIEyG2vHh5Vxsiy1r297YXEVeznfm7EcC0H20H8L1BnxfrsxIsE+SDYYI8CqS7DanJ1OQ+CHOJHB3TSaNeQ5P/HavfHN9B6z3KCy/1YDT4CQj+qiOse3CrMsDKnSvLj6/Jbmw9WEBZaLa/lm5LQ1BHvuDGiqShp+eUcacw2PcnciXcfGkrA5U+Zi7dMYErCNtbb6rX/FQM6e/07YUhw842lEBoTFI+/BjfnvRyXBKxe2W5kGqjw8eNJux2436LFpaHFHyiMmQEyIvHbGG9xTfMQwqS+4u8y/ixUaqUS/1V9xGehpPoBTZpDNNnhKC3HCRQ0vshf7kOI5SBSS/5mSICP/BsxYR4h93tMq6pmM3neWucUXBxLqjHrc/h5TSS7gngkeCE805W9YZqyZY8xlYwAd14aMSsJk+9O2oaquWQymrTUxQ28LnuUeg14u9jmKc0S2XGganTzbKThlUIZ4UDir+PAfwZROkLPOnE5uKYXjl05zVjJ+1uHwl4G4SdJnktn/+YTFKycC6cp0GfVGACXvIgB9DGOXfU4rqHUVHo8rP2VTlhpNBXDoxgxvu4iV7hQkMBLMPmdGqJcIT1jbrbwp/1+yzz7eCwiQCuDqJ3dQV2uYQBJbqUmGbHevF04K5dE8h+mlR7fzptfz/+u9QBgAZSirwg8SEgZbdBLmIBwBZ4vqYekpY4qIqDQv5ohbUBTmXRwiLZ457jS7CzEYIRePvSUevduMHnk7LLTuLv4FMDJiWsYcZk8LzWI9e+23FBh8DjrifclyL/TZwmtO5Ll/yZcNtgZHp7YYaizOLdoPOwsOt6p72xa+4Gd+kbEgqcuixGpjgZY4C+/LgPmE3kdCgRcZkDTn3Wm1PWYqInRkKR/6IuJiHDwDvE/cG+Jxrh02sg5TzCCQigB4uvqhNV9DBpUItLGz4t/BFbrSSLa+B1h0suumZJCD0hb9A4StbFbzVcZ/6kvR9aKP5waXhYpeOGBUAMpjN+eTBYMPA2jsEGXIT5bgm+OVzxXphNZrqrN2JOtjnZbj4XW67SdfJnWH3X1eVGV+rW6Ul0HF/Xx1Df3eZiYUT3QDEmbf/9Rp8ViR/jOd8dGTx+cjDiIZMdPIyZ9wJHbKP2A6Zb5xJBPUNgabddEGJ5qTeDSBR1y38ICTkHvAfWZ/ur7Ewk2IlrU7ENHxNaUMSYkf9ocAxXoyiU/bv22h6HG8EDCgLoLJAduYY5vaqc/yo1LkR4Lhv7OjYUMkhkx5wZzgobXBIIEAA9Qju3ynOPVcF8Gz2F6+SXshV1JNYgABIIBHhfib3DxXZ2jVRTGTtb4JY5HaNp+71Qk+lmUQ1YYNC3pXcAGKYx4VZ0ci4+9OOnmobUH6v39L6GYM4OEfq3ieCc1y59SGEbBBfZIhxYzpzUZ9FlOfkE3IcG8DmqZ399DS1dmhcHNNyfm8vSQCOabXS05tNmF1TkyfbhvzpScxpSYM2DNKZTxqzKUu3b9iQ14DL5r+9/NSiroAYrzCaPG9P5Mj32/mhKviInXBio6j8uLcQ2VhuObe8aTtrLa38q8lqkwuh82u2N7Dj5hwlLEBpNo83UVoeXYGfUc6X6x7/hdmO0l2uuzbhXt04IJ1iEZ/GaXHGFRuJ6AbwjcULpxDXrxv9iDIAQIMRUwEwYJKoZIhvcNAQkVMQYEBAEAAAAAAAAAAAAwgAYJKoZIhvcNAQcGoIAwgAIBADCABgkqhkiG9w0BBwEwHAYKKoZIhvcNAQwBBjAOBAgZ80q/SJqh0QICB9CggASCBLio19pt1XKsZegGLNne8L83VwoRvsS1aQoz+ktiOrTE30mCUJfyV5fC5pirWZfl8t6T/45XnHESm6WHkCq3Ln1w3GEqgWin1e1x805GXsev9eK6MNoNskoMUjabhO0xIq5B1HByreU90H1GaEt4UYFcTJuABJ6QeJx2KSuzoxlG4w3eYwPiowzLGY8MeqaLBG9zQGNxGQY/XFxyWv9b3S6WFhfwQWlCcs4HvxjmuHuy3+OnxZZxwrEv43qD9lukitKDTljWT/QVY76Ko1+jZc2vHPOS1uTfYkt/nFIGIFNWxfOC2SSWgTbvrBQsp4v4bHsbImbpehmwUqEV1dYN3QaN+u2AVR39naOXuGsBB5ZINZ8Zm4oofTwfZvuopJR9fRCj3UN+Eaexy15v1p3/urYMnUMQTk3CgwI8kN2U9nPNcLsHFsvTQnnWN30L8p22NBbLGrUbTj03OSM2CjJeY6r+taOgwIuz+a5HQqEMZYvHT3yCqc6cIyu2BIC3shYmQIc8LLGa8F1GwZLxd8WNyTbo3HZhxdMaw2elw2n3Cs8I8gfGN5rzwp6CWNSqnatkmFbUYlpqH+lxqMMAVBT5igSaR+fmec2Hern05ocVO0NEhU4QA17iWBb3MhDT+Iid9M+rYX9y5ZG0iW5q33hMND6gefj0V2qNOIRNa+LcBSIwUmxJJE4SuPCT6qyUC3LazEL3y4KZqOwMaIwbCeTPo2v4e3um8TZpajnd2nNL6JKSDkFhwZf2qZdbOoLhdWnnFO9r//Ov7U9NPMCBQqD54ZocjW+DqdKyh49+VAiNG5Mp00CSq7iNWo8p0a6QKdQihlaJvY2pLMJqAMFcs00EggJIJau6ZXE9BL0gvGqth8cceO9qNsnHbRZdk0m0FO1dfDs+pFQkSILHQKWRU7mS0REZwW1TSz++6liUavzpNlY5gzYKdvbEHAUVsofLpQpuxnGYiAYanCJA3jdHlWCD1Dl94PXItJQLH8Y8fc7NVxuiILgnxN/LQmiEHoGVFQuY+cGtzN1gqI72mAS2P4prrv3vI8/MWyrox2eQYFeSS3a9WF3zHZG7l0p+jDUF4PFjZ0EszJNDVjVhAg5N8augp8NOX0wdgGeWOYe4Fewh6gX7YpI43Izz6kMt0Vo/fFFHuRk+WYzfigYqb/Cn22pzp7qa8nv7zYQDSzJFneuoPY75iMCd31NtQveO5jimskd8Pzk1NJtXQhh0qV9vvSuK1N/v+jeWGKlO3oSqzszHUfpiIu69W11zlqaeNvL2mfljUnUOyw1/iwU71+FxpvgClDzqgbsNabfSjRLuHyvBMFpFQZ5mLX67J9TE4SVZNmDU3qJmpQT73E+SKjuwJaqyZMTJxGCANhRG6612XV+/G1/gugtv4QIc4Bm6oWdMhLjfSNSxoRhvazubKnuXdYbz1yatgNIRoxjEkKvOJwM295NXP/rEyW5/oyGwpWs7gQ1DcPsHkrbWMw3I8DE20Fw4YP27z/+WZhFCvMu726D4lSp+zqpcQaMeV3qM58wd6/pp0pHRfw792mkLjBkUh4Z714LoEDyP9jsfx88TsGoZw1Ofz1g8xStmG1sDanVzWBc7wsHCC8mH/hMUHlvelQ4AAAAAAAAAAAAAAAAAAAAAAAAwMTAhMAkGBSsOAwIaBQAEFLQ/PpgiP15cK0IpLJ0ta4TNgdO0BAgFoqBuxaSDWgICB9AAAA==\"}}";
                var onboardingResponse =
                    JsonConvert.DeserializeObject(onboardingResponseAsJson, typeof(OnboardingResponse));
                return onboardingResponse as OnboardingResponse;
            }
        }
    }
}