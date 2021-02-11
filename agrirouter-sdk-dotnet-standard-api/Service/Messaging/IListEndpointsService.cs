using Agrirouter.Sdk.Api.Service.Parameters;

namespace Agrirouter.Sdk.Api.Service.Messaging
{
    /// <summary>
    ///     Sending a message to list all the connected endpoints within the AR.
    /// </summary>
    public interface IListEndpointsService : IMessagingService<ListEndpointsParameters>,
        IEncodeMessageService<ListEndpointsParameters>
    {
    }
}