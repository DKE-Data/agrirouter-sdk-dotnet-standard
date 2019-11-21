using System;
using com.dke.data.agrirouter.api.dto.onboard.inner;

namespace com.dke.data.agrirouter.api.dto.onboard
{
    public class OnboardingResponse
    {
        String DeviceAlternateId { get; set; }

        String CapabilityAlternateId { get; set; }

        String SensorAlternateId { get; set; }

        ConnectionCriteria ConnectionCriteria { get; set; }

        Authentication Authentication { get; set; }
    }
}