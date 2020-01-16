using System.Collections.Generic;
using Agrirouter.Api.Service.Parameters.Inner;
using Agrirouter.Request.Payload.Endpoint;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class CapabilitiesParameters : MessageParameters
    {
        public string ApplicationId { get; set; }

        public string CertificationVersionId { get; set; }

        public CapabilitySpecification.Types.PushNotification EnablePushNotifications { get; set; }

        public List<CapabilityParameter> CapabilityParameters { get; set; }
    }
}