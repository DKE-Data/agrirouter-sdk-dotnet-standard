using Agrirouter.Sdk.Api.Service.Parameters;

namespace Agrirouter.Sdk.Api.Service.Messaging.Vcu
{
    /// <summary>
    ///     Service to onboard VCUs.
    /// </summary>
    public interface IOnboardVcuService : IMessagingService<OnboardVcuParameters>,
        IEncodeMessageService<OnboardVcuParameters>
    {
    }
}