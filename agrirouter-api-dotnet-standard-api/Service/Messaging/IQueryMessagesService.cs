using Agrirouter.Api.Service.Parameters;

namespace Agrirouter.Api.Service.Messaging
{
    /// <summary>
    /// Service to query messages.
    /// </summary>
    public interface IQueryMessagesService : IMessagingService<QueryMessagesParameters>,
        IEncodeMessageService<QueryMessagesParameters>
    {
    }
}