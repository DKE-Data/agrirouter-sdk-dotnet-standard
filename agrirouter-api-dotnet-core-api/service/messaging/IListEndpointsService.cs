using com.dke.data.agrirouter.api.service.parameters;

namespace com.dke.data.agrirouter.api.service.messaging
{
    /**
     * Sending a message to list all the connected endpoints within the AR.
     */
    public interface IListEndpointsService : IMessagingService<ListEndpointsParameters>,
        IEncodeMessageService<ListEndpointsParameters>
    {
    }
}