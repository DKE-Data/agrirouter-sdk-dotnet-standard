namespace com.dke.data.agrirouter.api.dto.onboard.inner
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class Authentication
    {
        public string Type { get; set; }

        public string Secret { get; set; }

        public string Certificate { get; set; }
    }
}