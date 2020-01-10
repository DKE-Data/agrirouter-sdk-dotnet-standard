namespace com.dke.data.agrirouter.api.definitions
{
    /**
     * Defining the offset for different timestamp actions.
     */
    public class TimestampOffset
    {
        public static long None => 0;

        public static long OneMinute => 60;

        public static long OneHour => OneMinute * 60;

        public static long OneDay => OneHour * 24;
        public static long TwoDays => OneDay * 2;
        public static long ThreeDays => OneDay * 3;
        public static long FourDays => OneDay * 4;
        public static long FiveDays => OneDay * 5;
        public static long SixDays => OneDay * 6;

        public static long OneWeek => OneDay * 7;
        public static long TwoWeeks => OneWeek * 2;
        public static long ThreeWeeks => OneWeek * 3;
        public static long FourWeeks => OneWeek * 4;
    }
}