using System;

namespace Agrirouter.Sdk.Api.Exception
{
    /// <summary>
    ///     Will be thrown if the signature can not be created.
    /// </summary>
    [Serializable]
    public class CouldNotCreateSignatureException : System.Exception
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public CouldNotCreateSignatureException()
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="message">-</param>
        public CouldNotCreateSignatureException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="message">-</param>
        /// <param name="inner">-</param>
        public CouldNotCreateSignatureException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}