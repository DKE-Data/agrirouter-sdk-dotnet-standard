using System.Collections.Generic;
using Agrirouter.Api.Dto.Onboard;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public abstract class MessageParameters : Parameters
    {
        /// <summary>
        /// Onboarding response.
        /// </summary>
        public OnboardResponse OnboardResponse { get; set; }

        /// <summary>
        /// Builder.
        /// </summary>
        /// <param name="encodedMessages">Encoded messages.</param>
        /// <returns></returns>
        public MessagingParameters BuildMessagingParameter(List<string> encodedMessages)
        {
            return new MessagingParameters
            {
                ApplicationMessageId = ApplicationMessageId,
                TeamsetContextId = TeamsetContextId,
                OnboardResponse = OnboardResponse,
                EncodedMessages = encodedMessages
            };
        }
    }
}