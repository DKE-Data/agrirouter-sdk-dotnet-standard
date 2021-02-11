using Agrirouter.Sdk.Api.Service.Parameters;

namespace Agrirouter.Sdk.Api.Service.Messaging
{
    /// <summary>
    ///     Service to confirm messages within the feed.
    /// </summary>
    public interface IFeedConfirmService : IMessagingService<FeedConfirmParameters>,
        IEncodeMessageService<FeedConfirmParameters>
    {
    }
}