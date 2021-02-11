using System;

namespace Agrirouter.Sdk.Api.Exception
{
    /// <summary>
    ///     Will be thrown if a given message can not be decoded.
    /// </summary>
    [Serializable]
    public class CouldNotDecodeMessageException : System.Exception
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public CouldNotDecodeMessageException()
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="message">-</param>
        public CouldNotDecodeMessageException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="message">-</param>
        /// <param name="inner">-</param>
        public CouldNotDecodeMessageException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}