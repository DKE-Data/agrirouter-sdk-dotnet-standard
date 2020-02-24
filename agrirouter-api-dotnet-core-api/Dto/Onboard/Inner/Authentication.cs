namespace Agrirouter.Api.Dto.Onboard.Inner
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