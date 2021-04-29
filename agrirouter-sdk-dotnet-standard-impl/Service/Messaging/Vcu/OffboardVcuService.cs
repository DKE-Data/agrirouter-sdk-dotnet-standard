using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agrirouter.Cloud.Registration;
using Agrirouter.Request;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Service.Messaging;
using Agrirouter.Api.Service.Messaging.Vcu;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Impl.Service.Common;
using Google.Protobuf;

namespace Agrirouter.Impl.Service.Messaging.Vcu
{
    /// <summary>
    ///     Service to onboard VCUs.
    /// </summary>
    public class OffboardVcuService : IOffboardVcuService
    {
        private readonly IMessagingService<MessagingParameters> _messagingService;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="messagingService">-</param>
        public OffboardVcuService(IMessagingService<MessagingParameters> messagingService)
        {
            _messagingService = messagingService;
        }

        /// <summary>
        ///     Please see <seealso cref="IMessagingService{T}.Send" /> for documentation.
        /// </summary>
        /// <param name="offboardVcuParameters">-</param>
        /// <returns>-</returns>
        public MessagingResult Send(OffboardVcuParameters offboardVcuParameters)
        {
            var encodedMessages = new List<string> {Encode(offboardVcuParameters).Content};
            var messagingParameters = offboardVcuParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.Send(messagingParameters);
        }

        /// <summary>
        ///     Please see <seealso cref="IMessagingService{T}.SendAsync" /> for documentation.
        /// </summary>
        /// <param name="offboardVcuParameters">-</param>
        /// <returns>-</returns>
        public Task<MessagingResult> SendAsync(OffboardVcuParameters offboardVcuParameters)
        {
            var encodedMessages = new List<string> {Encode(offboardVcuParameters).Content};
            var messagingParameters = offboardVcuParameters.BuildMessagingParameter(encodedMessages);
            return _messagingService.SendAsync(messagingParameters);
        }

        /// <summary>
        ///     Please see <seealso cref="IEncodeMessageService{T}.Encode" /> for documentation.
        /// </summary>
        /// <param name="offboardVcuParameters"></param>
        /// <returns>-</returns>
        public EncodedMessage Encode(OffboardVcuParameters offboardVcuParameters)
        {
            var messageHeaderParameters = new MessageHeaderParameters
            {
                ApplicationMessageId = offboardVcuParameters.ApplicationMessageId,
                TeamSetContextId = offboardVcuParameters.TeamsetContextId ?? "",
                TechnicalMessageType = TechnicalMessageTypes.DkeCloudOffboardEndpoints,
                Mode = RequestEnvelope.Types.Mode.Direct
            };

            var messagePayloadParameters = new MessagePayloadParameters
            {
                TypeUrl = OffboardingRequest.Descriptor.FullName
            };

            var offboardingRequest = new OffboardingRequest();
            foreach (var endpoint in offboardVcuParameters.Endpoints) offboardingRequest.Endpoints.Add(endpoint);

            messagePayloadParameters.Value = offboardingRequest.ToByteString();

            var encodedMessage = new EncodedMessage
            {
                Id = Guid.NewGuid().ToString(),
                Content = EncodeMessageService.Encode(messageHeaderParameters, messagePayloadParameters)
            };

            return encodedMessage;
        }
    }
}