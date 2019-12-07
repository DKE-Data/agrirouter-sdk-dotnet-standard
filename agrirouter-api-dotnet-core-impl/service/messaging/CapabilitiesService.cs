using System.Collections.Generic;
using com.dke.data.agrirouter.api.service;
using com.dke.data.agrirouter.api.service.parameters;

namespace com.dke.data.agrirouter.impl.service.messaging
{
    public class CapabilitiesService : MessagingService, ICapabilitiesServices
    {
        public string send(CapabilitiesParameters parameters)
        {
            var messageParameters = new MessagingParameters();
            messageParameters.OnboardingResponse = parameters.OnboardingResponse;
            messageParameters.EncodedMessages = new List<string>();
            return send(messageParameters);
        }
    }
}