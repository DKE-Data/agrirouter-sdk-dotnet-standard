using com.dke.data.agrirouter.api.service.parameters;

namespace com.dke.data.agrirouter.api.service.messaging
{
    /// <summary>
    /// Service to set the capabilities for an endpoint.
    /// </summary>
    public interface ICapabilitiesServices : IMessagingService<CapabilitiesParameters>,
        IEncodeMessageService<CapabilitiesParameters>
    {
    }
}