﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agrirouter.Feed.Request;
using Agrirouter.Request;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Google.Protobuf;

namespace Agrirouter.Impl.Service.Messaging
{
    /// <summary>
    ///     Please see <seealso cref="IFeedDeleteService" /> for documentation.
    /// </summary>
    public class FeedDeleteService : IFeedDeleteService
    {
        private readonly IMessagingService<MessagingParameters> _messagingService;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        public FeedDeleteService(IMessagingService<MessagingParameters> messagingService)
        {
            _messagingService = messagingService;
        }

        /// <summary>
        ///     Please see <seealso cref="IMessagingService{T}.Send" /> for documentation.
        /// </summary>
        /// <param name="feedDeleteParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(FeedDeleteParameters feedDeleteParameters)
        {
            var encodedMessages = new List<string> {Encode(feedDeleteParameters).Content};
            var messagingParameters = feedDeleteParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        /// <summary>
        ///     Please see <seealso cref="IMessagingService{T}.SendAsync" /> for documentation.
        /// </summary>
        /// <param name="feedDeleteParameters">-</param>
        /// <returns>-</returns>
        public Task<MessagingResult> SendAsync(FeedDeleteParameters feedDeleteParameters)
        {
            var encodedMessages = new List<string> {Encode(feedDeleteParameters).Content};
            var messagingParameters = feedDeleteParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.SendAsync(messagingParameters);
        }

        /// <summary>
        ///     Please see <seealso cref="IEncodeMessageService{T}.Encode" /> for documentation.
        /// </summary>
        /// <param name="feedDeleteParameters">-</param>
        /// <returns>-</returns>
        public EncodedMessage Encode(FeedDeleteParameters feedDeleteParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = feedDeleteParameters.ApplicationMessageId,
                TeamSetContextId = feedDeleteParameters.TeamsetContextId ?? "",
                TechnicalMessageType = TechnicalMessageTypes.DkeFeedDelete,
                Mode = RequestEnvelope.Types.Mode.Direct
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = MessageDelete.Descriptor.FullName
            };

            var messageDelete = new MessageDelete();
            feedDeleteParameters.Senders?.ForEach(sender => messageDelete.Senders.Add(sender));
            feedDeleteParameters.MessageIds?.ForEach(messageId => messageDelete.MessageIds.Add(messageId));
            messageDelete.ValidityPeriod = feedDeleteParameters.ValidityPeriod;

            messagePayloadParameters.Value = messageDelete.ToByteString();

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = EncodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}