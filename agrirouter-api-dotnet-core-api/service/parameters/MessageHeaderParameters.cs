using System.Collections.Generic;
using Agrirouter.Commons;
using Agrirouter.Request;

namespace com.dke.data.agrirouter.api.service.parameters
{
    public class MessageHeaderParameters : Parameters
    {
        public long ApplicationMessageSeqNo { get; set; }
        public string TechnicalMessageType { get; set; }
        public RequestEnvelope.Types.Mode Mode { get; set; }
        public string TeamSetContextId { get; set; }
        public List<string> Recipients { get; set; }
        public ChunkComponent ChunkInfo { get; set; }
    }
}