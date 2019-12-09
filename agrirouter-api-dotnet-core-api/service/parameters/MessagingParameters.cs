using System.Collections.Generic;
using com.dke.data.agrirouter.api.dto.onboard;

namespace com.dke.data.agrirouter.api.service.parameters
{
    public class MessagingParameters : Parameters
    {
        public OnboardingResponse OnboardingResponse { get; set; }

        public List<string> EncodedMessages { get; set; }
    }
}