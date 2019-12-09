using System;
using com.dke.data.agrirouter.api.definitions;
using com.dke.data.agrirouter.api.enums;

namespace com.dke.data.agrirouter.api.service.parameters
{
    /**
     * Necessary parameters for the onboarding process.
     */
    public class OnboardingParameters : Parameters
    {
        public string ApplicationId { get; set; }

        public string Uuid { get; set; }

        public string CertificationVersionId { get; set; }

        public string GatewayId { get; set; }

        public string CertificationType { get; set; }

        public string ApplicationType { get; set; }

        public string RegistrationCode { get; set; }
    }
}