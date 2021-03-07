using Agrirouter.Response;

namespace Agrirouter.Api.Dto.Messaging
{
    /// <summary>
    ///     Data transfer object for the communication.
    /// </summary>
    public class DecodedMessage
    {
        /// <summary>
        ///     Response envelope.
        /// </summary>
        public ResponseEnvelope ResponseEnvelope { get; set; }

        /// <summary>
        ///     Response payload wrapper.
        /// </summary>
        public ResponsePayloadWrapper ResponsePayloadWrapper { get; set; }
    }
}