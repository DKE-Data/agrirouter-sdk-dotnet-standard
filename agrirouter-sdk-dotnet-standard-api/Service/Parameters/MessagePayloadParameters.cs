using Google.Protobuf;

namespace Agrirouter.Sdk.Api.Service.Parameters
{
    /// <summary>
    ///     Parameter container definition.
    /// </summary>
    public class MessagePayloadParameters : Parameters
    {
        /// <summary>
        ///     Type URL.
        /// </summary>
        public string TypeUrl { get; set; }

        /// <summary>
        ///     Value.
        /// </summary>
        public ByteString Value { get; set; }
    }
}