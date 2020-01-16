using Google.Protobuf;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public class MessagePayloadParameters : Parameters
    {
        public string TypeUrl { get; set; }

        public ByteString Value { get; set; }
    }
}