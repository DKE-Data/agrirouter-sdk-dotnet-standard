using System;
using System.Net;

namespace com.dke.data.agrirouter.api.exception
{
    /// <summary>
    /// Will be thrown if the message could not be sent to the AR.
    /// </summary>
    [Serializable]
    public class CouldNotSendMessageException : Exception
    {
        private HttpStatusCode StatusCode { get; }
      
        private string ErrorMessage { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="statusCode">-</param>
        /// <param name="errorMessage">-</param>
        public CouldNotSendMessageException(HttpStatusCode statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }
    }
}