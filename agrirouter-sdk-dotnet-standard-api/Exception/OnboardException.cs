using System.Net;
using Agrirouter.Api.Dto.Onboard;

namespace Agrirouter.Api.Exception
{
    /// <summary>
    ///     Will be thrown if the onboarding or revoking processes are not successful.
    /// </summary>
    [Serializable]
    public class OnboardException : System.Exception
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="statusCode">-</param>
        /// <param name="onboardError">-</param>
        public OnboardException(HttpStatusCode statusCode, OnboardError onboardError): base(onboardError.Message)
        {
            StatusCode = statusCode;
            Error = onboardError;
        }

        public HttpStatusCode StatusCode { get; }

        public OnboardError Error { get; }
    }
}