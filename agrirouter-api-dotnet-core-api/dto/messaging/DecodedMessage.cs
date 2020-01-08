using Agrirouter.Request;
using Agrirouter.Response;

namespace com.dke.data.agrirouter.api.dto.messaging
{
    public class DecodedMessage
    {
        public ResponseEnvelope ResponseEnvelope { get; set; }
        public ResponsePayloadWrapper ResponsePayloadWrapper { get; set; }
    }
}