using System;
using System.Net;

namespace Agrirouter.Sdk.Api.Exception
{
    /// <summary>
    ///     Will be thrown if the onboarding process was not successful.
    /// </summary>
    [Serializable]
    public class OnboardException : System.Exception
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="statusCode">-</param>
        /// <param name="errorMessage">-</param>
        public OnboardException(HttpStatusCode statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        private HttpStatusCode StatusCode { get; }

        private string ErrorMessage { get; }
    }
}