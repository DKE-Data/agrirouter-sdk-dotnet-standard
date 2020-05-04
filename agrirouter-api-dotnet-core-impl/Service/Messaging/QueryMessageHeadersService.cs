using System;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Feed.Response;
using Agrirouter.Impl.Service.Messaging.Abstraction;
using Google.Protobuf.WellKnownTypes;

namespace Agrirouter.Impl.Service.messaging
{
    /// <summary>
    /// Service to query message headers.
    /// </summary>
    public class QueryMessageHeadersService : QueryMessageBaseService,
        IDecodeMessageResponseService<HeaderQueryResponse>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        public QueryMessageHeadersService(IMessagingService<MessagingParameters> messagingService)
            : base(messagingService)
        {
        }

        protected override string TechnicalMessageType => TechnicalMessageTypes.DkeFeedHeaderQuery;

        public HeaderQueryResponse Decode(Any messageResponse)
        {
            try
            {
                return HeaderQueryResponse.Parser.ParseFrom(messageResponse.Value);
            }
            catch (Exception e)
            {
                throw new CouldNotDecodeMessageException("Could not decode query message header response.", e);
            }
        }
    }
}