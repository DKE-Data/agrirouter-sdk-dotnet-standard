using Agrirouter.Api.Service.Parameters;

namespace Agrirouter.Api.Service.Messaging
{
    /// <summary>
    ///     Service to delete messages from the feed.
    /// </summary>
    public interface IFeedDeleteService : IMessagingService<FeedDeleteParameters>,
        IEncodeMessageService<FeedDeleteParameters>
    {
    }
}