namespace Agrirouter.Api.Definitions
{
    /// <summary>
    /// Defining the offset for different timestamp actions.
    /// </summary>
    public class TimestampOffset
    {
        /// <summary>
        /// No offset.
        /// </summary>
        public static long None => 0;

        /// <summary>
        /// Offset of a minute.
        /// </summary>
        public static long OneMinute => 60;

        /// <summary>
        /// Offset of an hour.
        /// </summary>
        public static long OneHour => OneMinute * 60;

        /// <summary>
        /// Offset of one day.
        /// </summary>
        public static long OneDay => OneHour * 24;

        /// <summary>
        /// Offset of two days.
        /// </summary>
        public static long TwoDays => OneDay * 2;

        /// <summary>
        /// Offset of three days.
        /// </summary>
        public static long ThreeDays => OneDay * 3;

        /// <summary>
        /// Offset of four days.
        /// </summary>
        public static long FourDays => OneDay * 4;

        /// <summary>
        /// Offset of five days.
        /// </summary>
        public static long FiveDays => OneDay * 5;

        /// <summary>
        /// Offset of six days-
        /// </summary>
        public static long SixDays => OneDay * 6;

        /// <summary>
        /// Offset of one week.
        /// </summary>
        public static long OneWeek => OneDay * 7;

        /// <summary>
        /// Offset of two weeks.
        /// </summary>
        public static long TwoWeeks => OneWeek * 2;

        /// <summary>
        /// Offset of three weeks.
        /// </summary>
        public static long ThreeWeeks => OneWeek * 3;

        /// <summary>
        /// Offset of four weeks.
        /// </summary>
        public static long FourWeeks => OneWeek * 4;
    }
}