using com.dke.data.agrirouter.api.service.parameters;

namespace com.dke.data.agrirouter.api.service.messaging
{
    /// <summary>
    /// Service to query messages.
    /// </summary>
    public interface IQueryMessagesService : IMessagingService<QueryMessagesParameters>,
        IEncodeMessageService<QueryMessagesParameters>
    {
    }
}