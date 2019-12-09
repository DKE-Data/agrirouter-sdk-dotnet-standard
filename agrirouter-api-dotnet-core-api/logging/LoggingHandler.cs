using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace com.dke.data.agrirouter.api.logging
{
    /**
     * Internal logging handler to log request and response.
     */
    public class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Log.Debug("Request:");
            Log.Debug(request.ToString());
            if (request.Content != null)
            {
                Log.Debug(await request.Content.ReadAsStringAsync());
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

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