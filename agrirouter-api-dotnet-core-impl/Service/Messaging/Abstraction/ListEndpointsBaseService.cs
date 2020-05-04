using System;
using System.Collections.Generic;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Agrirouter.Request;
using Agrirouter.Request.Payload.Account;
using Agrirouter.Response.Payload.Account;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;

namespace Agrirouter.Impl.Service.Messaging.Abstraction
{
    /// <summary>
    /// Abstraction of the service to list endpoints to avoid multiple implementations.
    /// </summary>
    public abstract class ListEndpointsBaseService : IListEndpointsService
    {
        private readonly IMessagingService<MessagingParameters> _messagingService;

        /// <summary>
        /// Constructor.
        /// <param name="messagingService">-</param>
        /// </summary>
        protected ListEndpointsBaseService(IMessagingService<MessagingParameters> messagingService)
        {
            _messagingService = messagingService;
        }

        /// <summary>
        /// Please see base class declaration for documentation.
        /// </summary>
        /// <param name="listEndpointsParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(ListEndpointsParameters listEndpointsParameters)
        {
            var encodedMessages = new List<string> {Encode(listEndpointsParameters).Content};
            var messagingParameters = listEndpointsParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        /// <summary>
        /// Please see <seealso cref="IEncodeMessageService{T}.Encode"/> for documentation.
        /// </summary>
        /// <param name="listEndpointsParameters">-</param>
        /// <returns>-</returns>
        public EncodedMessage Encode(ListEndpointsParameters listEndpointsParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = listEndpointsParameters.ApplicationMessageId,
                TeamSetContextId = listEndpointsParameters.TeamsetContextId ?? "",
                TechnicalMessageType = TechnicalMessageType,
                Mode = RequestEnvelope.Types.Mode.Direct
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = ListEndpointsQuery.Descriptor.FullName
            };

            var listEndpointsQuery = new ListEndpointsQuery {Direction = listEndpointsParameters.Direction};

            if (null != listEndpointsParameters.TechnicalMessageType)
            {
                listEndpointsQuery.TechnicalMessageType = listEndpointsParameters.TechnicalMessageType;
            }

            messagePayloadParameters.Value = listEndpointsQuery.ToByteString();

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = EncodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
        
        /// <summary>
        /// Decode the list endpoints response from the server.
        /// </summary>
        /// <param name="messageResponse"></param>
        /// <returns></returns>
        /// <exception cref="CouldNotDecodeMessageException"></exception>
        public ListEndpointsResponse Decode(Any messageResponse)
        {
            try
            {
                return ListEndpointsResponse.Parser.ParseFrom(messageResponse.Value);
            }
            catch (Exception e)
            {
                throw new CouldNotDecodeMessageException("Could not decode list endpoints message.", e);
            }
        }

        protected abstract string TechnicalMessageType { get; }
    }
}