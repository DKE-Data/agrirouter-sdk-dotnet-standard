using Agrirouter.Api.Dto.Onboard;
using Agrirouter.Api.Service.Parameters;
using Agrirouter.Cloud.Registration;

namespace Agrirouter.Api.Service.Messaging.Vcu
{
    /// <summary>
    ///     Service to onboard VCUs.
    /// </summary>
    public interface IOnboardVcuService : IMessagingService<OnboardVcuParameters>,
        IEncodeMessageService<OnboardVcuParameters>
    {
        /// <summary>
        /// Enhancing the virtual endpoint with the data from the parent to have a fully usable endpoint.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="virtualCommunicationUnit">The virtual communication unit.</param>
        /// <returns></returns>
        OnboardResponse EnhanceVirtualCommunicationToFullyUsableOnboardResponse(OnboardResponse parent,
            OnboardingResponse.Types.EndpointRegistrationDetails virtualCommunicationUnit);
    }
}