using System;
using com.dke.data.agrirouter.api.enums;

namespace com.dke.data.agrirouter.api.service.parameters
{
    /**
     * Necessary parameters for the onboarding process.
     */
    public class OnboardingParameters
    {
        public String ApplicationId { get; set; }

        public String Uuid { get; set; }

        public String CertificationVersionId { get; set; }

        public String GatewayId { get; set; }

        public CertificationType CertificationType { get; set; }

        public ApplicationType ApplicationType { get; set; }

        public String RegistrationCode { get; set; }
    }
}