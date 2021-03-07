using Agrirouter.Api.Service.Parameters;

namespace Agrirouter.Api.Service.Messaging
{
    /// <summary>
    ///     Sending a message to list all the connected endpoints within the AR.
    /// </summary>
    public interface IListEndpointsService : IMessagingService<ListEndpointsParameters>,
        IEncodeMessageService<ListEndpointsParameters>
    {
    }
}