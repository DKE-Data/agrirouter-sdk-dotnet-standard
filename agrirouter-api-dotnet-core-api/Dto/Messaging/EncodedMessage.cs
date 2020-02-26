namespace Agrirouter.Api.Dto.Messaging
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class EncodedMessage
    {
        /// <summary>
        /// Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Content.
        /// </summary>
        public string Content { get; set; }
    }
}