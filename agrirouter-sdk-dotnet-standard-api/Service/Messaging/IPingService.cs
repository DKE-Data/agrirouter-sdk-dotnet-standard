using System.Threading.Tasks;
using Agrirouter.Api.Dto.Messaging;
using Agrirouter.Api.Service.Parameters;

namespace Agrirouter.Api.Service.Messaging
{
    /// <summary>
    ///     Send a ping message to check if the endpoint still exists.
    /// </summary>
    public interface IPingService : IMessagingService<PingParameters>
    {
        /// <summary>
        ///     Please see base class declaration for documentation.
        /// </summary>
        /// <param name="pingParameters">-</param>
        /// <returns>-</returns>
        MessagingResult Send(PingParameters pingParameters);

        /// <summary>
        ///     Please see base class declaration for documentation.
        /// </summary>
        /// <param name="pingParameters">-</param>
        /// <returns>-</returns>
        Task<MessagingResult> SendAsync(PingParameters pingParameters);

        /// <summary>
        ///     Please see <seealso cref="IEncodeMessageService{T}.Encode" /> for documentation.
        /// </summary>
        /// <param name="pingParameters">-</param>
        /// <returns>-</returns>
        EncodedMessage Encode(PingParameters pingParameters);
    }
}