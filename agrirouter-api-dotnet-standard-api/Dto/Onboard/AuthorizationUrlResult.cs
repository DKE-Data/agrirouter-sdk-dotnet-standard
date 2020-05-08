namespace Agrirouter.Api.Dto.Onboard
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class AuthorizationUrlResult
    {
        /// <summary>
        /// URL.
        /// </summary>
        public string AuthorizationUrl { get; set; }

        /// <summary>
        /// State.
        /// </summary>
        public string State { get; set; }
    }
}