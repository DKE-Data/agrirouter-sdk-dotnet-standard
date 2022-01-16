using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Agrirouter.Api.Definitions;
using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Request;
using Agrirouter.Api.Exception;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Cloud.Registration;
using Agrirouter.Commons;
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
                    Log.Debug(
                        "The message should be chunked, current size of the payload ({}) is above the limitation.",
                        messagePayloadParameters.Value.ToStringUtf8().Length);
                    var wholeMessage = messagePayloadParameters.Value.ToStringUtf8();
                    var messageChunks = SplitByLength(wholeMessage,
                        MessagePayloadParameters.MaxLengthForRawMessageContent).ToList();
                    var messageParameterTuples = new List<MessageParameterTuple>();
                    var chunkNr = 1;
                    var chunkContextId = ChunkContextIdService.ChunkContextId();
                    foreach (var messageChunk in messageChunks)
                    {
                        var messageId = MessageIdService.ApplicationMessageId();

                        var chunkInfo = new ChunkComponent()
                        {
                            Current = chunkNr++,
                            Total = messageChunks.Count(),
                            ContextId = chunkContextId,
                            TotalSize = wholeMessage.Length
                        };

                        var messageHeaderParametersForChunk = new MessageHeaderParameters()
                        {
                            Metadata = messageHeaderParameters.Metadata,
                            Mode = messageHeaderParameters.Mode,
                            Recipients = messageHeaderParameters.Recipients,
                            ApplicationMessageId = messageId,
                            TechnicalMessageType = messageHeaderParameters.TechnicalMessageType,
                            TeamSetContextId = messageHeaderParameters.TeamSetContextId,
                            ChunkInfo = chunkInfo
                        };

                        var messagePayloadParametersForChunk = new MessagePayloadParameters()
                        {
                            Value = ByteString.CopyFromUtf8(
                                Convert.ToBase64String(Encoding.UTF8.GetBytes(messageChunk))),
                            TypeUrl = messagePayloadParameters.TypeUrl,
                        };

                        messageParameterTuples.Add(new MessageParameterTuple
                        {
                            MessageHeaderParameters = messageHeaderParametersForChunk,
                            MessagePayloadParameters = messagePayloadParametersForChunk
                        });
                    }

                    return messageParameterTuples;
                }
                else
                {
                    Log.Debug("The message type needs to be base64 encoded, therefore we are encoding the raw value.");
                    var messagePayloadParametersWithEncodedValue = new MessagePayloadParameters()
                    {
                        TypeUrl = messagePayloadParameters.TypeUrl,
                        Value = ByteString.CopyFromUtf8(
                            Convert.ToBase64String(messagePayloadParameters.Value.ToByteArray()))
                    };
                    return new List<MessageParameterTuple>()
                    {
                        new MessageParameterTuple()
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
                    new MessageParameterTuple()
                    {
                        MessageHeaderParameters = messageHeaderParameters,
                        MessagePayloadParameters = messagePayloadParameters
                    }
                };
            }
        }

        private static IEnumerable<string> SplitByLength(string str, int maxLength)
        {
            for (int index = 0; index < str.Length; index += maxLength)
            {
                yield return str.Substring(index, Math.Min(maxLength, str.Length - index));
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