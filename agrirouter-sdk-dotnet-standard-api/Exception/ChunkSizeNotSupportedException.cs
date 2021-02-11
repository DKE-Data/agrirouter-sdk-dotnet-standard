using System;

namespace Agrirouter.Sdk.Api.Exception
{
    /// <summary>
    ///     Will be thrown if the chunk size is not supported.
    /// </summary>
    [Serializable]
    public class ChunkSizeNotSupportedException : System.Exception
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public ChunkSizeNotSupportedException()
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="message">-</param>
        public ChunkSizeNotSupportedException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="message">-</param>
        /// <param name="inner">-</param>
        public ChunkSizeNotSupportedException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}