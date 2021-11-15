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
    }
}