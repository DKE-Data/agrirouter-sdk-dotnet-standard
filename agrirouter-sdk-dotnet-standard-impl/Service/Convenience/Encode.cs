using System;
using System.Text;
using Google.Protobuf.WellKnownTypes;

namespace Agrirouter.Impl.Service.Convenience
{
    /// <summary>
    ///     Encode and decode message contents from the AR.
    /// </summary>
    public class Encode
    {
        /// <summary>
        ///     Encode the message content. This means, that the raw data (which is read from the file) is encoded to base 64
        ///     (twice).
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public static string ToMessageContent(byte[] raw)
        {
            var imageAsBase64String = Convert.ToBase64String(raw);
            var base64EncodedMessageContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(imageAsBase64String));
            return base64EncodedMessageContent;
        }

        /// <summary>
        ///     Decode the message content. This means, that the message content (which is read from the message) is decoded from
        ///     base 64 (twice).
        /// </summary>
        /// <param name="messageContent"></param>
        /// <returns></returns>
        public static byte[] FromMessageContent(Any messageContent)
        {
            var content = Convert.FromBase64String(messageContent.Value.ToBase64());
            var raw = Convert.FromBase64String(Encoding.UTF8.GetString(content));
            return raw;
        }
    }
}