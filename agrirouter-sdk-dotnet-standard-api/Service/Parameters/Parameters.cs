namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    ///     Parameter container definition.
    /// </summary>
    public abstract class Parameters
    {
        private static int _applicationMessageSeqNo = 1;

        /// <summary>
        ///     Application message sequence number. Automatically incremented by 1.
        /// </summary>
        public static long ApplicationMessageSeqNo => _applicationMessageSeqNo++;

        /// <summary>
        ///     Message ID.
        /// </summary>
        public string ApplicationMessageId { get; set; }

        /// <summary>
        ///     Teamset context ID.
        /// </summary>
        public string TeamsetContextId { get; set; }
    }
}