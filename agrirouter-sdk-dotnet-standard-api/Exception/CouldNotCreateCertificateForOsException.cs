namespace Agrirouter.Api.Exception
{
    /// <summary>
    /// Will be thrown if the OS of the certificate is not supported.
    /// </summary>
    public class CouldNotCreateCertificateForOsException : System.Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">-</param>
        public CouldNotCreateCertificateForOsException(string message) : base(message)
        {
        }
    }
}