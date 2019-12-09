using System.Collections.Generic;
using com.dke.data.agrirouter.api.service.parameters.inner;

using com.dke.data.agrirouter.api.dto.onboard;

namespace com.dke.data.agrirouter.api.service.parameters
{
    public class CapabilitiesParameters : Parameters
    {
    public OnboardingResponse OnboardingResponse { get; set; }

    public string ApplicationId { get; set; }

    public string CertificationVersionId { get; set; }

    public bool EnablePushNotifications { get; set; }
    
    public List<CapabilityParameter> CapabilityParameters{get;set;}

    }
}