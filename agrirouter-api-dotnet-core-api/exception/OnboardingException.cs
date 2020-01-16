using System;
using System.Net;

namespace Agrirouter.Api.Exception
{
    /// <summary>
    /// Will be thrown if the onboarding process was not successful.
    /// </summary>
    [Serializable]
    public class OnboardingException : System.Exception
    {
        private HttpStatusCode StatusCode { get; }

        private string ErrorMessage { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="statusCode">-</param>
        /// <param name="errorMessage">-</param>
        public OnboardingException(HttpStatusCode statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }
    }
}