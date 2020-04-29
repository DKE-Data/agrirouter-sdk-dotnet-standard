namespace Agrirouter.Api.Definitions
{
    /// <summary>
    /// Several types of certificates used during the onboarding process.
    /// </summary>
    public static class CertificationTypeDefinition
    {
        /// <summary>
        /// Type 'PEM'.
        /// </summary>
        public static string Pem => "PEM";

        /// <summary>
        /// Type 'P12'.
        /// </summary>
        public static string P12 => "P12";
    }
}