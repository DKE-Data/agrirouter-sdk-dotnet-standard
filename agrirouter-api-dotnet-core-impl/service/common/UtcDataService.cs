using System;

namespace com.dke.data.agrirouter.impl.service.common
{
    public class UtcDataService
    {
        public string TimeZone =>
            (TimeZoneInfo.Local.BaseUtcOffset < TimeSpan.Zero ? "-" : "+") +
            TimeZoneInfo.Local.BaseUtcOffset.ToString("hh") + ":00";

        public string Now => DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
    }
}