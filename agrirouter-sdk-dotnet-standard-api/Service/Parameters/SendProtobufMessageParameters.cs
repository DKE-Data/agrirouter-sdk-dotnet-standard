using System.Collections.Generic;
using Agrirouter.Commons;
using Google.Protobuf;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    ///     Parameter container definition for protobuf messages.
    /// </summary>
    public class SendProtobufMessageParameters : MessageParameters
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
        ///     Message content in Protobuf format.
        /// </summary>
        public ByteString ProtobufMessageContent { get; set; }

        /// <summary>
        ///     Type URL.
        /// </summary>
        public string TypeUrl { get; set; }

        /// <summary>
        ///     Define the size of the chunks.
        /// </summary>
        public int ChunkSize { get; set; }
    }
}