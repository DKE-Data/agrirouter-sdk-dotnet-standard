using com.dke.data.agrirouter.api.service.parameters;

namespace com.dke.data.agrirouter.api.service.messaging
{
    /// <summary>
    /// Service to delete messages from the feed.
    /// </summary>
    public interface IFeedDeleteService : IMessagingService<FeedDeleteParameters>,
        IEncodeMessageService<FeedDeleteParameters>
    {
    }
}