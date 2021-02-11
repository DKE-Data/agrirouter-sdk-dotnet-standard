using System.Collections.Generic;
using Agrirouter.Request.Payload.Endpoint;
using Agrirouter.Sdk.Api.Service.Parameters.Inner;

namespace Agrirouter.Sdk.Api.Service.Parameters
{
    /// <summary>
    ///     Parameter container definition.
    /// </summary>
    public class CapabilitiesParameters : MessageParameters
    {
        /// <summary>
        ///     Application ID.
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        ///     Certification version ID.
        /// </summary>
        public string CertificationVersionId { get; set; }

        /// <summary>
        ///     Enabling push notifications.
        /// </summary>
        public CapabilitySpecification.Types.PushNotification EnablePushNotifications { get; set; }

        /// <summary>
        ///     Capability parameters.
        /// </summary>
        public List<CapabilityParameter> CapabilityParameters { get; set; }
    }
}