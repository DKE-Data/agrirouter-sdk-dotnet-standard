namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    /// Containing a tuple for message sending, i.e. used after chunking the messages.
    /// </summary>
    public class MessageParameterTuple
    {
        /// <summary>
        /// The message header parameters.
        /// </summary>
        public MessageHeaderParameters MessageHeaderParameters { get; set; }

        /// <summary>
        /// The message payload parameters.
        /// </summary>
        public MessagePayloadParameters MessagePayloadParameters { get; set; }
    }
}