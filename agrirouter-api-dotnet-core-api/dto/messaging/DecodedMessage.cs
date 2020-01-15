using Agrirouter.Response;

namespace com.dke.data.agrirouter.api.dto.messaging
{
    /**
     * Data transfer object for the communication.
     */
    public class DecodedMessage
    {
        public ResponseEnvelope ResponseEnvelope { get; set; }
 
        public ResponsePayloadWrapper ResponsePayloadWrapper { get; set; }
    }
}