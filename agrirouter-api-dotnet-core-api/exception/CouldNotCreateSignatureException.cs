using System;

namespace com.dke.data.agrirouter.api.exception
{
    /// <summary>
    /// Will be thrown if the signature can not be created.
    /// </summary>
    [Serializable]
    public class CouldNotCreateSignatureException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public CouldNotCreateSignatureException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">-</param>
        public CouldNotCreateSignatureException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">-</param>
        /// <param name="inner">-</param>
        public CouldNotCreateSignatureException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}