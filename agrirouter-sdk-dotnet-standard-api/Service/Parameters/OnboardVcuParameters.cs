using System.Collections.Generic;
using Agrirouter.Cloud.Registration;

namespace Agrirouter.Sdk.Api.Service.Parameters
{
    /// <summary>
    ///     Parameter container definition.
    /// </summary>
    public class OnboardVcuParameters : MessageParameters
    {
        /// <summary>
        ///     Onboarding requests.
        /// </summary>
        public List<OnboardingRequest.Types.EndpointRegistrationDetails> OnboardingRequests { get; set; }
    }
}