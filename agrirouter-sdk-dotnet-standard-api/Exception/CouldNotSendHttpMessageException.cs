using System;
using System.Net;

namespace Agrirouter.Api.Exception
{
    /// <summary>
    ///     Will be thrown if the message could not be sent to the AR.
    /// </summary>
    [Serializable]
    public class CouldNotSendHttpMessageException : System.Exception
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="statusCode">-</param>
        /// <param name="errorMessage">-</param>
        public CouldNotSendHttpMessageException(HttpStatusCode statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        public HttpStatusCode StatusCode { get; }

        public string ErrorMessage { get; }
    }
}