namespace Agrirouter.Sdk.Api.Definitions
{
    /// <summary>
    ///     Chunk size definition for chunking messages.
    /// </summary>
    public class ChunkSizeDefinition
    {
        /// <summary>
        ///     Maximum value the AR can handle.
        /// </summary>
        public static int MaximumSupported => 1000000 - Buffer(1000000);

        /// <summary>
        ///     Half of a megabyte.
        /// </summary>
        public static int HalfOfTheMaximum => MaximumSupported / 2;

        /// <summary>
        ///     Quarter of a megabyte.
        /// </summary>
        public static int QuarterOfTheMaximum => MaximumSupported / 4;

        /// <summary>
        ///     10 % buffer to ensure the AR will not decline the maximum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static int Buffer(int value)
        {
            return (int) (value * 0.05);
        }
    }
}