using System;
using System.Net;

namespace Agrirouter.Api.Exception
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
        /// <param name="message">-</param>
        public OnboardException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}