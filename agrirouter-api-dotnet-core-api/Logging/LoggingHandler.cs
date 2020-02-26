using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Agrirouter.Api.Logging
{
    /// <summary>
    ///  Internal logging handler to log request and response.
    /// </summary>
    public class LoggingHandler : DelegatingHandler
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="innerHandler">-</param>
        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        /// <summary>
        /// Logging and delegating the request to the common handler.
        /// </summary>
        /// <param name="request">-</param>
        /// <param name="cancellationToken">-</param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Log.Debug("Request:");
            Log.Debug(request.ToString());
            if (request.Content != null)
            {
                Log.Debug(await request.Content.ReadAsStringAsync());
            }

            var response = await base.SendAsync(request, cancellationToken);

            Log.Debug("Response:");
            Log.Debug(response.ToString());
            if (response.Content != null)
            {
                Log.Debug(await response.Content.ReadAsStringAsync());
            }

            return response;
        }
    }
}