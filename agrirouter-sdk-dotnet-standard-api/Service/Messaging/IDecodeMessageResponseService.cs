using Google.Protobuf.WellKnownTypes;

namespace Agrirouter.Sdk.Api.Service.Messaging
{
    /// <summary>
    ///     Decoding message responses for designated messages types.
    /// </summary>
    /// <typeparam name="T">Type of the message response.</typeparam>
    public interface IDecodeMessageResponseService<out T>
    {
        /// <summary>
        ///     Decoding the given message using the given message parameters.
        /// </summary>
        /// <param name="messageResponse">The response from the AR.</param>
        /// <returns>The decoded message.</returns>
        T Decode(Any messageResponse);
    }
}