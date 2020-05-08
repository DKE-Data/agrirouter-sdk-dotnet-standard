using System;
using System.IO;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Request;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Serilog;

namespace Agrirouter.Impl.Service.Common
{
    /// <summary>
    /// Service for message encoding.
    /// </summary>
    public class EncodeMessageService
    {
        /// <summary>
        /// Encode a message using the given header parameters and payload parameters. The encoded Base64 message can be sent to the AR directly.
        /// </summary>
        /// <param name="messageHeaderParameters">Parameters for the message header.</param>
        /// <param name="messagePayloadParameters">Parameters for the message payload.</param>
        /// <returns>-</returns>
        /// <exception cref="MissingParameterException">Will be thrown if any of the parameters is missing.</exception>
        public static string Encode(MessageHeaderParameters messageHeaderParameters,
            MessagePayloadParameters messagePayloadParameters)
        {
            Log.Debug("Start encoding of the message.");

            if (null == messageHeaderParameters || null == messagePayloadParameters)
            {
                throw new MissingParameterException();
            }

            using var memoryStream = new MemoryStream();
            Header(messageHeaderParameters).WriteDelimitedTo(memoryStream);
            PayloadWrapper(messagePayloadParameters).WriteDelimitedTo(memoryStream);
            var encodedMessage = Convert.ToBase64String(memoryStream.GetBuffer());

            Log.Debug("Finished encoding of the message.");
            return encodedMessage;
        }

        private static RequestEnvelope Header(MessageHeaderParameters messageHeaderParameters)
        {
            Log.Debug("Begin creating the header of the message.");

            var requestEnvelope = new RequestEnvelope
            {
                ApplicationMessageId = messageHeaderParameters.ApplicationMessageId ??
                                       MessageIdService.ApplicationMessageId(),
                ApplicationMessageSeqNo = Parameters.ApplicationMessageSeqNo,
                TechnicalMessageType = messageHeaderParameters.TechnicalMessageType,
                Mode = messageHeaderParameters.Mode,
                Timestamp = UtcDataService.NowAsTimestamp()
            };

            if (!string.IsNullOrEmpty(messageHeaderParameters.TeamSetContextId))
            {
                requestEnvelope.TeamSetContextId = messageHeaderParameters.TeamSetContextId;
            }

            if (messageHeaderParameters.Recipients != null)
            {
                foreach (var recipient in messageHeaderParameters.Recipients)
                {
                    requestEnvelope.Recipients.Add(recipient);
                }
            }

            if (messageHeaderParameters.ChunkInfo != null)
            {
                requestEnvelope.ChunkInfo = messageHeaderParameters.ChunkInfo;
            }

            Log.Debug("Finished creating the header of the message.");

            return requestEnvelope;
        }

        private static RequestPayloadWrapper PayloadWrapper(MessagePayloadParameters messagePayloadParameters)
        {
            Log.Debug("Begin creating the payload of the message.");
            var any = new Any {TypeUrl = messagePayloadParameters.TypeUrl, Value = messagePayloadParameters.Value};

            var requestPayloadWrapper = new RequestPayloadWrapper {Details = any};

            Log.Debug("Finished creating the payload of the message.");
            return requestPayloadWrapper;
        }
    }
}