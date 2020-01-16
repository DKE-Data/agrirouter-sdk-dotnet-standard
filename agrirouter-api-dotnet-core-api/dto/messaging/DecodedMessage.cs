using Agrirouter.Response;

namespace com.dke.data.agrirouter.api.dto.messaging
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class DecodedMessage
    {
        public ResponseEnvelope ResponseEnvelope { get; set; }

        public ResponsePayloadWrapper ResponsePayloadWrapper { get; set; }
    }
}