using System;
using Google.Protobuf.WellKnownTypes;

namespace com.dke.data.agrirouter.impl.service.common
{
    public class UtcDataService
    {
        public string TimeZone =>
            (TimeZoneInfo.Local.BaseUtcOffset < TimeSpan.Zero ? "-" : "+") +
            TimeZoneInfo.Local.BaseUtcOffset.ToString("hh") + ":00";

        public string Now => DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");

        public Timestamp NowAsTimestamp()
        {
            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            return new Timestamp {Seconds = (long) timeSpan.TotalSeconds, Nanos = 1000000};
        }

        public string NowAsUnixTimestamp()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp.ToString();
        }
    }
}