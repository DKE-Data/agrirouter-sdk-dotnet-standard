using System;
using System.Net;

namespace Agrirouter.Api.Exception
{
    /// <summary>
    ///     Will be thrown if the messages can not be fetched from the AR.
    /// </summary>
    [Serializable]
    public class CouldNotFetchMessagesException : System.Exception
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="statusCode">-</param>
        /// <param name="message">-</param>
        public CouldNotFetchMessagesException(HttpStatusCode statusCode, string message): base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}