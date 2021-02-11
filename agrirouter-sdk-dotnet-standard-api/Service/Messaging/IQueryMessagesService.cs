using Agrirouter.Sdk.Api.Service.Parameters;

namespace Agrirouter.Sdk.Api.Service.Messaging
{
    /// <summary>
    ///     Service to query messages.
    /// </summary>
    public interface IQueryMessagesService : IMessagingService<QueryMessagesParameters>,
        IEncodeMessageService<QueryMessagesParameters>
    {
    }
}