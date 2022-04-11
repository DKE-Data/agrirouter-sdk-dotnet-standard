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
        ///     Overrides the the computer time and instead allways return the given time.
        /// </summary>
        public static DateTime? UtcNowOverride { get; set; } = null;

        /// <summary>
        ///     Adds an offset to the computer time.
        /// </summary>
        public static TimeSpan UtcNowOffset { get; set; } = TimeSpan.Zero;

        /// <summary>
        ///     Gets an adjusted UtcNow value that uses UtcNowOverride and UtcNowOffset to calculate the time to return.
        /// </summary>
        public static DateTime UtcNow => UtcNowOverride ?? DateTime.UtcNow.Add(UtcNowOffset);

        /// <summary>
        ///     Delivering the current time zone.
        /// </summary>
        public static string TimeZone =>
            (TimeZoneInfo.Local.BaseUtcOffset < TimeSpan.Zero ? "-" : "+") +
            TimeZoneInfo.Local.BaseUtcOffset.ToString("hh") + ":00";

        /// <summary>
        ///     Delivering the current date using a valid AR format.
        /// </summary>
        public static string Now => UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");

        /// <summary>
        ///     Delivering the current date using a timestamp format.
        /// </summary>
        /// <returns></returns>
        public static Timestamp NowAsTimestamp()
        {
            var timeSpan = UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            return new Timestamp {Seconds = (long) timeSpan.TotalSeconds, Nanos = 1000000};
        }

        /// <summary>
        ///     Delivering the current date using a timestamp format.
        /// </summary>
        /// <param name="offset">The current offset.</param>
        /// <returns>-</returns>
        public static Timestamp Timestamp(long offset)
        {
            var timeSpan = UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            return new Timestamp {Seconds = (long) timeSpan.TotalSeconds - offset, Nanos = 1000000};
        }

        /// <summary>
        ///     Delivering the current date using a unix timestamp format.
        /// </summary>
        /// <returns>-</returns>
        public static string NowAsUnixTimestamp()
        {
            var unixTimestamp = (int) UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            return unixTimestamp.ToString();
        }
    }
}