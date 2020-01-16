using System.Collections.Generic;
using Agrirouter.Api.Dto.Onboard;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public abstract class MessageParameters : Parameters
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