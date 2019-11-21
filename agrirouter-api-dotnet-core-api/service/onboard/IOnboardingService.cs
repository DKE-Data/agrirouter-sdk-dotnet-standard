using com.dke.data.agrirouter.api.dto.onboard;
using com.dke.data.agrirouter.api.service.parameters;

namespace com.dke.data.agrirouter.api.service.onboard
{
    public interface IOnboardingService
    {
        OnboardingResponse Onboard(OnboardingParameters parameters);
    }
}