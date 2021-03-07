namespace Agrirouter.Api.Dto.Onboard.Inner
{
    /// <summary>
    ///     Data transfer object for the communication.
    /// </summary>
    public class Authentication
    {
        /// <summary>
        ///     Type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Secret.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        ///     Certificate.
        /// </summary>
        public string Certificate { get; set; }
    }
}