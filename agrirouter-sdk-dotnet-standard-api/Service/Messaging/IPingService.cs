using Agrirouter.Api.Service.Parameters;

namespace Agrirouter.Api.Service.Messaging
{
    /// <summary>
    ///     Send a ping message to check if the endpoint still exists.
    /// </summary>
    public interface IPingService : IMessagingService<PingParameters>, IEncodeMessageService<PingParameters>
    {
    }
}