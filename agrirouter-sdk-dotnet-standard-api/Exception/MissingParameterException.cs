using System;

namespace Agrirouter.Sdk.Api.Exception
{
    /// <summary>
    ///     Will be thrown if there are any missing parameters.
    /// </summary>
    [Serializable]
    public class MissingParameterException : System.Exception
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public MissingParameterException()
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="message">-</param>
        public MissingParameterException(string message) : base(message)
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="message">-</param>
        /// <param name="inner">-</param>
        public MissingParameterException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}