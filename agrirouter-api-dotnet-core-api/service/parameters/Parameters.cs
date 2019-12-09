namespace com.dke.data.agrirouter.api.service.parameters
{
    public abstract class Parameters
    {
        public string ApplicationMessageId { get; set; }

        public string TeamsetContextId { get; set; }

        public int ApplicationMessageSeqNo { get; set; }
    }
}