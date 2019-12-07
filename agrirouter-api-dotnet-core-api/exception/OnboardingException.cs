using System;
using System.Net;
using System.Threading.Tasks;

namespace com.dke.data.agrirouter.api.exception
{
    /**
     * Will be thrown if the onboarding process was not successful.
     */
    public class OnboardingException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ErrorMessage { get; }

        public OnboardingException(HttpStatusCode statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }
    }
}