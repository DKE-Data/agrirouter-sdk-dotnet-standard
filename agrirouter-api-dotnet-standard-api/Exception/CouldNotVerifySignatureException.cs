using System;

namespace Agrirouter.Api.Exception
{
    /// <summary>
    /// Will be thrown if the signature can not be verified.
    /// </summary>
    [Serializable]
    public class CouldNotVerifySignatureException : System.Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public CouldNotVerifySignatureException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">-</param>
        public CouldNotVerifySignatureException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">-</param>
        /// <param name="inner">-</param>
        public CouldNotVerifySignatureException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}