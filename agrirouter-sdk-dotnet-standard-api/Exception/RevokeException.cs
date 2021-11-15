using System.Net;

namespace Agrirouter.Api.Exception
{
    /// <summary>
    ///     Will be thrown if the revoke is not successful.
    /// </summary>
    public class RevokeException : System.Exception
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="statusCode">-</param>
        /// <param name="message">-</param>
        public RevokeException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        private HttpStatusCode StatusCode { get; }
    }
}