using System;
using System.IO;
using Agrirouter.Request;
using com.dke.data.agrirouter.api.exception;
using com.dke.data.agrirouter.api.service.parameters;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Serilog;

namespace com.dke.data.agrirouter.impl.service.common
{
    public class EncodeMessageService
    {
        private readonly UtcDataService _utcDataService;

        public EncodeMessageService()
        {
            _utcDataService = new UtcDataService();
        }

        public string Encode(MessageHeaderParameters messageHeaderParameters,
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

        private RequestEnvelope Header(MessageHeaderParameters messageHeaderParameters)
        {
            Log.Debug("Begin creating the header of the message.");

            var requestEnvelope = new RequestEnvelope
            {
                ApplicationMessageId = messageHeaderParameters.ApplicationMessageId ??
                                       MessageIdService.ApplicationMessageId(),
                ApplicationMessageSeqNo = MessageHeaderParameters.ApplicationMessageSeqNo,
                TechnicalMessageType = messageHeaderParameters.TechnicalMessageType,
                Mode = messageHeaderParameters.Mode,
                Timestamp = _utcDataService.NowAsTimestamp()
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

        private RequestPayloadWrapper PayloadWrapper(MessagePayloadParameters messagePayloadParameters)
        {
            Log.Debug("Begin creating the payload of the message.");
            var any = new Any {TypeUrl = messagePayloadParameters.TypeUrl, Value = messagePayloadParameters.Value};

            var requestPayloadWrapper = new RequestPayloadWrapper {Details = any};

            Log.Debug("Finished creating the payload of the message.");
            return requestPayloadWrapper;
        }
    }
}