using System;
using Google.Protobuf.WellKnownTypes;

namespace com.dke.data.agrirouter.impl.service.common
{
    /**
     * UTC data service.
     */
    public class UtcDataService
    {
        /**
         * Delivering the current time zone.
         */
        public string TimeZone =>
            (TimeZoneInfo.Local.BaseUtcOffset < TimeSpan.Zero ? "-" : "+") +
            TimeZoneInfo.Local.BaseUtcOffset.ToString("hh") + ":00";

        /**
         * Delivering the current date using a valid AR format.
         */
        public string Now => DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");

        /**
         * Delivering the current date using a timestamp format.
         */
        public Timestamp NowAsTimestamp()
        {
            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            return new Timestamp {Seconds = (long) timeSpan.TotalSeconds, Nanos = 1000000};
        }
        /**
         * Delivering the current date using a unix timestamp format.
         */
        public string NowAsUnixTimestamp()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp.ToString();
        }
    }
}