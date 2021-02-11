using System;
using Agrirouter.Feed.Response;
using Agrirouter.Sdk.Api.Definitions;
using Agrirouter.Sdk.Api.Exception;
using Agrirouter.Sdk.Api.Service.Messaging;
using Agrirouter.Sdk.Api.Service.Parameters;
using Agrirouter.Sdk.Impl.Service.Messaging.Abstraction;
using Google.Protobuf.WellKnownTypes;

namespace Agrirouter.Sdk.Impl.Service.Messaging
{
    /// <summary>
    ///     Service to query messages.
    /// </summary>
    public class QueryMessagesService : QueryMessageBaseService, IDecodeMessageResponseService<MessageQueryResponse>
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        public QueryMessagesService(IMessagingService<MessagingParameters> messagingService) :
            base(messagingService)
        {
        }

        protected override string TechnicalMessageType => TechnicalMessageTypes.DkeFeedMessageQuery;

        public MessageQueryResponse Decode(Any messageResponse)
        {
            try
            {
                return MessageQueryResponse.Parser.ParseFrom(messageResponse.Value);
            }
            catch (Exception e)
            {
                throw new CouldNotDecodeMessageException("Could not decode query message header response.", e);
            }
        }
    }
}