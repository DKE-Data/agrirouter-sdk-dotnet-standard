using System.Collections.Generic;
using Agrirouter.Request.Payload.Endpoint;
using com.dke.data.agrirouter.api.dto.onboard;
using com.dke.data.agrirouter.api.service.parameters.inner;

namespace com.dke.data.agrirouter.api.service.parameters
{
    /**
     * Parameter container definition.
     */
    public class CapabilitiesParameters : Parameters
    {
        public OnboardingResponse OnboardingResponse { get; set; }

        public string ApplicationId { get; set; }

        public string CertificationVersionId { get; set; }

        public CapabilitySpecification.Types.PushNotification EnablePushNotifications { get; set; }

        public List<CapabilityParameter> CapabilityParameters { get; set; }
    }
}