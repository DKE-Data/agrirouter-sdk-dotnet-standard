using System;
using System.Collections.Generic;
using System.Threading;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Response;
using com.dke.data.agrirouter.api.definitions;
using com.dke.data.agrirouter.api.dto.onboard;
using com.dke.data.agrirouter.api.service.parameters;
using com.dke.data.agrirouter.api.service.parameters.inner;
using com.dke.data.agrirouter.impl.service.common;
using com.dke.data.agrirouter.impl.service.messaging;
using Newtonsoft.Json;
using Xunit;

namespace com.dke.data.agrirouter.api.test.service.messaging
{
    public class SubscriptionServiceTest : AbstractIntegrationTest
    {
        [Fact]
        public void GivenEmptySubscriptionWhenSendingSubscriptionMessageThenTheMessageShouldBeAccepted()
        {
            var subscriptionService = new SubscriptionService(new MessagingService());
            var subscriptionParameters = new SubscriptionParameters()
            {
                OnboardingResponse = OnboardingResponse,
            };

            subscriptionService.Send(subscriptionParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService();
            var fetch = fetchMessageService.Fetch(OnboardingResponse);
            Assert.Single(fetch);

            var decodeMessageService = new DecodeMessageService();
            var decodedMessage = decodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.Ack,
                decodedMessage.ResponseEnvelope.Type);
        }

        [Fact]
        public void GivenSingleSubscriptionEntryWhenSendingSubscriptionMessageThenTheMessageShouldBeAccepted()
        {
            var subscriptionService = new SubscriptionService(new MessagingService());
            var subscriptionParameters = new SubscriptionParameters()
            {
                OnboardingResponse = OnboardingResponse,
                TechnicalMessageTypes = new List<Subscription.Types.MessageTypeSubscriptionItem>()
            };
            var technicalMessageType = new Subscription.Types.MessageTypeSubscriptionItem
            {
                TechnicalMessageType = TechnicalMessageTypes.Iso11783TaskdataZip
            };
            subscriptionParameters.TechnicalMessageTypes.Add(technicalMessageType);
            subscriptionService.Send(subscriptionParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService();
            var fetch = fetchMessageService.Fetch(OnboardingResponse);
            Assert.Single(fetch);

            var decodeMessageService = new DecodeMessageService();
            var decodedMessage = decodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(201, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.Ack,
                decodedMessage.ResponseEnvelope.Type);
        }

        [Fact]
        public void
            GivenMultipleSubscriptionEntriesWithOneInvalidTechnicalMessageTypeWhenSendingSubscriptionMessageThenTheMessageShouldBeNotBeAccepted()
        {
            var subscriptionService = new SubscriptionService(new MessagingService());
            var subscriptionParameters = new SubscriptionParameters()
            {
                OnboardingResponse = OnboardingResponse,
                TechnicalMessageTypes = new List<Subscription.Types.MessageTypeSubscriptionItem>()
            };
            var technicalMessageTypeForTaskdata = new Subscription.Types.MessageTypeSubscriptionItem
            {
                TechnicalMessageType = TechnicalMessageTypes.Iso11783TaskdataZip
            };
            subscriptionParameters.TechnicalMessageTypes.Add(technicalMessageTypeForTaskdata);

            var technicalMessageTypeForProtobuf = new Subscription.Types.MessageTypeSubscriptionItem
            {
                TechnicalMessageType = TechnicalMessageTypes.Iso11783DeviceDescriptionProtobuf
            };
            subscriptionParameters.TechnicalMessageTypes.Add(technicalMessageTypeForProtobuf);
            subscriptionService.Send(subscriptionParameters);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            var fetchMessageService = new FetchMessageService();
            var fetch = fetchMessageService.Fetch(OnboardingResponse);
            Assert.Single(fetch);

            var decodeMessageService = new DecodeMessageService();
            var decodedMessage = decodeMessageService.Decode(fetch[0].Command.Message);
            Assert.Equal(400, decodedMessage.ResponseEnvelope.ResponseCode);
            Assert.Equal(ResponseEnvelope.Types.ResponseBodyType.AckWithFailure,
                decodedMessage.ResponseEnvelope.Type);

            var messages = decodeMessageService.Decode(decodedMessage.ResponsePayloadWrapper.Details);
            Assert.NotNull(messages);
            Assert.NotEmpty(messages.Messages_);
            Assert.Single(messages.Messages_);
            Assert.Equal("VAL_000006", messages.Messages_[0].MessageCode);
            Assert.Equal(
                "Subscription to \"iso:11783:-10:device_description:protobuf\" is not valid per reported capabilities.",
                messages.Messages_[0].Message_);
        }

        private OnboardingResponse OnboardingResponse
        {
            get
            {
                var onboardingResponseAsJson =
                    "{\"deviceAlternateId\":\"44d7003e-2743-492b-86aa-4fed91fcf20a\",\"capabilityAlternateId\":\"c2467f6d-0a7e-48ca-9b57-1862186aef12\",\"sensorAlternateId\":\"bfbfa1a5-bf09-4423-9ad3-3678368ffe53\",\"connectionCriteria\":{\"gatewayId\":\"3\",\"measures\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/measures/44d7003e-2743-492b-86aa-4fed91fcf20a\",\"commands\":\"https://dke-qa.eu10.cp.iot.sap/iot/gateway/rest/commands/44d7003e-2743-492b-86aa-4fed91fcf20a\"},\"authentication\":{\"type\":\"P12\",\"secret\":\"8A71w6kCuckYiTK4R1k4bCVxsnB6flcK9crZ\",\"certificate\":\"MIACAQMwgAYJKoZIhvcNAQcBoIAkgASCBAAwgDCABgkqhkiG9w0BBwGggCSABIIEADCCBRowggUWBgsqhkiG9w0BDAoBAqCCBO4wggTqMBwGCiqGSIb3DQEMAQMwDgQIU8YKzcpqXAkCAgfQBIIEyMUMLKNDCMD37yAy0eGowRxzeotWJWEa6DBgoXs/XzB/TIMvio+1rTa2S7xDYHIX1rYVYorXqYc6adzuZ9nBeglg68JpmYyjWERlFDQsSzc0rXZQoJo0gdxWT2C6XgMx3xRZh72xg5c1GcLJTHBG2OtT46UHrKFowA5hELIOs8pOQ4IrHJg5+Xa9ES/BPkxD+iS8oETHzvj1IxYXXrcdU/xjHVZfkh455LwUF7tEbxFr+/pV3XFE+jQ9qFTUt4oI0B6kTJrMEaAMiP32kwCHZph+iMF1u0uo1nlWgCaqH/e0JKAGKfN6KTUC4wXrumgRG6wJSpFu/uDII4GYO4rDMKgYu9H8UNsN+5VFHYfCNuqPQiOPgqr2QgEiY2VgIBhJ0VaxJkANxIN+0F96sMRbhSuOPMam8EZqqaUx4PohXBkJ3qTpnjMEKQhpiQQmywRIBW2ejO1N+DPOE7xA0CCKKVbZUkf3dl/uR5+fBOM2vvxmlE5sEa0n89R0yXMxKo9mS6Z1hZuFSamx7/k5zEO4zsFW5He1g+l76PRBH+2tuRDpFUnAe/qOwKXLa1qZLRoEFCel6cMh7PrgZReQ7uH9hF/Fz+1oYSGS9zatjvpbG9F0616dkpP58M7p6cxtRR/bkUqdF0nF7cwE+iyEZSxF2QlAC5SIcmPMlgrl6lsOOF4AtR012wp9iyjOQvpua9stEtO0XnKv41twY9eb9ezOPqU4e/3k7k22jK0nELMiw5ibU1kKi5/1Ai3ugrW5YgAByWMLS3n+oBS6DncJD6RicsdoyZzyL8xngo4e608jH2RUBWz5YdENtEne8qzrB0ZqVNVAKpd1De54G98P+ZGup46On3sWH6OBdd1ukIiKvqdmI2mpJurEpNYEb0MIQzsv4bZBzKeTKJfb+qRvlMo6VpcfejgcV4S4zPWbN8hAr3914TMxg9lcXfiEA13fi/D7pnW8OPDCUtwTd/g8KxsiTi2kLN0YGUrc/tmjeWIGHhbrQvJsTTJWJ8cFAkNsQ+li+8tBxMwMBtn0JwdEmO2VALZbdEo8zOPqhUR+pjuAxGk0ItUianiuxaRVdLys+byBBE1/e+kNK5nCMX2Hxd1UA30FnMnN83RDcZpgsPtROoyaSD9puxJiqinqcaNqryjHN82tQvNhi1Eli6eBxqTRlfMBcc+00QK9A+GVIk84bfYFLuaOMC+vIA6Q9FOBLfblFhc910pm55BOO5GZ0BoaXcmUWIh+jGj0LQrWBIIEAMO980k7kWhROvknIXUBzB268jUlplS2BIIBHi1iS/lggg3npP4KjXyDq+CGuX+hRDsf+G/StU6SkAbe7ECy6D0oPUxCbR8VmAHH46w8cuwAs0cwDX/3FiUjchhZpDDbDg94QXvtn6DVEDItNOq9qLj6rZ7OZxXVpRRcJgGaQS21QtzzyRQis8159i9ogeoACmDIYUCaUXcnmLncvYxjxJPZhdfiTRV+X4aqFinXIDjWulVs2V1qjog2SThG/vi9Kxb8ic8HBbFn5FPOfDSFU4a5Ji64xk3lQQKuyWuIvDnFm24RKPzZ7R1KaXqLY75sQ5GGvR9jbV3yqw3Kb4Caa2CUaEH8XcQy4i4PZcXO/ceoThfzivWMtQ/ERYMGNP37qZ3cMRUwEwYJKoZIhvcNAQkVMQYEBAEAAAAAAAAAAAAwgAYJKoZIhvcNAQcGoIAwgAIBADCABgkqhkiG9w0BBwEwHAYKKoZIhvcNAQwBBjAOBAhlHfUqGXeHawICB9CggASCBLg5xsl9fdFnQsUXIgi+P8FfPFQ3GyejNYRMbM2JExrgmyP45fi8gtXu4c2fPNkQ/NJ0BZ0dyW4R2JtI93PGBaC39heGgoKDZVm6rp9pyzETqVkDNbnocc2D+tB7b3prcoBruEUUMqyxJs6MTgCHD0NCpEEn+ZJhbtU2B2cEnsIefe7Ur7Tx6dFN002QQNKSTLolO8yjY4H1Z9XD61sA/wPVPu/nIVZMB5cmZWv/knE0DfT5YOi5EQU7QETr9qJUcZBg7idvG7OrViV5+77VCyiWDZ0mVeX/FiEYclEh7YalXyANbgQaHfKeBmpqQYRgs4fiQ1wvUmOb1Y1tV2Ok/orwENDiG6o6dXYfiLbwLvWeihchGAaZF6Di2VwaWL3Qo65ZMVzx3JFEBLmnU6u31aSzAiIMQcezKl4ERq2OGG7GJFRkQ7W8Kj728W1EUEdqk7ARKuHA1DaVeq9VyrqRfYkiagIGcX0NzCtRWFBDj9K9W7q2BWI9Cu5ihRkHClq7XoIZGbOeo7esc0os9216g4rasFvc/udWzPdbChEr1xirCrK+uw3YR2GF5Bfag5Bi4xRiwSRXMPv+zw28fX9ic3HSmvEA7HE5MYjdkaUm20MF4KDJpqElyY9b3P0ahU69hDehgzSxXjDk2BoKL4taGuZFDe0KyRBYyI5X+r3Q8rcrkFLcYHYyqj1JrGvKiPwD2YxTw+ulBVVv5DtA8B3IjfdKhG4Lx4ZOaZIiEv5rfpC/6tJb5Rvw7pwLxU3HZHk/pWtjQAOWbW28q6yZgUaHCny+lJf1B1ho3sicrTACYqNWTR4wmgjaN6DYP254+HM7/aloGqsQ/sem4chvkoYEggJIc8BEVMcoTBk+HsGwyRmMuAQ6WpISu9tq1ptSRIa89m0u8TJIQ5gnHzIznjRCwS8OLJhd+C3kRll1QFUk42wVo/mlmLny4Q/2b7TdWnqLrH02gFEsyg/FdLxHabzRr8ptm7lHTVXwOhFP5oZAs4z9vACudfe0L/vLrXOz8aRl4UqqcVVFYv9zPyBcMS5/AAmy6/kuB4yLE2H7Hl1Rsb/nMzv2UcNPxjYad57nTqifWCguMpy/16ZyiHT1BXsktgT7IFmjE7xe7c6FqJFTE0DfwBpCDSDN8uAopBOCETqU4V40b3kudOFCqKiOQ6nPgOdYnNBV/24R8Opal/wywryHNUjK6lFVxQR7TpMjUrxl3QSKwUWkxWKSCQFw/vR7t8/2t3j411dQTal6N/r28W3QhrDgzQpc8SHL8GD/acyAtazRu9zAQh8lRsb7FK/y3j6PDUwYom88jlPUmJOgL1cS/5IOOdCCKEVf/4sxidhJJ7RC1+GDZYPPAKvD3h6fBDU8oyssPh09UKu6lxIcKxkEmf5rUd6SfeUTjcUxs3Vj5L/SDpwSxeCytNV4T6cbwsWMnUQSp8xyLjvNYMD64H8AQ9AVZZq4aUoP5jZfCgD9ezoYLPnUM+WpiIUP5XCfkG1QYiXc4EE6njQdXiW5YCg98XzYpMPDVfxF5vzhXpyFv1P1225ClRB+4h+QJIeXYJIAYZCemSyx2JCh6jkeCT4VdFmNGO+3h0yawnkE+BN3nB5a76Y9wq8bfkmIomgAAAAAAAAAAAAAAAAAAAAAAAAwMTAhMAkGBSsOAwIaBQAEFAuhqhajoSPZBzZyehcRUuT+9ex2BAhPV5tF43Wa8AICB9AAAA==\"}}";
                var onboardingResponse =
                    JsonConvert.DeserializeObject(onboardingResponseAsJson, typeof(OnboardingResponse));
                return onboardingResponse as OnboardingResponse;
            }
        }
    }
}