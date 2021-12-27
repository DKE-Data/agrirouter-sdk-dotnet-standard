using System;
using System.Collections.Generic;
using System.IO;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Request;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Cloud.Registration;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Serilog;

namespace Agrirouter.Impl.Service.Common
{
    /// <summary>
    ///     Service for message encoding.
    /// </summary>
    public class EncodeMessageService
    {
        /// <summary>
        ///     Encode a message using the given header parameters and payload parameters. The encoded Base64 message can be sent
        ///     to the AR directly.
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
                throw new MissingParameterException();

            using var memoryStream = new MemoryStream();
            Header(messageHeaderParameters).WriteDelimitedTo(memoryStream);
            PayloadWrapper(messagePayloadParameters).WriteDelimitedTo(memoryStream);
            var encodedMessage = Convert.ToBase64String(memoryStream.GetBuffer());

            Log.Debug("Finished encoding of the message.");
            return encodedMessage;
        }

        /// <summary>
        /// Chunk and add the Base64 encoding for a message if necessary.
        /// If there is only one chunk, the single chunk will be returned as Base64 encoded value.
        /// The chunk information and all IDs will be set by the SDK and are no longer in control of the application.
        /// </summary>
        /// <returns></returns>
        public static List<MessageParameterTuple> EncodeAndChunk(MessageHeaderParameters messageHeaderParameters,
            MessagePayloadParameters messagePayloadParameters, OnboardResponse onboardResponse)
        {
            if (null == messageHeaderParameters || null == messagePayloadParameters || null == onboardResponse)
            {
                throw new MissingParameterException(
                    "Header and payload parameters are required, as well as the onboard response.");
            }

            if (TechnicalMessageTypes.NeedsBase64Encoding(messageHeaderParameters.TechnicalMessageType))
            {
                if (messagePayloadParameters.ShouldBeChunked())
                {
                    throw new NotImplementedException();
                }
                else
                {
                    Log.Debug("The message type needs to be base64 encoded, therefore we are encoding the raw value.");
                    var messagePayloadParametersWithEncodedValue = new MessagePayloadParameters()
                    {
                        TypeUrl = messagePayloadParameters.TypeUrl,
                        ApplicationMessageId = messagePayloadParameters.ApplicationMessageId,
                        TeamsetContextId = messagePayloadParameters.TeamsetContextId,
                        Value = ByteString.CopyFromUtf8(
                            Convert.ToBase64String(messagePayloadParameters.Value.ToByteArray()))
                    };
                    return new List<MessageParameterTuple>()
                    {
                        new()
                        {
                            MessageHeaderParameters = messageHeaderParameters,
                            MessagePayloadParameters = messagePayloadParametersWithEncodedValue
                        }
                    };
                }
            }
            else
            {
                Log.Debug(
                    "The message type does not need base 64 encoding, we are returning the tuple 'as it is'.");
                return new List<MessageParameterTuple>()
                {
                    new()
                    {
                        MessageHeaderParameters = messageHeaderParameters,
                        MessagePayloadParameters = messagePayloadParameters
                    }
                };
            }
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
                Timestamp = UtcDataService.NowAsTimestamp(),
                Metadata = messageHeaderParameters.Metadata
            };

            if (!string.IsNullOrEmpty(messageHeaderParameters.TeamSetContextId))
                requestEnvelope.TeamSetContextId = messageHeaderParameters.TeamSetContextId;

            if (messageHeaderParameters.Recipients != null)
                foreach (var recipient in messageHeaderParameters.Recipients)
                    requestEnvelope.Recipients.Add(recipient);

            if (messageHeaderParameters.ChunkInfo != null)
                requestEnvelope.ChunkInfo = messageHeaderParameters.ChunkInfo;

            Log.Debug("Finished creating the header of the message.");

            return requestEnvelope;
        }

        private static RequestPayloadWrapper PayloadWrapper(MessagePayloadParameters messagePayloadParameters)
        {
            Log.Debug("Begin creating the payload of the message.");
            var any = new Any { TypeUrl = messagePayloadParameters.TypeUrl, Value = messagePayloadParameters.Value };

            var requestPayloadWrapper = new RequestPayloadWrapper { Details = any };

            Log.Debug("Finished creating the payload of the message.");
            return requestPayloadWrapper;
        }
    }
}