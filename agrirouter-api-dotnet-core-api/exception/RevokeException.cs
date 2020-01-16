using System;
using System.Net;

namespace com.dke.data.agrirouter.api.exception
{
    /// <summary>
    /// Will be thrown if the revoke is not successful.
    /// </summary>
    public class RevokeException : Exception
    {
        private HttpStatusCode StatusCode { get; }

        private string ErrorMessage { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="statusCode">-</param>
        /// <param name="errorMessage">-</param>
        public RevokeException(HttpStatusCode statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }
    }
}