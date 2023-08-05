using System.Collections.Generic;
using Agrirouter.Commons;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    ///     Parameter container definition.
    /// </summary>
    public class SendMessageParameters : MessageParameters
    {
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
        ///     Define the file metadata.
        /// </summary>
        public Metadata Metadata { get; set; }
    }
}