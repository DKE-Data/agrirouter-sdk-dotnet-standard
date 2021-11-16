using System;
using System.Net;
using Agrirouter.Api.Dto.Onboard;

namespace Agrirouter.Api.Exception
{
    /// <summary>
    ///     Will be thrown if the onboarding process is not successful.
    /// </summary>
    [Serializable]
    public class OnboardException : OnboardRevokeExceptionBase {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="statusCode">-</param>
        /// <param name="onboardError">-</param>
        public OnboardException(HttpStatusCode statusCode, OnboardError onboardError): base(statusCode, onboardError)
        {
        }

        /// <summary> 
        ///     Constructor with error message only. 
        /// </summary> 
        /// <param name="statusCode">-</param> 
        /// <param name="message">-</param> 
        public OnboardException(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
    }
}