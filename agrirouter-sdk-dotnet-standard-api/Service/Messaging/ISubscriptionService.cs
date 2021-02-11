using Agrirouter.Sdk.Api.Service.Parameters;

namespace Agrirouter.Sdk.Api.Service.Messaging
{
    /// <summary>
    ///     Service to send the subscription for messages types.
    /// </summary>
    public interface ISubscriptionService : IMessagingService<SubscriptionParameters>,
        IEncodeMessageService<SubscriptionParameters>
    {
    }
}