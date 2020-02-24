using Agrirouter.Api.Service.Parameters;

namespace Agrirouter.Api.Service.Messaging.vcu
{
    /// <summary>
    /// Service to onboard VCUs.
    /// </summary>
    public interface IOffboardVcuService : IMessagingService<OffboardVcuParameters>,
        IEncodeMessageService<OffboardVcuParameters>
    {
    }
}