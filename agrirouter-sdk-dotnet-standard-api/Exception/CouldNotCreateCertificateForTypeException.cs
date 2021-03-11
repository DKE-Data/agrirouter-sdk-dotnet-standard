namespace Agrirouter.Api.Exception
{
    /// <summary>
    /// Will be thrown if the type of the certificate is unknown.
    /// </summary>
    public class CouldNotCreateCertificateForTypeException : System.Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">-</param>
        public CouldNotCreateCertificateForTypeException(string message) : base(message)
        {
        }
    }
}