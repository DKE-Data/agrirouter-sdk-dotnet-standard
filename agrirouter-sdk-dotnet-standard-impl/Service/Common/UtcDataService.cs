using System;
using Google.Protobuf.WellKnownTypes;

namespace Agrirouter.Impl.Service.Common
{
    /// <summary>
    ///     Service to provide UTC data like timestamps or string representations.
    /// </summary>
    public class UtcDataService
    {
        /// <summary>
        ///     Delivering the current time zone.
        /// </summary>
        public static string TimeZone =>
            (TimeZoneInfo.Local.BaseUtcOffset < TimeSpan.Zero ? "-" : "+") +
            TimeZoneInfo.Local.BaseUtcOffset.ToString("hh") + ":00";

        /// <summary>
        ///     Delivering the current date using a valid AR format.
        /// </summary>
        public static string Now => DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        
        /// <summary>
        ///     Delivering the current date using a valid AR format incl. a default offset.
        /// </summary>
        /// <returns></returns>
        public static string NowInclOffset() => SecondsInThePastFromNow(10);

        /// <summary>
        ///     Delivering the current date with an offset in seconds using a valid AR format.
        /// </summary>
        public static string SecondsInThePastFromNow(int offset)
        {
            return DateTime.UtcNow.AddSeconds(offset * -1).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }

        /// <summary>
        ///     Delivering the current date using a timestamp format.
        /// </summary>
        /// <returns></returns>
        public static Timestamp NowAsTimestamp()
        {
            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            return new Timestamp { Seconds = (long)timeSpan.TotalSeconds, Nanos = 1000000 };
        }

        /// <summary>
        ///     Delivering the current date using a timestamp format.
        /// </summary>
        /// <param name="offset">The current offset.</param>
        /// <returns>-</returns>
        public static Timestamp Timestamp(long offset)
        {
            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            return new Timestamp { Seconds = (long)timeSpan.TotalSeconds - offset, Nanos = 1000000 };
        }

        /// <summary>
        ///     Delivering the current date using a unix timestamp format.
        /// </summary>
        /// <returns>-</returns>
        public static string NowAsUnixTimestamp()
        {
            var unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            return unixTimestamp.ToString();
        }
    }
}