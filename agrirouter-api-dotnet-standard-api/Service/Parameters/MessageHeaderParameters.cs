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
        /// <summary>
        /// Technical message type.
        /// </summary>
        public string TechnicalMessageType { get; set; }

        /// <summary>
        /// Mode.
        /// </summary>
        public RequestEnvelope.Types.Mode Mode { get; set; }

        /// <summary>
        /// Teamset context ID.
        /// </summary>
        public string TeamSetContextId { get; set; }

        /// <summary>
        /// Recipients.
        /// </summary>
        public List<string> Recipients { get; set; }

        /// <summary>
        /// Chunk information.
        /// </summary>
        public ChunkComponent ChunkInfo { get; set; }
    }
}