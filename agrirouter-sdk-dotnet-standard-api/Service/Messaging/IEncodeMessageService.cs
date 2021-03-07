using Agrirouter.Api.Dto.Messaging;

namespace Agrirouter.Api.Service.Messaging
{
    /// <summary>
    ///     Interface for all services encoding their own messages.
    /// </summary>
    /// <typeparam name="T">Type of parameter that should be encoded.</typeparam>
    public interface IEncodeMessageService<in T>
    {
        /// <summary>
        ///     Encoding a message using the given message parameters.
        /// </summary>
        /// <param name="parameters">Parameters to encode.</param>
        /// <returns>The encoded message.</returns>
        EncodedMessage Encode(T parameters);
    }
}