using Agrirouter.Api.Service.Parameters;

namespace Agrirouter.Api.Service.Messaging
{
    /// <summary>
    ///     Service to send the subscription for messages types.
    /// </summary>
    public interface ISubscriptionService : IMessagingService<SubscriptionParameters>,
        IEncodeMessageService<SubscriptionParameters>
    {
    }
}