namespace com.dke.data.agrirouter.api.dto.onboard.inner
{
    /**
     * Data transfer object for the communication.
     */
    public class Authentication
    {
        public string Type { get; set; }

        public string Secret { get; set; }

        public string Certificate { get; set; }
    }
}