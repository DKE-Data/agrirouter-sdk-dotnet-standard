using System.Collections.Generic;
using Agrirouter.Cloud.Registration;

namespace Agrirouter.Api.Service.Parameters
{
    public class OnboardVcuParameters : MessageParameters
    {
        public List<OnboardingRequest.Types.EndpointRegistrationDetails> OnboardingRequests { get; set; }
    }
}