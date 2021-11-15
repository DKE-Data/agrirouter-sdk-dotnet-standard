using System;
using System.Net;
using Agrirouter.Api.Dto.Onboard;

namespace Agrirouter.Api.Exception
{
    /// <summary>
    ///     Will be thrown if the revoking process is not successful.
    /// </summary>
    [Serializable]
    public class RevokeException : OnboardRevokeExceptionBase {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="statusCode">-</param>
        /// <param name="onboardError">-</param>
        public RevokeException(HttpStatusCode statusCode, OnboardError onboardError): base(statusCode, onboardError)
        {
        }
    }
}