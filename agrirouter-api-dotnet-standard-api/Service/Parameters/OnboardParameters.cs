namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class OnboardParameters : Parameters
    {
        /// <summary>
        /// Application ID.
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        /// UUID for the endpoint.
        /// </summary>
        public string Uuid { get; set; }

        /// <summary>
        /// Certification version ID.
        /// </summary>
        public string CertificationVersionId { get; set; }

        /// <summary>
        /// Gateway ID.
        /// </summary>
        public string GatewayId { get; set; }

        /// <summary>
        /// Certificate type.
        /// </summary>
        public string CertificationType { get; set; }

        /// <summary>
        /// Application type.
        /// </summary>
        public string ApplicationType { get; set; }

        /// <summary>
        /// Registration code.
        /// </summary>
        public string RegistrationCode { get; set; }
    }
}