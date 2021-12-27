namespace Agrirouter.Api.Definitions
{
    /// <summary>
    /// Every endpoint can send message based on its capabilities. The size of a message is however limited. A message contains 2 parts: Header and Body. The limitation of a message is defined as follows:
    /// - The maximum body size is equivalent of 1024000 characters/signs
    /// - Since the chunking is performed on the raw message data this means, that we have to lower the MAX_LENGTH_FOR_MESSAGES to allow Base64 encoding afterwards.
    /// - Total message size is limited to 1468000 characters/signs
    /// - Messages that are above this limit will be rejected.
    /// The AR will return an error indicating that the message size is above the limit.
    /// If the message size is above 5 MB the AR will not return any error. In order to send messages with sizes above threshold, these messages must be split into chunks with the above limit.
    /// </summary>
    public class ChunkSizeDefinition
    {
        /// <summary>
        ///     Maximum value the AR can handle.
        /// </summary>
        public static int MaximumSupportedBase64EncodedMessageLength => 1024000;

        /// <summary>
        ///     Maximum size of a raw message without being encoded.
        /// </summary>
        public static int MaximumSupportedRawMessageLength => 767997;
    }
}