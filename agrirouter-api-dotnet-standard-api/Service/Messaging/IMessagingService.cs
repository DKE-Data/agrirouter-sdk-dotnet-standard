using Agrirouter.Api.Dto.Messaging;

namespace Agrirouter.Api.Service.Messaging
{
    /// <summary>
    ///     Interface for all services sending messages.
    /// </summary>
    /// <typeparam name="T">Type of the parameters.</typeparam>
    public interface IMessagingService<in T>
    {
        /// <summary>
        ///     Sending a message using the given message parameters.
        /// </summary>
        /// <param name="parameters">Parameters for message sending.</param>
        /// <returns>-</returns>
        MessagingResult Send(T parameters);
    }
}