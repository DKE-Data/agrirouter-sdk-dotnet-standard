namespace Agrirouter.Api.Service.Parameters
{
    /// <summary>
    /// Parameter container definition.
    /// </summary>
    public abstract class Parameters
    {
        private static int _applicationMessageSeqNo = 1;

        public static long ApplicationMessageSeqNo => _applicationMessageSeqNo++;

        public string ApplicationMessageId { get; set; }

        public string TeamsetContextId { get; set; }
    }
}