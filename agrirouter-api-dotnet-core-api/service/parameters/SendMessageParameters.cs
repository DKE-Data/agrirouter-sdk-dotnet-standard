using System.Collections.Generic;
using com.dke.data.agrirouter.api.dto.onboard;

namespace com.dke.data.agrirouter.api.service.parameters
{
    public abstract class SendMessageParameters : Parameters
    {
        public OnboardingResponse OnboardingResponse { get; set; }

        public MessagingParameters BuildMessagingParameter(List<string> encodedMessages)
        {
            return new MessagingParameters
            {
                ApplicationMessageId = ApplicationMessageId,
                TeamsetContextId = TeamsetContextId,
                OnboardingResponse = OnboardingResponse,
                EncodedMessages = encodedMessages
            };
        }
    }
}