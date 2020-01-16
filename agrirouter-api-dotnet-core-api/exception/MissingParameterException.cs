using System;

namespace com.dke.data.agrirouter.api.exception
{
    /// <summary>
    /// Will be thrown if there are any missing parameters.
    /// </summary>
    [Serializable]
    public class MissingParameterException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MissingParameterException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">-</param>
        public MissingParameterException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">-</param>
        /// <param name="inner">-</param>
        public MissingParameterException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}