using Google.Protobuf;

namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    ///     Parameter container definition.
    /// </summary>
    public class MessagePayloadParameters
    {
        /// <summary>
        /// Every endpoint can send messages based on its capabilities. The size of a message is however limited. A message contains 2 parts: Header and Body. The limitation of a message is defined as follows:
        /// - The maximum body size is equivalent of 1024000 characters/signs
        /// - Since the chunking is performed on the raw message data this means, that we have to lower the MAX_LENGTH_FOR_MESSAGES to allow Base64 encoding afterwards.
        /// - Total message size is limited to 1468000 characters/signs
        /// - Messages that are above this limit will be rejected.
        /// The AR will return an error indicating that the message size is above the limit.
        /// If the message size is above 5 MB the AR will not return any error. In order to send messages with sizes above threshold, these messages must be split into chunks with the above limit.
        /// </summary>
        public static int MaxLengthForRawMessageContent => 767997;

        /// <summary>
        ///     Type URL.
        /// </summary>
        public string TypeUrl { get; set; }

        /// <summary>
        ///     Value.
        /// </summary>
        public ByteString Value { get; set; }

        /**
         * Checks whether the payload should be chunked or not.
         */
        public bool ShouldBeChunked()
        {
            return Value.ToStringUtf8().Length > MaxLengthForRawMessageContent;
        }
    }
}