using com.dke.data.agrirouter.api.service.parameters;

namespace com.dke.data.agrirouter.api.service.messaging
{
    /// <summary>
    /// Service to send the subscription for messages types.
    /// </summary>
    public interface ISubscriptionService : IMessagingService<SubscriptionParameters>,
        IEncodeMessageService<SubscriptionParameters>
    {
    }
}