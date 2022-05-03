namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    ///     Parameter container definition.
    /// </summary>
    public class VerificationParameters : Parameters
    {
        /// <summary>
        ///     External ID for the endpoint.
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        ///     Application ID.
        /// </summary>
        public string ApplicationId { get; set; }

        /// <summary>
        ///     Certification version ID.
        /// </summary>
        public string CertificationVersionId { get; set; }

        /// <summary>
        ///     Gateway ID.
        /// </summary>
        public string GatewayId { get; set; }

        /// <summary>
        ///     Certificate type.
        /// </summary>
        public string CertificationType { get; set; }

        /// <summary>
        /// The registration code used for the authorization.
        /// </summary>
        public string RegistrationCode { get; set; }
        
    }
}