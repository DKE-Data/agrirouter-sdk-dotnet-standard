namespace com.dke.data.agrirouter.api.service.parameters
{
    /**
     * Parameter container definition.
     */
    public class VerificationParameters : Parameters
    {
        public string ApplicationId { get; set; }

        public string Uuid { get; set; }

        public string CertificationVersionId { get; set; }

        public string GatewayId { get; set; }

        public string CertificationType { get; set; }

        public string RegistrationCode { get; set; }
    }
}