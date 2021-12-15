using System.Collections.Generic;
using Agrirouter.Commons;

namespace Agrirouter.Api.Service.Parameters.Inner
{
    /// <summary>
    ///     Parameter container definition.
    /// </summary>
    public class MultipleMessageEntry
    {
        /// <summary>
        ///     Application message ID.
        /// </summary>
        public string ApplicationMessageId { get; set; }

        /// <summary>
        ///     Technical message type.
        /// </summary>
        public string TechnicalMessageType { get; set; }

        /// <summary>
        ///     Recipients.
        /// </summary>
        public List<string> Recipients { get; set; }

        /// <summary>
        ///     Chunk information.
        /// </summary>
        public ChunkComponent ChunkInfo { get; set; }

        /// <summary>
        ///     Message content.
        /// </summary>
        public string Base64MessageContent { get; set; }

        /// <summary>
        ///     Type URL.
        /// </summary>
        public string TypeUrl { get; set; }

        /// <summary>
        ///     Teamset context ID.
        /// </summary>
        public string TeamsetContextId { get; set; }
        
        /// <summary>
        ///     Metadata.
        /// </summary>
        public Metadata Metadata { get; set; }
        
    }
}