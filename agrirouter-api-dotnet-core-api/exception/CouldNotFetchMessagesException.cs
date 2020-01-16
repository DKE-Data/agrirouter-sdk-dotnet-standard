using System;
using System.Net;

namespace com.dke.data.agrirouter.api.exception
{
    /// <summary>
    /// Will be thrown if the messages can not be fetched from the AR.
    /// </summary>
    [Serializable]
    public class CouldNotFetchMessagesException : Exception
    {
        private HttpStatusCode StatusCode { get; }

        private string ErrorMessage { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="statusCode">-</param>
        /// <param name="errorMessage">-</param>
        public CouldNotFetchMessagesException(HttpStatusCode statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }
    }
}