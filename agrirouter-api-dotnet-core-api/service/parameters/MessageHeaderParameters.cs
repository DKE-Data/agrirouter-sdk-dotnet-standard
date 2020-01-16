using System.Collections.Generic;
using Agrirouter.Commons;
using Agrirouter.Request;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class MessageHeaderParameters : Parameters
    {
        public string TechnicalMessageType { get; set; }

        public RequestEnvelope.Types.Mode Mode { get; set; }

        public string TeamSetContextId { get; set; }

        public List<string> Recipients { get; set; }

        public ChunkComponent ChunkInfo { get; set; }
    }
}