namespace Agrirouter.Api.Dto.Onboard
{
    /// <summary>
    /// Data transfer object for the communication.
    /// </summary>
    public class AuthorizationUrlResult
    {
        public string AuthorizationUrl { get; set; }
        public string State { get; set; }
    }
}