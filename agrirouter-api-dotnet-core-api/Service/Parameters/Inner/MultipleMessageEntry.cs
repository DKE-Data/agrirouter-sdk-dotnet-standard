using System.Collections.Generic;
using Agrirouter.Commons;

namespace Agrirouter.Api.Service.Parameters.Inner
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class MultipleMessageEntry
    {
        public string ApplicationMessageId { get; set; }

        public string TechnicalMessageType { get; set; }

        public List<string> Recipients { get; set; }

        public ChunkComponent ChunkInfo { get; set; }

        public string Base64MessageContent { get; set; }

        public string TypeUrl { get; set; }

        public string TeamsetContextId { get; set; }
    }
}