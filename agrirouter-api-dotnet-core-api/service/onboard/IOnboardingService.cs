using com.dke.data.agrirouter.api.dto.onboard;
using com.dke.data.agrirouter.api.service.parameters;

namespace com.dke.data.agrirouter.api.service.onboard
{
    /**
     * Onboarding service.
     */
    public interface IOnboardingService
    {
        /**
         * Onboarding for endpoint that do not use the secured onboarding process.
         */
        OnboardingResponse Onboard(OnboardingParameters onboardingParameters);
    }
}