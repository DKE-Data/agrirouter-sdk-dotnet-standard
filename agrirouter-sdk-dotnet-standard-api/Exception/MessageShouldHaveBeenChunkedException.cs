using System;

namespace Agrirouter.Sdk.Api.Exception
{
    /// <summary>
    ///     Will be thrown if the chunk size is not supported.
    /// </summary>
    [Serializable]
    public class MessageShouldHaveBeenChunkedException : System.Exception
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public MessageShouldHaveBeenChunkedException()
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="message">-</param>
        public MessageShouldHaveBeenChunkedException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="message">-</param>
        /// <param name="inner">-</param>
        public MessageShouldHaveBeenChunkedException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}