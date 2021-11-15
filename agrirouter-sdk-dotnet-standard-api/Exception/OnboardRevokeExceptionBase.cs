using System;
using System.Net;
using Agrirouter.Api.Dto.Onboard;

namespace Agrirouter.Api.Exception
{
    /// <summary>
    ///     Base exception class for errors during onboarding and revoking processes.
    /// </summary>
    [Serializable]
    public class OnboardRevokeExceptionBase : System.Exception
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="statusCode">-</param>
        /// <param name="onboardError">-</param>
        public OnboardRevokeExceptionBase(HttpStatusCode statusCode, OnboardError onboardError): base(onboardError.Message)
        {
            StatusCode = statusCode;
            Error = onboardError;
        }

        /// <summary>
        ///     Constructor with error message only.
        /// </summary>
        /// <param name="statusCode">-</param>
        /// <param name="message">-</param>
        public OnboardRevokeExceptionBase(HttpStatusCode statusCode, string message): base(message)
        {
            StatusCode = statusCode;
            Error = new OnboardError {
                Message = message
            };
        }

        public HttpStatusCode StatusCode { get; }

        public OnboardError Error { get; }
    }
}
