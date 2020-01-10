using com.dke.data.agrirouter.api.service.parameters;

namespace com.dke.data.agrirouter.api.service.messaging
{
    public interface IFeedDeleteService : IMessagingService<FeedDeleteParameters>,
        IEncodeMessageService<FeedDeleteParameters>
    {
    }
}