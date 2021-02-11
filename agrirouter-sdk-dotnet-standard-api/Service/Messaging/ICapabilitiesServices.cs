using Agrirouter.Sdk.Api.Service.Parameters;

namespace Agrirouter.Sdk.Api.Service.Messaging
{
    /// <summary>
    ///     Service to set the capabilities for an endpoint.
    /// </summary>
    public interface ICapabilitiesServices : IMessagingService<CapabilitiesParameters>,
        IEncodeMessageService<CapabilitiesParameters>
    {
    }
}