using System.Collections.Generic;
using Agrirouter.Request.Payload.Endpoint;
using com.dke.data.agrirouter.api.service.parameters.inner;

namespace com.dke.data.agrirouter.api.service.parameters
{
    /**
     * Parameter container definition.
     */
    public class CapabilitiesParameters : SendMessageParameters
    {

        public string ApplicationId { get; set; }

        public string CertificationVersionId { get; set; }

        public CapabilitySpecification.Types.PushNotification EnablePushNotifications { get; set; }

        public List<CapabilityParameter> CapabilityParameters { get; set; }
    }
}