using Agrirouter.Response;

namespace Agrirouter.Api.Dto.Messaging
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