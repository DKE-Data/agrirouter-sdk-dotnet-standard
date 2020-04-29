using System;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Feed.Response;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Impl.Service.messaging.abstraction;
using Google.Protobuf.WellKnownTypes;

namespace Agrirouter.Impl.Service.messaging
{
    /// <summary>
    /// Service to query messages.
    /// </summary>
    public class QueryMessagesService : QueryMessageBaseService, IDecodeMessageResponseService<MessageQueryResponse>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        /// <param name="encodeMessageService">-</param>
        public QueryMessagesService(MessagingService messagingService, EncodeMessageService encodeMessageService) :
            base(messagingService, encodeMessageService)
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