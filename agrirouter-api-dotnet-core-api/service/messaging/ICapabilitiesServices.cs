using com.dke.data.agrirouter.api.service.parameters;

namespace com.dke.data.agrirouter.api.service.messaging
{
    /**
     * Setting capabilities using the special message type.
     */
    public interface ICapabilitiesServices : IMessagingService<CapabilitiesParameters>,
        IEncodeMessageService<CapabilitiesParameters>
    {
    }
}