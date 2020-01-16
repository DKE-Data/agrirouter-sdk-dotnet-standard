using Google.Protobuf;

namespace com.dke.data.agrirouter.api.service.parameters
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