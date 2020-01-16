using System;
using System.Net;

namespace Agrirouter.Api.Exception
{
    /// <summary>
    /// Will be thrown if the message could not be sent to the AR.
    /// </summary>
    [Serializable]
    public class CouldNotSendMessageException : System.Exception
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